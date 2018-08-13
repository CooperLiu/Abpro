using Abpro.WebApiClient.Auditing;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Abpro.WebApiClient.Factory
{
    public class HttpClientFactoryDependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel, false));

            container.Register(

                Component.For<IHttpClientFactoryOptions, HttpClientFactoryOptions>().ImplementedBy<HttpClientFactoryOptions>(),

                Component.For<IHttpMessageHandlerBuilder, HttpMessageHandlerBuilder>().ImplementedBy<DefaultHttpMessageHandlerBuilder>(),

                Component.For<IHttpClientFactory, DefaultHttpClientFactory>().ImplementedBy<DefaultHttpClientFactory>(),

                Component.For<IHttpCallingAuditingHelper, HttpCallingAuditingHelper>().ImplementedBy<HttpCallingAuditingHelper>().LifestyleTransient(),

                Component.For<IHttpCallingAuditingStore, NullHttpCallingAuditingStore>().ImplementedBy<NullHttpCallingAuditingStore>().LifestyleTransient(),

                Component.For<IHttpMessageHandlerBuilderFilter, LoggingHttpMessageHandlerBuilderFilter>().ImplementedBy<LoggingHttpMessageHandlerBuilderFilter>()
                );
        }
    }
}