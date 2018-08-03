using System.Reflection;
using Abp.Modules;
using Abpro.MessageBus.Consumer;
using Abpro.MessageBus.Dependency;
using Rebus.Bus;

namespace Abpro.MessageBus
{
    public class RebusRebbitmqConsumerModule : AbpModule
    {
        // private static string RabbitMqServerConnectString = ConfigurationManager.ConnectionStrings["RabbitMqServer"].ConnectionString;

        private IBus _bus;

        public override void PreInitialize()
        {
            IocManager.IocContainer.Install(new RebusConsumerIocInstaller());

            //Configuration
            //    .Modules
            //    .UseRebusConsumer()
            //    .SetMaxParallelism(1)
            //    .SetNumberOfWorkers(1)
            //    .UseLogging(c => c.NLog())
            //    .ConnectTo(RabbitMqServerConnectString)
            //    .UseQueue(Assembly.GetExecutingAssembly().GetName().Name)
            //    .RegisterHandlerInAssemblies(Assembly.GetExecutingAssembly(), typeof(RebusRebbitmqConsumerModule).Assembly);


        }

        public override void Initialize()
        {

        }

        public override void PostInitialize()
        {
            _bus = IocManager.Resolve<IRebusConsumerBootstrapper>().Start();
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