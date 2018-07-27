using System;
using System.Collections.Concurrent;
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

        public DefaultHttpClientFactory(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.Create(typeof(DefaultHttpClientFactory));
            _activeHandlers = new ConcurrentDictionary<string, Lazy<ActiveHandlerTrackingEntry>>(StringComparer.Ordinal);

            _entryFactory = name =>
            {
                return new Lazy<ActiveHandlerTrackingEntry>(() => { return CreateHandlerEntry(name);},LazyThreadSafetyMode.ExecutionAndPublication);
            };
        }

        private ActiveHandlerTrackingEntry CreateHandlerEntry(string name)
        {
            throw new NotImplementedException();
        }


        public HttpClient CreateClient(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            throw new NotImplementedException();
        }
    }


}