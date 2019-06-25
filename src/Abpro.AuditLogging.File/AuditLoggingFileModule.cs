using System;
using Abp;
using Abp.Auditing;
using Abp.Modules;

namespace Abpro.AuditLogging.File
{
    [DependsOn(typeof(AbpKernelModule))]
    public class AuditLoggingFileModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.IocManager.Register<IAuditingStore, AuditLoggingFileStore>();
        }
    }
}
