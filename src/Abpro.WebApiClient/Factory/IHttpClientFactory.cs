using System.Net.Http;

namespace Abpro.WebApiClient.Factory
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(string name);
    }


}