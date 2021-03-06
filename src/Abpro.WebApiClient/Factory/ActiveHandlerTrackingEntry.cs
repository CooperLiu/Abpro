﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace Abpro.WebApiClient.Factory
{
    // Thread-safety: We treat this class as immutable except for the timer. Creating a new object
    // for the 'expiry' pool simplifies the threading requirements significantly.
    internal class ActiveHandlerTrackingEntry
    {
        private readonly object _lock;
        private bool _timerInitialized;
        private Timer _timer;
        private TimerCallback _callback;

        public ActiveHandlerTrackingEntry(string name, LifetimeTrackingHttpMessageHandler handler, TimeSpan lifetime)
        {
            Name = name;
            Handler = handler;
            Lifetime = lifetime;

            _lock = new object();
        }

        public LifetimeTrackingHttpMessageHandler Handler { get; private set; }

        public TimeSpan Lifetime { get; }

        public string Name { get; }

        public void StartExpiryTimer(TimerCallback callback)
        {
            if (Lifetime == Timeout.InfiniteTimeSpan)
            {
                return; // never expires.
            }

            if (Volatile.Read(ref _timerInitialized))
            {
                return;
            }

            StartExpiryTimerSlow(callback);
        }

        private void StartExpiryTimerSlow(TimerCallback callback)
        {
            Debug.Assert(Lifetime != Timeout.InfiniteTimeSpan);

            lock (_lock)
            {
                if (Volatile.Read(ref _timerInitialized))
                {
                    return;
                }

                _callback = callback;
                _timer = new Timer(Timer_Tick, null, Lifetime, Timeout.InfiniteTimeSpan);

                Volatile.Write(ref _timerInitialized, true);
            }
        }

        private void Timer_Tick(object state)
        {
            Debug.Assert(_callback != null);
            Debug.Assert(_timer != null);

            lock (_lock)
            {
                _timer.Dispose();
                _timer = null;

                _callback(this);
            }
        }
    }

    internal class LifetimeTrackingHttpMessageHandler : DelegatingHandler
    {
        public LifetimeTrackingHttpMessageHandler(HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
        }
    }


    // Thread-safety: This class is immutable
    internal class ExpiredHandlerTrackingEntry
    {
        private readonly WeakReference _livenessTracker;

        public ExpiredHandlerTrackingEntry(ActiveHandlerTrackingEntry other)
        {
            Name = other.Name;

            _livenessTracker = new WeakReference(other.Handler);
            InnerHandler = other.Handler.InnerHandler;
        }

        public bool CanDispose => !_livenessTracker.IsAlive;

        public HttpMessageHandler InnerHandler { get; }

        public string Name { get; }
    }
}