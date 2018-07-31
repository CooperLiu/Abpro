using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Castle.Facilities.Logging;
using Castle.Windsor;
using Shouldly;
using Xunit;

namespace Abpro.WebApiClient.Tests
{
    public class HttpClientFactory_Test
    {
        private readonly IWindsorContainer _ioContainer;

        public HttpClientFactory_Test()
        {
            _ioContainer = new WindsorContainer();
            _ioContainer.AddFacility<LoggingFacility>(f => f.UseNLog("nlog.xml"));
        }

        [Fact]
        public void HttpClientFactoryDependency_Test()
        {
            IHttpClientFactoryDependency dependency = new HttpClientFactoryDependency(_ioContainer);

            dependency.AddHttpClient("pay2jk724.com",
                h =>
                {
                    h.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("JK724HttpClient", "v1.0.0"));
                }).ConfigurePrimaryHttpMessageHandler(CreateHttpMessageHandler);

            var factory = _ioContainer.Resolve<IHttpClientFactory>();

            var client = factory.CreateClient("pay2jk724.com");

            client.ShouldNotBeNull();

            var count = client.DefaultRequestHeaders.UserAgent.Count;

            count.ShouldBeGreaterThan(0);


        }

        private HttpMessageHandler CreateHttpMessageHandler()
        {
            return new HttpClientHandler() { UseCookies = false };
        }


        [Fact]
        public void HttpClient_MultipleCalls_Test()
        {
            IHttpClientFactoryDependency dependency = new HttpClientFactoryDependency(_ioContainer);

            dependency.AddHttpClient();

            var factory = _ioContainer.Resolve<IHttpClientFactory>();

            var client1 = factory.CreateClient("test1");

            client1.ShouldNotBeNull();

            var client2 = factory.CreateClient("test2");


            client2.ShouldNotBeNull();
        }


    }
}
