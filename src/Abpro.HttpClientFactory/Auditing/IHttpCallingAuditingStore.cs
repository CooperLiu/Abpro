using System.Threading.Tasks;

namespace Abpro.HttpClientFactory.Auditing
{
    public interface IHttpCallingAuditingStore
    {
        Task SaveAsync(HttpCallingAuditingInfo info);
    }
}