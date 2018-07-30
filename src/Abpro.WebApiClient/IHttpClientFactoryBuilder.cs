using System;
using System.Collections.Generic;
using System.Net.Http;
using Castle.MicroKernel.Registration;

namespace Abpro.WebApiClient
{
    public interface IHttpClientFactoryBuilder
    {
        string Name { get; set; }

        IHttpClientFactoryDependency Services { get; set; }
    }

    public class DefaultHttpClientFactoryBuilder : IHttpClientFactoryBuilder
    {
        public DefaultHttpClientFactoryBuilder(string name, IHttpClientFactoryDependency services)
        {
            Name = name;
            Services = services;
        }

        public string Name { get; set; }

        public IHttpClientFactoryDependency Services { get; set; }

    }

    public static class HttpClientFactoryBuilderExtensions
    {
        public static IHttpClientFactoryBuilder ConfigureHttpClient(this IHttpClientFactoryBuilder builder,
            Action<HttpClient> configureClient)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configureClient == null) throw new ArgumentNullException(nameof(configureClient));

            //builder.Services.IocContainer.Kernel.Register(
            //    Component
            //        .For(typeof(HttpClientFactoryOptions))
            //        .LifestyleTransient()
            //        .Named(builder.Name)
            //        .DependsOn()
            //    );

            //var options = builder.Services.IocContainer.Resolve<HttpClientFactoryOptions>(builder.Name);

            //options.HttpClientActions.Add(configureClient);

            return builder;
        }
    }


    public static class HttpClientFactoryDependencyExtensions
    {
        internal static bool IsRegistered<TType>(this IHttpClientFactoryDependency dependency)
        {
            return dependency.IocContainer.Kernel.HasComponent(typeof(TType));
        }

        internal static bool IsRegistered(this IHttpClientFactoryDependency dependency, string componentName)
        {
            return dependency.IocContainer.Kernel.HasComponent(componentName);
        }

        public static IHttpClientFactoryDependency AddHttpClient(this IHttpClientFactoryDependency dependency)
        {
            if (dependency == null) throw new ArgumentNullException(nameof(dependency));


            dependency.IocContainer.Register(

                Component.For<HttpMessageHandlerBuilder, DefaultHttpMessageHandlerBuilder>().LifestyleTransient(),

                Component.For<IHttpClientFactory, DefaultHttpClientFactory>().LifestyleSingleton()

                );

            if (!dependency.IsRegistered<IHttpMessageHandlerBuilderFilter>())
            {
                dependency.IocContainer.Register(
                    Component.For<IHttpMessageHandlerBuilderFilter, LoggingHttpMessageHandlerBuilderFilter>().LifestyleSingleton()
                );
            }

            return dependency;
        }

        public static IHttpClientFactoryBuilder AddHttpClient(this IHttpClientFactoryDependency dependency,
            string name)
        {
            if (dependency == null) throw new ArgumentNullException(nameof(dependency));
            if (name == null) throw new ArgumentNullException(nameof(name));

            dependency.AddHttpClient();

            return new DefaultHttpClientFactoryBuilder(name, dependency);
        }

        public static IHttpClientFactoryBuilder AddHttpClient(this IHttpClientFactoryDependency dependency,
            string name,
            Action<HttpClient> configureClientAction
        )
        {
            if (dependency == null) throw new ArgumentNullException(nameof(dependency));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (configureClientAction == null) throw new ArgumentNullException(nameof(configureClientAction));

            dependency.AddHttpClient();

            var builder = new DefaultHttpClientFactoryBuilder(name, dependency);

            builder.ConfigureHttpClient(configureClientAction);

            return builder;
        }
    }
}