using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Logging;

namespace Abpro.WebApiClient.Factory
{
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
        Action<IHttpMessageHandlerBuilder> Configure(Action<IHttpMessageHandlerBuilder> next);
    }


    internal class LoggingHttpMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggingHttpMessageHandlerBuilderFilter(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        public Action<IHttpMessageHandlerBuilder> Configure(Action<IHttpMessageHandlerBuilder> next)
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