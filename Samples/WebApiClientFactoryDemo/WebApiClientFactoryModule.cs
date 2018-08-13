using Abp.Modules;
using Abpro.WebApiClient;
using Abpro.WebApiClient.Factory;
using Abpro.WebApiClient.Factory.Logging;

namespace WebApiClientFactoryDemo
{
    [DependsOn(
        typeof(WebApiClientModule)
        )]
    public class WebApiClientFactoryModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IHttpMessageHandlerBuilderFilter, LoggingHttpMessageHandlerBuilderFilter>();

            Configuration
                .Modules
                .AddHttpClientFactory()
                .EnableHttpCallingAuditing();
        }
    }
}