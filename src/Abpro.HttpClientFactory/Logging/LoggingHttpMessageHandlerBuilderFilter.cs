using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Abpro.HttpClientFactory.Logging
{
    public class LoggingHttpMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
    {
        private readonly ILoggerFactory _loggerFactory;

        public LoggingHttpMessageHandlerBuilderFilter(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            _loggerFactory = loggerFactory;
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
                var outerLogger = _loggerFactory.CreateLogger($"System.Net.Http.HttpClient.{loggerName}.AuditingHandler");
                //var innerLogger = _loggerFactory.Create($"System.Net.Http.HttpClient.{loggerName}.ClientHandler");

                // The 'scope' handler goes first so it can surround everything.
                builder.AdditionalHandlers.Add(new LoggingAuditingHttpMessageHandler(outerLogger));

                // We want this handler to be last so we can log details about the request after
                // service discovery and security happen.
                // builder.AdditionalHandlers.Add(new LoggingHttpMessageHandler(innerLogger));

            };
        }
    }
}
