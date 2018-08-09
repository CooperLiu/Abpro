using System.Configuration;
using Abp.TestBase;
using Castle.Facilities.Logging;

namespace Abpro.Integration.Tests
{
    public class TestBase : AbpIntegratedTestBase<AbproIntegratedTestModule>
    {
        public TestBase()
        {
            //使用Nlog 日志
            AbpBootstrapper.IocManager
                .IocContainer.AddFacility<LoggingFacility>(f =>
                    f.UseNLog()
                        .WithConfig("NLog.config"));
        }
    }
}