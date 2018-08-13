using System.Threading.Tasks;

namespace Abpro.WebApiClient.Auditing
{
    public interface IHttpCallingAuditingStore
    {
        Task SaveAsync(HttpCallingAuditingInfo info);
    }
}