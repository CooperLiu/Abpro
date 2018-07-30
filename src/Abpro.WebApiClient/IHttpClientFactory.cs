using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Castle.Core.Logging;

namespace Abpro.WebApiClient
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(string name);
    }

    public class DefaultHttpClientFactory : IHttpClientFactory
    {
        private readonly ILogger _logger;

        private readonly ConcurrentDictionary<string, Lazy<ActiveHandlerTrackingEntry>> _activeHandlers;

        private readonly Func<string, Lazy<ActiveHandlerTrackingEntry>> _entryFactory;

        private readonly IHttpClientFactoryDependency _dependency;
        private TimerCallback _expiryCallback;

        internal readonly ConcurrentQueue<ExpiredHandlerTrackingEntry> _expiredHandlers;

        private readonly object _cleanupTimerLock;

        private Timer _cleanupTimer;

        // Default time of 10s for cleanup seems reasonable.
        // Quick math:
        // 10 distinct named clients * expiry time >= 1s = approximate cleanup queue of 100 items
        //
        // This seems frequent enough. We also rely on GC occurring to actually trigger disposal.
        private readonly TimeSpan _defaultCleanupInterval = TimeSpan.FromSeconds(10);
        private readonly object _cleanupActiveLock;
        private TimerCallback _cleanupCallback;

        private readonly IEnumerable<IHttpMessageHandlerBuilderFilter> _filters;




        public DefaultHttpClientFactory(
            ILoggerFactory loggerFactory,
            IHttpClientFactoryDependency dependency,
            IEnumerable<IHttpMessageHandlerBuilderFilter> filters)
        {
            _logger = loggerFactory.Create(typeof(DefaultHttpClientFactory));
            _activeHandlers = new ConcurrentDictionary<string, Lazy<ActiveHandlerTrackingEntry>>(StringComparer.Ordinal);
            _dependency = dependency;
            _filters = filters;

            _entryFactory = name =>
            {
                return new Lazy<ActiveHandlerTrackingEntry>(() =>
                {
                    return CreateHandlerEntry(name);
                }, LazyThreadSafetyMode.ExecutionAndPublication);
            };

            _expiryCallback = ExpiryTimer_Tick;

            _cleanupCallback = CleanupCallback;

            _expiredHandlers = new ConcurrentQueue<ExpiredHandlerTrackingEntry>();

            _cleanupTimerLock = new object();
            _cleanupActiveLock = new object();

        }

        private void CleanupCallback(object state)
        {
            // Stop any pending timers, we'll restart the timer if there's anything left to process after cleanup.
            //
            // With the scheme we're using it's possible we could end up with some redundant cleanup operations.
            // This is expected and fine.
            // 
            // An alternative would be to take a lock during the whole cleanup process. This isn't ideal because it
            // would result in threads executing ExpiryTimer_Tick as they would need to block on cleanup to figure out
            // whether we need to start the timer.
            StopCleanupTimer();

            try
            {
                if (!Monitor.TryEnter(_cleanupActiveLock))
                {
                    // We don't want to run a concurrent cleanup cycle. This can happen if the cleanup cycle takes
                    // a long time for some reason. Since we're running user code inside Dispose, it's definitely
                    // possible.
                    //
                    // If we end up in that position, just make sure the timer gets started again. It should be cheap
                    // to run a 'no-op' cleanup.
                    StartCleanupTimer();
                    return;
                }

                var initialCount = _expiredHandlers.Count;
                //Log.CleanupCycleStart(_logger, initialCount);

                var stopwatch = ValueStopwatch.StartNew();

                var disposedCount = 0;
                for (var i = 0; i < initialCount; i++)
                {
                    // Since we're the only one removing from _expired, TryDequeue must always succeed.
                    _expiredHandlers.TryDequeue(out var entry);
                    Debug.Assert(entry != null, "Entry was null, we should always get an entry back from TryDequeue");

                    if (entry.CanDispose)
                    {
                        try
                        {
                            entry.InnerHandler.Dispose();
                            disposedCount++;
                        }
                        catch (Exception ex)
                        {
                            //Log.CleanupItemFailed(_logger, entry.Name, ex);
                        }
                    }
                    else
                    {
                        // If the entry is still live, put it back in the queue so we can process it 
                        // during the next cleanup cycle.
                        _expiredHandlers.Enqueue(entry);
                    }
                }

                //Log.CleanupCycleEnd(_logger, stopwatch.GetElapsedTime(), disposedCount, _expiredHandlers.Count);
            }
            finally
            {
                Monitor.Exit(_cleanupActiveLock);
            }

            // We didn't totally empty the cleanup queue, try again later.
            if (_expiredHandlers.Count > 0)
            {
                StartCleanupTimer();
            }
        }

        // Internal so it can be overridden in tests
        internal virtual void StopCleanupTimer()
        {
            lock (_cleanupTimerLock)
            {
                _cleanupTimer.Dispose();
                _cleanupTimer = null;
            }
        }

        void ExpiryTimer_Tick(object state)
        {
            var active = (ActiveHandlerTrackingEntry)state;

            // The timer callback should be the only one removing from the active collection. If we can't find
            // our entry in the collection, then this is a bug.
            var removed = _activeHandlers.TryRemove(active.Name, out var found);

            // At this point the handler is no longer 'active' and will not be handed out to any new clients.
            // However we haven't dropped our strong reference to the handler, so we can't yet determine if
            // there are still any other outstanding references (we know there is at least one).
            //
            // We use a different state object to track expired handlers. This allows any other thread that acquired
            // the 'active' entry to use it without safety problems.
            var expired = new ExpiredHandlerTrackingEntry(active);
            _expiredHandlers.Enqueue(expired);

            StartCleanupTimer();

        }

        void StartCleanupTimer()
        {
            lock (_cleanupTimerLock)
            {
                if (_cleanupTimer == null)
                {
                    _cleanupTimer = new Timer(_cleanupCallback, null, _defaultCleanupInterval, Timeout.InfiniteTimeSpan);
                }
            }
        }

        private ActiveHandlerTrackingEntry CreateHandlerEntry(string name)
        {
            var builder = _dependency.IocContainer.Resolve<HttpMessageHandlerBuilder>();

            builder.Name = name ?? throw new ArgumentNullException(nameof(name));

            var filterArray = _filters.ToArray();

            /*
             * HttpClient 配置操作
             *
             * HttpClient 下，在构建HttpMessageHandler时 配置操作
             *
             * HttpClient 下，在构建HttpMessageHandler前，配置操作
             */

            var options = _dependency.IocContainer.Resolve<HttpClientFactoryOptions>(name);//TODO:如何注册Options

            // This is similar to the initialization pattern in:
            // https://github.com/aspnet/Hosting/blob/e892ed8bbdcd25a0dafc1850033398dc57f65fe1/src/Microsoft.AspNetCore.Hosting/Internal/WebHost.cs#L188
            Action<HttpMessageHandlerBuilder> configure = Configure;

            for (var i = filterArray.Length - 1; i >= 0; i--)
            {
                configure = filterArray[i].Configure(configure);
            }

            configure(builder);

            // Wrap the handler so we can ensure the inner handler outlives the outer handler.
            var handler = new LifetimeTrackingHttpMessageHandler(builder.Build());

            // Note that we can't start the timer here. That would introduce a very very subtle race condition
            // with very short expiry times. We need to wait until we've actually handed out the handler once
            // to start the timer.
            // 
            // Otherwise it would be possible that we start the timer here, immediately expire it (very short
            // timer) and then dispose it without ever creating a client. That would be bad. It's unlikely
            // this would happen, but we want to be sure.
            return new ActiveHandlerTrackingEntry(name, handler, options.HandlerLifetime);


            void Configure(HttpMessageHandlerBuilder b)
            {
                for (var i = 0; i < options.HttpMessageHandlerBuilderActions.Count; i++)
                {
                    options.HttpMessageHandlerBuilderActions[i](b);
                }
            }

        }

        public HttpClient CreateClient(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var entry = _activeHandlers.GetOrAdd(name, _entryFactory).Value;

            var client = new HttpClient(entry.Handler, false);

            StartHandlerEntryTimer(entry);

            var options = _dependency.IocContainer.Resolve<HttpClientFactoryOptions>(name); 

            for (var i = 0; i < options.HttpClientActions.Count; i++)
            {
                options.HttpClientActions[i](client);
            }

            return client;

        }

        private void StartHandlerEntryTimer(ActiveHandlerTrackingEntry entry)
        {
            entry.StartExpiryTimer(_expiryCallback);
        }
    }


}