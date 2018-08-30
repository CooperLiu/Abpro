using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Abpro.HttpClientFactory.Auditing
{
    public interface IHttpCallingAuditingHelper
    {

        Task<HttpCallingAuditingInfo> CreateAuditingInfo(string trackId, HttpRequestMessage request, double executionDuration, HttpStatusCode statusCode, HttpResponseMessage response);


        Task SaveAsync(HttpCallingAuditingInfo info);

    }
}