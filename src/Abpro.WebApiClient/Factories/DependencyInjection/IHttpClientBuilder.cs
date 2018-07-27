using System;
using System.Collections.Generic;
using System.Net.Http;
using Castle.MicroKernel.Registration;

namespace Abp.WebApi.Client.DependencyInjection
{
    public interface IHttpClientBuilder
    {
        string Name { get; }

        IAbproWebApiClientIocManager IocManager { get; }
    }


    public class DefaultHttpClientBuilder : IHttpClientBuilder
    {
        public DefaultHttpClientBuilder(IAbproWebApiClientIocManager iocManager, string name)
        {
            Name = name;
            IocManager = iocManager;
        }


        public string Name { get; }


        public IAbproWebApiClientIocManager IocManager { get; }
    }

    public static class HttpClientBuilderExtensions
    {

        public static IHttpClientBuilder ConfigHttpClientBuilder(this IHttpClientBuilder builder,
            TimeSpan handlerLifetime,
            Action<HttpClient> configureClientAction,
            IEnumerable<Func<DelegatingHandler>> configureHandlerFuncs)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.IocManager.IocContainer.Register(
                Component.For<HttpClientFactoryOptions>().LifestyleTransient().Named(builder.Name).OnCreate(options =>
                {
                    options.HandlerLifetime = handlerLifetime;

                    if (configureClientAction != null)
                    {
                        options.HttpClientActions.Add(configureClientAction);
                    }

                    if (configureHandlerFuncs != null)
                    {
                        foreach (var func in configureHandlerFuncs)
                        {
                            options.HttpMessageHandlerBuilderActions.Add(b => b.AdditionalHandlers.Add(func()));
                        }
                    }
                })
            );

            return builder;
        }


        //public static IHttpClientBuilder ConfigureHttpClient(this IHttpClientBuilder builder,
        //    Action<HttpClient> configureClient)
        //{
        //    if (builder == null) throw new ArgumentNullException(nameof(builder));
        //    if (configureClient == null) throw new ArgumentNullException(nameof(configureClient));

        //    builder.IocManager.IocContainer.Register(
        //        Component.For<HttpClientFactoryOptions>().LifestyleTransient().Named(builder.Name).OnCreate(options => options.HttpClientActions.Add(configureClient))
        //        );

        //    return builder;
        //}



        //public static IHttpClientBuilder AddHttpMessageHandler(this IHttpClientBuilder builder,
        //    Func<DelegatingHandler> configureHandler)
        //{
        //    if (builder == null) throw new ArgumentNullException(nameof(builder));
        //    if (configureHandler == null) throw new ArgumentNullException(nameof(configureHandler));

        //    builder.IocManager.IocContainer.Register(
        //        Component
        //            .For<HttpClientFactoryOptions>()
        //            .LifestyleTransient()
        //            .Named(builder.Name)
        //            .OnCreate(options => options.HttpMessageHandlerBuilderActions.Add(b => b.AdditionalHandlers.Add(configureHandler())))
        //        );

        //    return builder;
        //}
    }
}