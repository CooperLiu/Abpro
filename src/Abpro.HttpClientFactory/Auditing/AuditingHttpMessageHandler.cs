using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Abpro.HttpClientFactory.Auditing
{
    public class AuditingHttpMessageHandler : DelegatingHandler
    {
        private readonly IHttpCallingAuditingHelper _auditingHelper;

        public AuditingHttpMessageHandler(IHttpCallingAuditingHelper auditingHelper)
        {
            _auditingHelper = auditingHelper;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var stopwatch = ValueStopwatch.StartNew();
            var trackId = Guid.NewGuid().ToString("N");
            request.Headers.TryAddWithoutValidation(WebApiClientConsts.HttpTrackIdHeadName, trackId);

            var response = await base.SendAsync(request, cancellationToken);

            response.Headers.TryAddWithoutValidation(WebApiClientConsts.HttpTrackIdHeadName, trackId);

            var auditingInfo = await _auditingHelper.CreateAuditingInfo(trackId, request, stopwatch.GetElapsedTime().TotalMilliseconds, response.StatusCode, response);

            await _auditingHelper.SaveAsync(auditingInfo);

            return response;
        }
    }
}