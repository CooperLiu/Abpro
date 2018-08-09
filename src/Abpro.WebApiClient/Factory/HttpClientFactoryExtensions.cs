using System;
using System.Net.Http;

namespace Abpro.WebApiClient.Factory
{
    public static class HttpClientFactoryExtensions
    {
        public static string DefaultName = string.Empty;

        /// <summary>
        /// Creates a new <see cref="HttpClient"/> using the default configuration.
        /// </summary>
        /// <param name="factory">The <see cref="IHttpClientFactory"/>.</param>
        /// <returns>An <see cref="HttpClient"/> configured using the default configuration.</returns>
        public static HttpClient CreateClient(this IHttpClientFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            return factory.CreateClient(DefaultName);
        }
    }
}