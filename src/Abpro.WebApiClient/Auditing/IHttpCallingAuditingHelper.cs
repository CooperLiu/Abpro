using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Abpro.WebApiClient.Auditing
{
    public interface IHttpCallingAuditingHelper
    {
        bool IsEnableHttpCallingAuditing();

        Task<HttpCallingAuditingInfo> CreateAuditingInfo(string trackId, HttpRequestMessage request, double executionDuration, HttpStatusCode statusCode, HttpResponseMessage response);


        Task SaveAsync(HttpCallingAuditingInfo info);

    }
}