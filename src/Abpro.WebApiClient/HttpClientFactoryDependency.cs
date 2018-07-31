using System;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;

namespace Abpro.WebApiClient
{
    public interface IHttpClientFactoryDependency
    {
        IWindsorContainer IocContainer { get; set; }

        //void Install(IWindsorContainer iocContainer);

        void Register<TType>();

        void Register<TType, TImpl>()
            where TType : class
            where TImpl : class, TType;
    }

    public class HttpClientFactoryDependency : IHttpClientFactoryDependency
    {
        public IWindsorContainer IocContainer { get; set; }

        public HttpClientFactoryDependency(IWindsorContainer iocContainer)
        {
            IocContainer = iocContainer ?? throw new ArgumentNullException(nameof(iocContainer));

            RegisterBaseService();
        }

        public void Register<TType>()
        {
            IocContainer.Kernel.Register(
                Component.For(typeof(TType)).LifestyleTransient()
            );
        }

        public void Register<TType, TImpl>() where TType : class
            where TImpl : class, TType
        {
            IocContainer.Kernel.Register(
                Component.For<TType, TImpl>().LifestyleTransient()
                );
        }

        private void RegisterBaseService()
        {
            IocContainer.Kernel.Resolver.AddSubResolver(new CollectionResolver(IocContainer.Kernel, false));

            IocContainer.Register(
                Component
                    .For<IHttpClientFactoryDependency, HttpClientFactoryDependency>()
                    .LifestyleSingleton()
                    .UsingFactoryMethod<IHttpClientFactoryDependency>(f => this)
                );
        }
    }



}
