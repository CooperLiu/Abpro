using Abp.Configuration.Startup;

namespace Abpro.WebApiClient.Factory
{
    public static class HttpClientFactoryConfigurationExtensions
    {
        public static IHttpClientFactoryOptions AddHttpClientFactory(this IModuleConfigurations configuration)
        {
            return configuration.AbpConfiguration.GetOrCreate("Modules.Abp.HttpClientFactory",
                () => configuration.AbpConfiguration.IocManager.Resolve<IHttpClientFactoryOptions>());
        }
    }
}