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

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var stopwatch = ValueStopwatch.StartNew();
            var trackId = Guid.NewGuid().ToString("N");

            using (Log.BeginRequestPipelineScope(_logger, trackId, request))
            {
                Log.RequestPipelineStart(_logger, trackId, request, await request.Content.ReadAsStringAsync());
                var response = await base.SendAsync(request, cancellationToken);
                Log.RequestPipelineEnd(_logger, trackId, response, await response.Content.ReadAsStringAsync(), stopwatch.GetElapsedTime());

                return response;
            }
        }

        private static class Log
        {
            private static readonly Func<ILogger, string, HttpMethod, Uri, IDisposable> _beginRequestPipelineScope =
                (logger, trackId, httpMethod, uri) =>
                {
                    logger.Info($"HTTP Request ({trackId}) {httpMethod} {uri}");

                    return new Scope();
                };

            private static readonly Action<ILogger, string, HttpMethod, Uri, string, string, Exception> _requestPipelineStart = (logger, trackId, httpMethod, uri, requestHeader, requestBody, e) => { logger.Debug($"Start processing HTTP request ({trackId}) {httpMethod} {uri} \r\n {requestHeader}  Request Body:\r\n {requestBody}", e); };

            private static readonly Action<ILogger, string, double, HttpStatusCode, string, string, Exception> _requestPipelineEnd = (logger, trackId, elapsedMilliseconds, statusCode, responseHeader, responseBody, e) => { logger.Debug($"End processing HTTP request ({trackId}) after {elapsedMilliseconds}ms - {statusCode} \r\n {responseHeader} Response Body:\r\n {responseBody}", e); };


            public static IDisposable BeginRequestPipelineScope(ILogger logger, string trackId, HttpRequestMessage request)
            {
                return _beginRequestPipelineScope(logger, trackId, request.Method, request.RequestUri);
            }

            public static void RequestPipelineStart(ILogger logger, string trackId, HttpRequestMessage request, string requestBody)
            {
                _requestPipelineStart(logger, trackId, request.Method, request.RequestUri, new HttpHeadersLogValue(HttpHeadersLogValue.Kind.Request, request.Headers, request.Content?.Headers).ToString(), requestBody, null);
            }

            public static void RequestPipelineEnd(ILogger logger, string trackId, HttpResponseMessage response, string responseBody, TimeSpan duration)
            {
                _requestPipelineEnd(logger, trackId, duration.TotalMilliseconds, response.StatusCode, new HttpHeadersLogValue(HttpHeadersLogValue.Kind.Response, response.Headers, response.Content?.Headers).ToString(), responseBody, null);

                logger.Info($"HTTP Request ({trackId}) Responsed, after {duration.TotalMilliseconds}ms -{response.StatusCode}");
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