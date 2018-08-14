using System;
using Abp.Auditing;
using Abp.Modules;
using Abpro.MessageBus.Publisher;
using Abpro.MessageBus.Publisher.ApiAuditing;

namespace Abpro.MessageBus.ApiAuditing
{
    [DependsOn(typeof(RebusRebbitmqPublisherModule))]
    public class MessageBusApiAuditingModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.IocManager.Register<IAuditingStore, ApiAuditingMessagePublisher>();
        }
    }
}
