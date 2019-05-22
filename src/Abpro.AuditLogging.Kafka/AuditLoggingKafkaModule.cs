using System;
using Abp;
using Abp.Auditing;
using Abp.Modules;

namespace Abpro.AuditLogging.Kafka
{
    [DependsOn(typeof(AbpKernelModule))]
    public class AuditLoggingKafkaModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.IocManager.Register<IAuditLoggingKafkaConfig, AuditLoggingKafkaConfig>();
            Configuration.IocManager.Register<IAuditingStore, AuditLoggingKafkaProducer>();
        }
    }
}
