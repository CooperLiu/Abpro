using System;
using Abp;
using Abp.Modules;
using Abp.TestBase;
using Abpro.WebApiClient;
using Abpro.WebApiClient.Factory;

namespace Abpro.Integration.Tests
{
    [DependsOn(
        typeof(AbpTestBaseModule),
        typeof(WebApiClientModule)
        )]
    public class AbproIntegratedTestModule : AbpModule
    {
        public override void PreInitialize()
        {

            Configuration
                .Modules
                .AddHttpClientFactory()
                .AddHttpClientAction(c => { c.BaseAddress= new Uri("http://localhost"); });

        }
    }
}
