using Abp.Modules;
using Abpro.WebApiClient;
using Abpro.WebApiClient.Factory;

namespace WebApiClientFactoryDemo
{
    [DependsOn(
        typeof(WebApiClientModule)
        )]
    public class WebApiClientFactoryModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration
                .Modules
                .AddHttpClientFactory()
                .EnableHttpCallingAuditing();
        }
    }
}