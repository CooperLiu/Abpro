using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Abpro.MessageBus.Publisher.Dependency
{
    /// <summary>
    /// Rebus消息总线Ioc注册
    /// </summary>
    public class RebusPublisherIocInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IMessageBus, RebusRabbitMqMessageBus>().ImplementedBy<RebusRabbitMqMessageBus>().LifestyleSingleton(),
                Component.For<IRebusEventDataPublisherConfig, RebusEventDataPublisherConfig>().ImplementedBy<RebusEventDataPublisherConfig>().LifestyleSingleton(),
                Component.For<IRebusEventDataPublisherBootstrapper, RebusEventDataPublisherBootstrapper>().ImplementedBy<RebusEventDataPublisherBootstrapper>().LifestyleSingleton()
            );
        }
    }
}