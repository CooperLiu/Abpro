using System.Configuration;
using System.Reflection;
using Abp.Modules;
using Abp.Runtime.Caching.Redis;
using Abpro.MessageBus;
using Abpro.MessageBus.Consumer.Setup;
using Rebus.NLog;

namespace MessageBusConsumer
{
    [DependsOn(
        typeof(AbpRedisCacheModule),
        typeof(RebusRebbitmqConsumerModule)
    )]
    public class MessageBusConsumerModule : AbpModule
    {
        private static string RabbitMqServerConnectString = ConfigurationManager.ConnectionStrings["RabbitMqServer"].ConnectionString;

        public override void PreInitialize()
        {
            Configuration.Caching.UseRedis();

            Configuration
                .Modules
                .UseRebusConsumer()
                .SetMaxParallelism(1)
                .SetNumberOfWorkers(1)
                .UseLogging(c => c.NLog())
                .ConnectTo(RabbitMqServerConnectString)
                .UseQueue(Assembly.GetExecutingAssembly().GetName().Name)
                .RegisterHandlerInAssemblies(Assembly.GetExecutingAssembly(), typeof(MessageBusConsumerModule).Assembly);


        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

    }
}