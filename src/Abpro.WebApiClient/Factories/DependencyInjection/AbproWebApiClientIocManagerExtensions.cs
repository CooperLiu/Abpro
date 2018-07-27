using System;
using Castle.MicroKernel.Registration;

namespace Abp.WebApi.Client.DependencyInjection
{
    public static class AbproWebApiClientIocManagerExtensions
    {
        public static void AddHttpClient(this IAbproWebApiClientIocManager iocManager)
        {
            if (iocManager == null) throw new ArgumentNullException(nameof(iocManager));


            iocManager.IocContainer.Register(
                Component.For<HttpMessageHandlerBuilder, DefaultHttpMessageHandlerBuilder>().LifestyleTransient(),
                Component.For<IHttpClientFactory, DefaultHttpClientFactory>().LifestyleSingleton()
                );



            if (!iocManager.IocContainer.IsRegister<IHttpMessageHandlerBuilderFilter>())
            {
                iocManager.IocContainer.Register(
                    Component.For<IHttpMessageHandlerBuilderFilter, LoggingHttpMessageHandlerBuilderFilter>().LifestyleSingleton()
                );
            }
        }

        public static IHttpClientBuilder AddHttpClient(this IAbproWebApiClientIocManager iocManager, string name)
        {
            if (iocManager == null) throw new ArgumentNullException(nameof(iocManager));
            if (name == null) throw new ArgumentNullException(nameof(name));

            AddHttpClient(iocManager);

            return new DefaultHttpClientBuilder(iocManager, name);
        }
    }
}