using System.Threading.Tasks;

namespace Abpro.HttpClientFactory.Auditing
{
    public class NullHttpCallingAuditingStore : IHttpCallingAuditingStore
    {
        public Task SaveAsync(HttpCallingAuditingInfo info)
        {
            return Task.CompletedTask;
        }
    }
}