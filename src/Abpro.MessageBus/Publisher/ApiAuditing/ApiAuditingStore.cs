using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Dependency;

namespace Abpro.MessageBus.Publisher.ApiAuditing
{
    public class ApiAuditingStore : IAuditingStore
    {
        public Task SaveAsync(AuditInfo auditInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}