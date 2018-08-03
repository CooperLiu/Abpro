using Abp.Configuration.Startup;

namespace Abpro.MessageBus.Publisher.Setup
{
    public static class RebusPublisherConfigurationExtensions
    {
        public static IRebusEventDataPublisherConfig UseRebusPublisher(this IModuleConfigurations configuration)
        {
            return configuration.AbpConfiguration.GetOrCreate("Modules.Abp.RebusRabbitMqPublisher", () => configuration.AbpConfiguration.IocManager.Resolve<IRebusEventDataPublisherConfig>());
        }
    }
}