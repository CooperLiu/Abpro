using Castle.Windsor;

namespace Abp.WebApi.Client.DependencyInjection
{
    internal static class IocManagerExtensions
    {
        public static bool IsRegister<TComponent>(this IWindsorContainer container)
        {
            return container.Kernel.HasComponent(typeof(TComponent));
        }
    }
}