using Abp.Configuration.Startup;

namespace Abpro.MessageBus.Consumer.Setup
{
    public static class RebusConsumerConfigurationExtensions
    {
        /// <summary>
        /// 使用Rebus消费端
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IRebusConsumerConfig UseRebusConsumer(this IModuleConfigurations configuration)
        {
            return configuration.AbpConfiguration.GetOrCreate("Modules.Abp.RebusRabbitMqConsumer", () => configuration.AbpConfiguration.IocManager.Resolve<IRebusConsumerConfig>());
        }
    }
}