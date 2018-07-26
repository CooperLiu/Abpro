using Abpro.WebApiClient;
using Castle.MicroKernel.Registration;

namespace Abp.WebApi.Client.DependencyInjection
{
    public static class AbproWebApiClientIocManagerExtensions
    {
        public static void AddHttpClient(this IAbproWebApiClientIocManager iocManager)
        {
            iocManager.IocContainer.Register(
                Component.For<HttpMessageHandlerBuilder, DefaultHttpMessageHandlerBuilder>().LifestyleTransient(),
                Component.For<IHttpClientFactory, DefaultHttpClientFactory>().LifestyleSingleton(),
                Component.For<HttpMessageHandlerBuilder, DefaultHttpMessageHandlerBuilder>().LifestyleTransient()
                );

            if (!iocManager.IocContainer.Kernel.HasComponent(typeof(IHttpMessageHandlerBuilderFilter)))
            {
                iocManager.IocContainer.Register(
                    Component.For<IHttpMessageHandlerBuilderFilter, LoggingHttpMessageHandlerBuilderFilter>().LifestyleSingleton()
                );
            }
        }
    }
}