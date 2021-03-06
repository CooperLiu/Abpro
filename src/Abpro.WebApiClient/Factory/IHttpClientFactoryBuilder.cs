﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Castle.MicroKernel.Registration;

namespace Abpro.WebApiClient.Factory
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

            if (!builder.Services.IsRegistered<HttpClientFactoryOptions>())
            {

                builder.Services.IocContainer.Kernel.Register(
                    Component
                        .For(typeof(HttpClientFactoryOptions))
                        .LifestyleTransient()
                        .Named(builder.Name)
                    .OnCreate(a => AddOptionsHttpClientActions((HttpClientFactoryOptions)a, configureClient))
                    );
            }

            //builder.Services.IocContainer.Kernel.ComponentCreated += HttpClientOptionsComponentCreated;

            return builder;
        }

        private static void AddOptionsHttpClientActions(HttpClientFactoryOptions options, Action<HttpClient> configureClient)
        {
            options.HttpClientActions.Add(configureClient);
        }

        public static IHttpClientFactoryBuilder ConfigurePrimaryHttpMessageHandler(this IHttpClientFactoryBuilder builder,
            Func<HttpMessageHandler> configureHandler)
        {

            if (configureHandler == null) throw new ArgumentNullException(nameof(configureHandler));

            if (!builder.Services.IsRegistered<HttpClientFactoryOptions>())
            {

                builder.Services.IocContainer.Kernel.Register(
                    Component
                        .For(typeof(HttpClientFactoryOptions))
                        .LifestyleTransient()
                        .Named(builder.Name)
                        .OnCreate(a => { var o = (HttpClientFactoryOptions)a; o.HttpMessageHandlerBuilderActions.Add(b => b.PrimaryHandler = configureHandler()); })
                    );
            }

            //builder.Services.IocContainer.Kernel.ComponentCreated += Kernel_ComponentCreated;

            //var options = builder.Services.IocContainer.Resolve<HttpClientFactoryOptions>(builder.Name);

            //options.HttpMessageHandlerBuilderActions.Add(b => b.PrimaryHandler = configureHandler());

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

                Component.For<IHttpMessageHandlerBuilder, DefaultHttpMessageHandlerBuilder>().LifestyleTransient(),

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