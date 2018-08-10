using Abp.Modules;
using Abpro.WebApiClient;

namespace WebApiClientFactoryDemo
{
    [DependsOn(
        typeof(WebApiClientModule)
        )]
    public class WebApiClientFactoryModule : AbpModule
    {

    }
}