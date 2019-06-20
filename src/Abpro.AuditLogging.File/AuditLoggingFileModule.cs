using System;
using Abp;
using Abp.Modules;

namespace Abpro.AuditLogging.File
{
    [DependsOn(typeof(AbpKernelModule))]
    public class AuditLoggingFileModule : AbpModule
    {
    }
}
