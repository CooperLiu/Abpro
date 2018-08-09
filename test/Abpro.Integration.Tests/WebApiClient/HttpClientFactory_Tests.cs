using System;
using Abpro.WebApiClient.Factory;
using Shouldly;
using Xunit;
// ReSharper disable InconsistentNaming

namespace Abpro.Integration.Tests.WebApiClient
{
    public class HttpClientFactory_Tests : TestBase
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly IHttpClientFactoryOptions _options;

        public HttpClientFactory_Tests()
        {
            _clientFactory = Resolve<IHttpClientFactory>();
            _options = Resolve<IHttpClientFactoryOptions>();
        }

        [Fact]
        public void HttpClient_Should_CreateInstance()
        {
            var client = _clientFactory.CreateClient("724.com");

            client.ShouldNotBeNull();
        }

        [Fact]
        public void HttpClient_Should_ShareConfig()
        {
            var client1 = _clientFactory.CreateClient("724.com");
            var client2 = _clientFactory.CreateClient("725.com");

            client1.BaseAddress.ShouldBe(new Uri("http://localhost"));
            client2.BaseAddress.ShouldBe(new Uri("http://localhost"));
        }

        [Fact]
        public void Factory_MultipleCalls_DoesNotCacheHttpClient()
        {
            var count = 0;

            _options.AddHttpClientAction(c => { count++; });

            var client1 = _clientFactory.CreateClient("724.com");
            var client2 = _clientFactory.CreateClient("725.com");

            count.ShouldBe(2);
        }

        [Fact]
        public void Factory_MultipleCalls_CachesHandler()
        {
            var count = 0;

            _options.HttpMessageHandlerBuilderActions.Add(c => { count++; });

            var client1 = _clientFactory.CreateClient();
            var client2 = _clientFactory.CreateClient();

            count.ShouldBe(1);
        }
    }
}