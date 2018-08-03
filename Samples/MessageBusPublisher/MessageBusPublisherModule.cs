using System.Configuration;
using System.Reflection;
using Abp.Modules;
using Abp.Runtime.Caching.Redis;
using Abpro.MessageBus;
using Abpro.MessageBus.Publisher.Setup;
using Rebus.NLog;


namespace MessageBusPublisher
{
    [DependsOn(
        typeof(AbpRedisCacheModule),
        typeof(RebusRebbitmqPublisherModule)
    )]
    public class MessageBusPublisherModule : AbpModule
    {
        private static string RabbitMqServerConnectString = ConfigurationManager.ConnectionStrings["RabbitMqServer"].ConnectionString;


        public override void PreInitialize()
        {
            Configuration.Caching.UseRedis();

            Configuration
                .Modules
                .UseRebusPublisher()
                .ConnectionTo(RabbitMqServerConnectString)
                .UseLogging(c => c.NLog());


        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

    }
}