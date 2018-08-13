using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Abpro.WebApiClient.Factory;
using Abpro.WebApiClient.Factory.Logging;

namespace Abpro.WebApiClient.Auditing
{
    public class HttpCallingAuditingHelper : IHttpCallingAuditingHelper
    {
        private readonly IHttpCallingAuditingStore _auditingStore;
        private readonly IHttpClientFactoryOptions _options;

        public HttpCallingAuditingHelper(IHttpCallingAuditingStore auditingStore, IHttpClientFactoryOptions options)
        {
            _auditingStore = auditingStore;
            _options = options;
        }

        public bool IsEnableHttpCallingAuditing()
        {
            return _options.IsEnableHttpCallingAuditing;
        }

        public async Task<HttpCallingAuditingInfo> CreateAuditingInfo(string trackId, HttpRequestMessage request, double executionDuration,
            HttpStatusCode statusCode, HttpResponseMessage response)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (trackId == null) trackId = Guid.NewGuid().ToString("N");

            var auditingInfo = new HttpCallingAuditingInfo();
            auditingInfo.RemoteApiUrl = request.RequestUri.ToString();
            auditingInfo.TrackId = trackId;
            auditingInfo.HttpMethod = request.Method.Method;
            auditingInfo.RequestHeaders = new HttpHeadersLogValue(HttpHeadersLogValue.Kind.Request, request.Headers, request.Content?.Headers).ToString();
            auditingInfo.RequestBody = await request.Content?.ReadAsStringAsync();
            auditingInfo.ExecutionDuration = executionDuration;

            auditingInfo.HttpStatusCode = (int)response.StatusCode;
            auditingInfo.ResponseHeaders = new HttpHeadersLogValue(HttpHeadersLogValue.Kind.Response, response.Headers, response.Content?.Headers).ToString();
            auditingInfo.ResponseBody = await response.Content?.ReadAsStringAsync();

            return auditingInfo;

        }

        public Task SaveAsync(HttpCallingAuditingInfo info)
        {
            if (info == null) throw new ArgumentNullException(nameof(info));
            return _auditingStore.SaveAsync(info);
        }
    }
}