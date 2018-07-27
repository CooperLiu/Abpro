using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Abp.WebApi.Client.DependencyInjection
{
    public interface IAbproWebApiClientIocManager
    {
        IWindsorContainer IocContainer { get; set; }

        void InjectIocContainer(IWindsorContainer container);
    }

    public class AbproWebApiClientIocManager: IAbproWebApiClientIocManager
    {
        public IWindsorContainer IocContainer { get; set; }

        public void InjectIocContainer(IWindsorContainer container)
        {
            IocContainer = container;
            //注入

            IocContainer.Register(
                Component.For<IAbproWebApiClientIocManager, AbproWebApiClientIocManager>().LifestyleSingleton()
                );

        }
    }
}
