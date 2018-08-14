using System.Threading.Tasks;
using Abp.Auditing;

namespace Abpro.MessageBus.Publisher.ApiAuditing
{
    public class ApiAuditingMessagePublisher : IAuditingStore
    {
        private readonly IMessageBus _bus;

        public ApiAuditingMessagePublisher(IMessageBus bus)
        {
            _bus = bus;
        }

        public async Task SaveAsync(AuditInfo auditInfo)
        {
            var mqMsgAuditInfo = new AuditInfoMqMessage
            {
                BrowserInfo = auditInfo.BrowserInfo,
                ClientIpAddress = auditInfo.ClientIpAddress,
                ClientName = auditInfo.ClientName,
                CustomData = auditInfo.CustomData,
                ExecutionDuration = auditInfo.ExecutionDuration,
                ImpersonatorTenantId = auditInfo.ImpersonatorTenantId,
                ImpersonatorUserId = auditInfo.ImpersonatorUserId,
                MethodName = auditInfo.MethodName,
                Parameters = auditInfo.Parameters,
                ServiceName = auditInfo.ServiceName,
                UserId = auditInfo.UserId,
                ExecutionTime = auditInfo.ExecutionTime,
                TenantId = auditInfo.TenantId
            };

            if (auditInfo.Exception != null)
            {
                mqMsgAuditInfo.CustomData += $" {auditInfo.Exception.Message}";
                mqMsgAuditInfo.Exception = auditInfo.Exception.StackTrace;
                if (auditInfo.Exception.InnerException != null)
                {
                    mqMsgAuditInfo.CustomData += $" {auditInfo.Exception.InnerException.Message}";
                    mqMsgAuditInfo.Exception = auditInfo.Exception.InnerException.StackTrace;
                }
            }

            await _bus.PublishAsync(mqMsgAuditInfo);
        }
    }
}