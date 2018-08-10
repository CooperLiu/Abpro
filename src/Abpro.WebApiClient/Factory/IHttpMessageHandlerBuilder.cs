using System.Collections.Generic;
using System.Net.Http;

namespace Abpro.WebApiClient.Factory
{
    public interface IHttpMessageHandlerBuilder
    {
        string Name { get; set; }
        HttpMessageHandler PrimaryHandler { get; set; }
        IList<DelegatingHandler> AdditionalHandlers { get; set; }
        HttpMessageHandler Build();
    }
}