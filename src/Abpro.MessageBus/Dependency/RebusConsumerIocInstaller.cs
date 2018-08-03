using Abpro.MessageBus.Consumer;
using Abpro.MessageBus.Idempotents;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Abpro.MessageBus.Dependency
{
    /// <summary>
    /// Rebus消费端Ioc注册
    /// </summary>
    public class RebusConsumerIocInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IRebusConsumerConfig, RebusConsumerConfig>().ImplementedBy<RebusConsumerConfig>().LifestyleSingleton(),
                Component.For<IRebusConsumerBootstrapper, RebusConsumerBootstrapper>().ImplementedBy<RebusConsumerBootstrapper>().LifestyleSingleton(),
                Component.For<IIdempotentKeyStore, CacheIdempotentKeyStore>().ImplementedBy<CacheIdempotentKeyStore>().LifestyleSingleton(),
                Component.For<IIdempotentCoordinator, IdempotentCoordinator>().ImplementedBy<IdempotentCoordinator>().LifestyleSingleton()
                );
        }
    }
}