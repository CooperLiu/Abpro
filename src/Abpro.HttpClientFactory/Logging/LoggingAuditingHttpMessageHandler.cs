using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Abpro.HttpClientFactory.Auditing;
using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;

namespace Abpro.HttpClientFactory.Logging
{
    public class LoggingAuditingHttpMessageHandler : LoggingHttpMessageHandler
    {
        private ILogger _logger;

        public LoggingAuditingHttpMessageHandler(ILogger logger)
            : base(logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var stopwatch = ValueStopwatch.StartNew();

            request.Headers.TryGetValues(WebApiClientConsts.HttpTrackIdHeadName, out var trackIds);
            var ids = trackIds?.ToArray();
            var trackId = ids?.Any() ?? false ? ids.First() : Guid.NewGuid().ToString("N");

            using (Log.BeginRequestPipelineScope(_logger, trackId, request))
            {
                var requestBody = request.Content != null ? await request.Content.ReadAsStringAsync() : null;
                Log.RequestPipelineStart(_logger, trackId, request, requestBody);
                var response = await base.SendAsync(request, cancellationToken);
                Log.RequestPipelineEnd(_logger, trackId, response, await response.Content.ReadAsStringAsync(), stopwatch.GetElapsedTime());

                return response;
            }
        }

        private static class Log
        {

            private static readonly Func<ILogger, string, HttpMethod, Uri, IDisposable> _beginRequestPipelineScope =
                LoggerMessage.DefineScope<string, HttpMethod, Uri>("HTTP Request ({trackId}) {HttpMethod} {Uri}");

            private static readonly Action<ILogger, string, HttpMethod, Uri, string, string, Exception> _requestPipelineStart =

                (logger, trackId, httpMethod, uri, requestHeader, requestBody, e) =>
                {
                    logger.LogDebug($"Start processing HTTP request ({trackId}) {httpMethod} {uri} \r\n {requestHeader}  Request Body:\r\n {requestBody}", e);
                };

            private static readonly Action<ILogger, string, double, HttpStatusCode, string, string, Exception> _requestPipelineEnd =


                (logger, trackId, elapsedMilliseconds, statusCode, responseHeader, responseBody, e) =>
                { logger.LogDebug($"End processing HTTP request ({trackId}) after {elapsedMilliseconds}ms - {statusCode} \r\n {responseHeader} Response Body:\r\n {responseBody}", e); };


            public static IDisposable BeginRequestPipelineScope(ILogger logger, string trackId, HttpRequestMessage request)
            {
                return _beginRequestPipelineScope(logger, trackId, request.Method, request.RequestUri);
            }

            public static void RequestPipelineStart(ILogger logger, string trackId, HttpRequestMessage request, string requestBody)
            {
                _requestPipelineStart(logger,
                    trackId,
                    request.Method,
                    request.RequestUri,
                    new HttpHeadersLogValue(HttpHeadersLogValue.Kind.Request, request.Headers, request?.Content?.Headers).ToString(),
                    requestBody,
                    null);
            }

            public static void RequestPipelineEnd(ILogger logger, string trackId, HttpResponseMessage response, string responseBody, TimeSpan duration)
            {
                _requestPipelineEnd(logger, trackId, duration.TotalMilliseconds, response.StatusCode, new HttpHeadersLogValue(HttpHeadersLogValue.Kind.Response, response.Headers, response.Content?.Headers).ToString(), responseBody, null);

                logger.LogInformation($"HTTP Request ({trackId}) Responsed, after {duration.TotalMilliseconds}ms -{response.StatusCode}");
            }
        }


    }
}
