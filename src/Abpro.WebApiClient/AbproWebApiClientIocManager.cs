using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Abpro.WebApiClient
{
    public interface IAbproWebApiClientIocManager
    {
        IWindsorContainer IocContainer { get; set; }

        void Startup(IWindsorContainer container);
    }

    public class AbproWebApiClientIocManager: IAbproWebApiClientIocManager
    {
        public IWindsorContainer IocContainer { get; set; }

        public void Startup(IWindsorContainer container)
        {
            IocContainer = container;
            //注入

            IocContainer.Register(
                Component.For<IAbproWebApiClientIocManager, AbproWebApiClientIocManager>().LifestyleSingleton()
                );

        }
    }
}
