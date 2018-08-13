using Abp.Modules;
using Abpro.WebApiClient.Factory;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace Abpro.WebApiClient
{
    public class WebApiClientModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.IocContainer.Install(new HttpClientFactoryDependencyInstaller());
        }


        public override void Shutdown()
        {
            IocManager.IocContainer.Kernel.Resolver.RemoveSubResolver(new CollectionResolver(IocManager.IocContainer.Kernel, false));
        }
    }
}