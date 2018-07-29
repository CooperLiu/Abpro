using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace Abpro.WebApiClient
{
    public abstract class HttpMessageHandlerBuilder
    {

        public abstract string Name { get; set; }

        public abstract HttpMessageHandler PrimaryHandler { get; set; }

        public abstract IList<DelegatingHandler> AdditionalHandlers { get; }

        public abstract HttpMessageHandler Build();

        protected internal static HttpMessageHandler CreateHandlerPipeline(HttpMessageHandler primaryHandler, IEnumerable<DelegatingHandler> additionalHandlers)
        {
            if (primaryHandler == null) throw new ArgumentNullException(nameof(primaryHandler));
            if (additionalHandlers == null) throw new ArgumentNullException(nameof(additionalHandlers));

            var additionalHandlersList = additionalHandlers as IReadOnlyList<DelegatingHandler> ?? additionalHandlers.ToArray();

            var next = primaryHandler;
            for (int i = additionalHandlersList.Count - 1; i >= 0; i--)
            {
                var handler = additionalHandlersList[i];

                if (handler == null)
                {
                    throw new InvalidOperationException($"the additional {nameof(additionalHandlers)} must not contain null value");
                }

                if (handler.InnerHandler != null)
                {
                    throw new InvalidOperationException($"the {nameof(DelegatingHandler.InnerHandler)} must be null.");
                }


                handler.InnerHandler = next;
                next = handler;
            }

            return next;

        }
    }

    internal class DefaultHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
    {
        private string _name;

        public override string Name
        {
            get => _name;
            set
            {
                _name = value ?? throw new ArgumentNullException(nameof(value));
            }
        }

        public override HttpMessageHandler PrimaryHandler { get; set; } = new HttpClientHandler(); //need some more configuration 

        public override IList<DelegatingHandler> AdditionalHandlers => new List<DelegatingHandler>();

        public override HttpMessageHandler Build()
        {
            if (PrimaryHandler == null) throw new InvalidOperationException($"{PrimaryHandler} must not be null");

            return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
        }
    }

    /// <summary>
    /// Used by the <see cref="DefaultHttpClientFactory"/> to apply additional initialization to the configure the 
    /// <see cref="HttpMessageHandlerBuilder"/> immediately before <see cref="HttpMessageHandlerBuilder.Build()"/>
    /// is called.
    /// </summary>
    public interface IHttpMessageHandlerBuilderFilter
    {
        /// <summary>
        /// Applies additional initialization to the <see cref="HttpMessageHandlerBuilder"/>
        /// </summary>
        /// <param name="next">A delegate which will run the next <see cref="IHttpMessageHandlerBuilderFilter"/>.</param>
        Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next);
    }

    internal class LoggingHttpMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggingHttpMessageHandlerBuilderFilter(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));

            return (builder) =>
            {
                next(builder);

                var loggerName = !string.IsNullOrEmpty(builder.Name) ? builder.Name : "Default";

                // We want all of our logging message to show up as-if they are coming from HttpClient,
                // but also to include the name of the client for more fine-grained control.
                var outerLogger = _loggerFactory.Create($"System.Net.Http.HttpClient.{loggerName}.LogicalHandler");
                var innerLogger = _loggerFactory.Create($"System.Net.Http.HttpClient.{loggerName}.ClientHandler");

                //// The 'scope' handler goes first so it can surround everything.
                //builder.AdditionalHandlers.Insert(0, new LoggingScopeHttpMessageHandler(outerLogger));

                //// We want this handler to be last so we can log details about the request after
                //// service discovery and security happen.
                //builder.AdditionalHandlers.Add(new LoggingHttpMessageHandler(innerLogger));

            };
        }
    }
}
