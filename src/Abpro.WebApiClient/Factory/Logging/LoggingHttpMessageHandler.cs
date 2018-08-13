// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core.Logging;

namespace Abpro.WebApiClient.Factory.Logging
{
    public class LoggingHttpMessageHandler : DelegatingHandler
    {
        private ILogger _logger;

        public LoggingHttpMessageHandler(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var stopwatch = ValueStopwatch.StartNew();

            // Not using a scope here because we always expect this to be at the end of the pipeline, thus there's
            // not really anything to surround.
            Log.RequestStart(_logger, request);
            var response = await base.SendAsync(request, cancellationToken);
            Log.RequestEnd(_logger, response, stopwatch.GetElapsedTime());

            return response;
        }

        private static class Log
        {
            private static readonly Action<ILogger, HttpMethod, Uri, Exception> _requestStart = (logger, httpMethod, uri, e) => { logger.Debug($"Sending HTTP request {httpMethod} {uri}", e); };

            private static readonly Action<ILogger, double, HttpStatusCode, Exception> _requestEnd = (logger, elapsedMilliseconds, statusCode, e) => { logger.Debug($"Received HTTP response after {elapsedMilliseconds}ms - {statusCode}", e); };
            public static void RequestStart(ILogger logger, HttpRequestMessage request)
            {
                _requestStart(logger, request.Method, request.RequestUri, null);

                if (logger.IsDebugEnabled)
                {
                    logger.Debug(new HttpHeadersLogValue(HttpHeadersLogValue.Kind.Request, request.Headers, request.Content?.Headers).ToString());
                }
            }

            public static void RequestEnd(ILogger logger, HttpResponseMessage response, TimeSpan duration)
            {
                _requestEnd(logger, duration.TotalMilliseconds, response.StatusCode, null);

                if (logger.IsDebugEnabled)
                {
                    logger.Debug(new HttpHeadersLogValue(HttpHeadersLogValue.Kind.Response, response.Headers, response.Content?.Headers).ToString());
                }
            }
        }
    }
}