using Abp.AutoMapper;
using Abp.Modules;
using Abpro.MessageBus.Publisher.Dependency;
using Rebus.Bus;

namespace Abpro.MessageBus.Publisher
{
    [DependsOn(
        typeof(AbpAutoMapperModule)
    )]
    public class RebusRebbitmqPublisherModule : AbpModule
    {
        private IBus _bus;

        public override void PreInitialize()
        {
            IocManager.IocContainer.Install(new RebusPublisherIocInstaller());
        }

        public override void Initialize()
        {
            //IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            _bus = IocManager.Resolve<IRebusEventDataPublisherBootstrapper>().Start();
        }

        public override void Shutdown()
        {
            if (_bus != null)
            {
                _bus.Dispose();
            }
        }
    }
}