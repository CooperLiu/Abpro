using Abp.Modules;
using Abpro.WebApiClient.Factory;

namespace Abpro.WebApiClient
{
    public class WebApiClientModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.IocContainer.Install(new HttpClientFactoryDependencyInstaller());
        }

    }
}