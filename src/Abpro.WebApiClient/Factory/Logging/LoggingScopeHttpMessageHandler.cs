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
    public class LoggingScopeHttpMessageHandler : DelegatingHandler
    {
        private ILogger _logger;

        public LoggingScopeHttpMessageHandler(ILogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _logger = logger;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var stopwatch = ValueStopwatch.StartNew();

            using (Log.BeginRequestPipelineScope(_logger, request))
            {
                Log.RequestPipelineStart(_logger, request);
                var response = await base.SendAsync(request, cancellationToken);
                Log.RequestPipelineEnd(_logger, response, stopwatch.GetElapsedTime());

                return response;
            }
        }

        private static class Log
        {
            private static readonly Func<ILogger, HttpMethod, Uri, IDisposable> _beginRequestPipelineScope =
                (logger, httpMethod, uri) =>
                {
                    logger.Debug($"HTTP {httpMethod} {uri}");

                    return new Scope();
                };

            private static readonly Action<ILogger, HttpMethod, Uri, Exception> _requestPipelineStart = (logger, httpMethod, uri, e) => { logger.Debug($"Start processing HTTP request {httpMethod} {uri}", e); };

            private static readonly Action<ILogger, double, HttpStatusCode, Exception> _requestPipelineEnd = (logger, elapsedMilliseconds, statusCode, e) => { logger.Debug($"End processing HTTP request after {elapsedMilliseconds}ms - {statusCode}", e); };


            public static IDisposable BeginRequestPipelineScope(ILogger logger, HttpRequestMessage request)
            {
                return _beginRequestPipelineScope(logger, request.Method, request.RequestUri);
            }

            public static void RequestPipelineStart(ILogger logger, HttpRequestMessage request)
            {
                _requestPipelineStart(logger, request.Method, request.RequestUri, null);

                if (logger.IsDebugEnabled)
                {
                    logger.Debug(new HttpHeadersLogValue(HttpHeadersLogValue.Kind.Request, request.Headers, request.Content?.Headers).ToString());

                }
            }

            public static void RequestPipelineEnd(ILogger logger, HttpResponseMessage response, TimeSpan duration)
            {
                _requestPipelineEnd(logger, duration.TotalMilliseconds, response.StatusCode, null);

                if (logger.IsDebugEnabled)
                {
                    logger.Debug(new HttpHeadersLogValue(HttpHeadersLogValue.Kind.Response, response.Headers, response.Content?.Headers).ToString());
                }
            }
        }

        private class Scope : IDisposable
        {
            public void Dispose()
            {

            }
        }
    }


}