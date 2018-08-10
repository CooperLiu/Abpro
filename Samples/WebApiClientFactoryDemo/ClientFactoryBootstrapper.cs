using System;
using System.Net.Http;
using System.Text;
using Abp.TestBase;
using Abp.Threading;
using Abpro.WebApiClient.Factory;
using Castle.Facilities.Logging;

namespace WebApiClientFactoryDemo
{
    public class ClientFactoryBootstrapper : AbpIntegratedTestBase<WebApiClientFactoryModule>
    {
        public ClientFactoryBootstrapper()
        {
            //使用Nlog 日志
            AbpBootstrapper.IocManager
                .IocContainer.AddFacility<LoggingFacility>(f =>
                    f.UseNLog()
                        .WithConfig("NLog.config"));
        }
    }

    public class TestClientFactory : ClientFactoryBootstrapper
    {
        private readonly IHttpClientFactory _clientFactory;

        public TestClientFactory():base()
        {
            _clientFactory = Resolve<IHttpClientFactory>();
        }

        public void TestHttpClientFactoryCreateInstance()
        {
            var client = _clientFactory.CreateClient();

            using (var msg = new HttpRequestMessage(HttpMethod.Post, "https://pay2uat.jk724.com/api/Payment/GetPayChannelList"))
            {
                msg.Content = new StringContent("{'appId':'2017060616445783','deviceSource':1}", Encoding.UTF8, "application/json");

                var res = AsyncHelper.RunSync(() => client.SendAsync(msg));

                var result = AsyncHelper.RunSync(()=> res.Content.ReadAsStringAsync()) ;


                Console.WriteLine("OK");
            }

        }
    }
}