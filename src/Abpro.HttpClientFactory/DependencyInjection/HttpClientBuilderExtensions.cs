using System;
using Abpro.HttpClientFactory.Auditing;
using Abpro.HttpClientFactory.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;

namespace Abpro.HttpClientFactory.DependencyInjection
{
    public static class HttpClientBuilderExtensions
    {
        public static IServiceCollection EnableHttpClientAuditing(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IHttpCallingAuditingStore, NullHttpCallingAuditingStore>();
            services.TryAddSingleton<IHttpCallingAuditingHelper, HttpCallingAuditingHelper>();

            services.Add(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, AuditingHttpMessageHandlerBuilderFilter>());


            return services;
        }

        public static IServiceCollection EnableHttpClientLogging(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            //services.RemoveAll<IHttpMessageHandlerBuilderFilter>();

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHttpMessageHandlerBuilderFilter, LoggingHttpMessageHandlerBuilderFilter>());

            return services;

        }

        public static IHttpClientBuilder EnableHttpClientAuditing(this IHttpClientBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.EnableHttpClientAuditing();

            return builder;
        }

        public static IHttpClientBuilder EnableHttpClientLogging(this IHttpClientBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.EnableHttpClientLogging();

            return builder;

        }

    }
}
