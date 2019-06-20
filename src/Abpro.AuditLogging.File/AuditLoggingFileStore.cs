using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Dependency;
using Castle.Core.Logging;

namespace Abpro.AuditLogging.File
{
    public class AuditLoggingFileStore : IAuditingStore, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public AuditLoggingFileStore()
        {
            Logger = NullLogger.Instance;
        }

        public async Task SaveAsync(AuditInfo auditInfo)
        {
            var m = new AuditInfoMqMessage();
            m.TenantId = auditInfo.TenantId;
            m.UserId = auditInfo.UserId;
            m.ImpersonatorUserId = auditInfo.ImpersonatorUserId;
            m.ImpersonatorTenantId = auditInfo.ImpersonatorTenantId;
            m.ServiceName = auditInfo.ServiceName;
            m.MethodName = auditInfo.MethodName;
            m.Parameters = auditInfo.Parameters;
            m.ExecutionTime = auditInfo.ExecutionTime;
            m.ExecutionDuration = auditInfo.ExecutionDuration;
            m.ClientIpAddress = auditInfo.ClientIpAddress;
            m.ClientName = auditInfo.ClientName;
            m.BrowserInfo = auditInfo.BrowserInfo;
            m.CustomData = auditInfo.CustomData;


            if (auditInfo.Exception != null)
            {
                m.CustomData += $" {auditInfo.Exception.Message}";
                m.Exception = auditInfo.Exception.StackTrace;
                if (auditInfo.Exception.InnerException != null)
                {
                    m.CustomData += $" {auditInfo.Exception.InnerException.Message}";
                    m.Exception = auditInfo.Exception.InnerException.StackTrace;
                }
            }

            Logger.Info(m.ToString());
        }
    }
}