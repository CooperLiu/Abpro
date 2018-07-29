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

        private readonly IHttpClientFactoryDependency _dependency;

        public DefaultHttpClientFactory(ILoggerFactory loggerFactory, IHttpClientFactoryDependency dependency)
        {
            _logger = loggerFactory.Create(typeof(DefaultHttpClientFactory));
            _activeHandlers = new ConcurrentDictionary<string, Lazy<ActiveHandlerTrackingEntry>>(StringComparer.Ordinal);
            _dependency = dependency;

            _entryFactory = name =>
            {
                return new Lazy<ActiveHandlerTrackingEntry>(() =>
                {
                    return CreateHandlerEntry(name);
                }, LazyThreadSafetyMode.ExecutionAndPublication);
            };
        }

        private ActiveHandlerTrackingEntry CreateHandlerEntry(string name)
        {
            var httpMessageHandlerBuilder = _dependency.IocContainer.Resolve<HttpMessageHandlerBuilder>();

            httpMessageHandlerBuilder.Name = name ?? throw new ArgumentNullException(nameof(name));

            //var options = _dependency.IocContainer.Resolve<HttpClientFactoryOptions>();// how to get options for factory,and how to new instance of options for factory?

        }


        public HttpClient CreateClient(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            throw new NotImplementedException();
        }
    }


}