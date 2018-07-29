using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Abpro.WebApiClient
{
    public interface IHttpClientFactoryDependency
    {
        IWindsorContainer IocContainer { get; set; }

        void Install(IWindsorContainer iocContainer);
    }

    public class HttpClientFactoryDependency : IHttpClientFactoryDependency
    {
        public IWindsorContainer IocContainer { get; set; }

        public void Install(IWindsorContainer iocContainer)
        {
            IocContainer = iocContainer ?? throw new ArgumentNullException(nameof(iocContainer));

            RegisterEntrySelf();
        }

        private void RegisterEntrySelf()
        {
            IocContainer.Register(
                Component.For<IHttpClientFactoryDependency, HttpClientFactoryDependency>().LifestyleSingleton(),

                Component.For<HttpMessageHandlerBuilder, DefaultHttpMessageHandlerBuilder>().LifestyleTransient(),

                Component.For<IHttpClientFactory, DefaultHttpClientFactory>().LifestyleSingleton()

                );
        }
    }


    internal static class IHttpClientFactoryDependencyExtensions
    {
        public static bool IsRegistered<TType>(this IHttpClientFactoryDependency dependency)
        {
            return dependency.IocContainer.Kernel.HasComponent(typeof(TType));
        }
    }
}
