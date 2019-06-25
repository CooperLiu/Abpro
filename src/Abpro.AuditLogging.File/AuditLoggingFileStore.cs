using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Dependency;
using Serilog;

namespace Abpro.AuditLogging.File
{
    public class AuditLoggingFileStore : IAuditingStore, ITransientDependency
    {
        private static readonly string LogPath = ConfigurationManager.AppSettings["AuditLogging.Path"] ?? "c:\\logs";
        private static readonly string LogName = ConfigurationManager.AppSettings["AuditLogging.Name"] ?? "auditlog.log";

        private static readonly bool LogEnable = ConfigurationManager.AppSettings["AuditLogging.Enable"]
                                                  ?.Equals("true", StringComparison.OrdinalIgnoreCase) ?? true;

        private static readonly bool Enable = (ConfigurationManager.AppSettings["AuditLogging.Path"] != null
                                               && ConfigurationManager.AppSettings["AuditLogging.Name"] != null
                                               && LogEnable);

        static AuditLoggingFileStore()
        {
            if (!Enable)
            {
                return;
            }

            var path = LogPath ?? "c:\\logs";
            var name = LogName ?? "auditlog.log";

            var logPath = Path.Combine(path, name);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Hour, outputTemplate: "{Message:lj}{NewLine}")
                .CreateLogger();
        }

        public AuditLoggingFileStore()
        {
        }

        public async Task SaveAsync(AuditInfo auditInfo)
        {
            if (Enable)
            {
                return;
            }

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

            Log.Debug("{@m}", m);

            await Task.CompletedTask;
        }
    }
}