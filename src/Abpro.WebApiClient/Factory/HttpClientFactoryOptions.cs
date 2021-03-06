﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;

namespace Abpro.WebApiClient.Factory
{
    public interface IHttpClientFactoryOptions
    {
        /// <summary>
        /// Gets a list of operations used to configure an <see cref="HttpMessageHandlerBuilder"/>.
        /// </summary>
        IList<Action<IHttpMessageHandlerBuilder>> HttpMessageHandlerBuilderActions { get; }

        /// <summary>
        /// Gets a list of operations used to configure an <see cref="HttpClient"/>.
        /// </summary>
        IList<Action<HttpClient>> HttpClientActions { get; }

        /// <summary>
        /// Gets or sets the length of time that a <see cref="HttpMessageHandler"/> instance can be reused. Each named 
        /// client can have its own configured handler lifetime value. The default value of this property is two minutes.
        /// Set the lifetime to <see cref="Timeout.InfiniteTimeSpan"/> to disable handler expiry.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default implementation of <see cref="IHttpClientFactory"/> will pool the <see cref="HttpMessageHandler"/>
        /// instances created by the factory to reduce resource consumption. This setting configures the amount of time
        /// a handler can be pooled before it is scheduled for removal from the pool and disposal.
        /// </para>
        /// <para>
        /// Pooling of handlers is desirable as each handler typically manages its own underlying HTTP connections; creating
        /// more handlers than necessary can result in connection delays. Some handlers also keep connections open indefinitely
        /// which can prevent the handler from reacting to DNS changes. The value of <see cref="HandlerLifetime"/> should be
        /// chosen with an understanding of the application's requirement to respond to changes in the network environment.
        /// </para>
        /// <para>
        /// Expiry of a handler will not immediately dispose the handler. An expired handler is placed in a separate pool 
        /// which is processed at intervals to dispose handlers only when they become unreachable. Using long-lived
        /// <see cref="HttpClient"/> instances will prevent the underlying <see cref="HttpMessageHandler"/> from being
        /// disposed until all references are garbage-collected.
        /// </para>
        /// </remarks>
        TimeSpan HandlerLifetime { get; set; }

        /// <summary>
        /// 是否开启Http请求审计
        /// </summary>
        bool IsEnableHttpCallingAuditing { get; }

        /// <summary>
        /// 增加HttpMessageHandler配置方法
        /// </summary>
        /// <param name="builderAction"></param>
        /// <returns></returns>
        IHttpClientFactoryOptions AddHttpMessageHandlerBuilderAction(Action<IHttpMessageHandlerBuilder> builderAction);


        /// <summary>
        /// 增加HttpClient配置方法
        /// </summary>
        /// <param name="clientAction"></param>
        /// <returns></returns>
        IHttpClientFactoryOptions AddHttpClientAction(Action<HttpClient> clientAction);

        /// <summary>
        /// 启用Http请求审计
        /// </summary>
        /// <returns></returns>
        IHttpClientFactoryOptions EnableHttpCallingAuditing();
    }

    /// <summary>
    /// An options class for configuring the default <see cref="IHttpClientFactory"/>.
    /// </summary>
    public class HttpClientFactoryOptions : IHttpClientFactoryOptions
    {
        // Establishing a minimum lifetime helps us avoid some possible destructive cases.
        //
        // IMPORTANT: This is used in a resource string. Update the resource if this changes.
        internal readonly static TimeSpan MinimumHandlerLifetime = TimeSpan.FromSeconds(1);

        private TimeSpan _handlerLifetime = TimeSpan.FromMinutes(2);

        /// <summary>
        /// Gets a list of operations used to configure an <see cref="HttpMessageHandlerBuilder"/>.
        /// </summary>
        public IList<Action<IHttpMessageHandlerBuilder>> HttpMessageHandlerBuilderActions { get; } = new List<Action<IHttpMessageHandlerBuilder>>();

        /// <summary>
        /// Gets a list of operations used to configure an <see cref="HttpClient"/>.
        /// </summary>
        public IList<Action<HttpClient>> HttpClientActions { get; } = new List<Action<HttpClient>>();

        /// <summary>
        /// Gets or sets the length of time that a <see cref="HttpMessageHandler"/> instance can be reused. Each named 
        /// client can have its own configured handler lifetime value. The default value of this property is two minutes.
        /// Set the lifetime to <see cref="Timeout.InfiniteTimeSpan"/> to disable handler expiry.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default implementation of <see cref="IHttpClientFactory"/> will pool the <see cref="HttpMessageHandler"/>
        /// instances created by the factory to reduce resource consumption. This setting configures the amount of time
        /// a handler can be pooled before it is scheduled for removal from the pool and disposal.
        /// </para>
        /// <para>
        /// Pooling of handlers is desirable as each handler typically manages its own underlying HTTP connections; creating
        /// more handlers than necessary can result in connection delays. Some handlers also keep connections open indefinitely
        /// which can prevent the handler from reacting to DNS changes. The value of <see cref="HandlerLifetime"/> should be
        /// chosen with an understanding of the application's requirement to respond to changes in the network environment.
        /// </para>
        /// <para>
        /// Expiry of a handler will not immediately dispose the handler. An expired handler is placed in a separate pool 
        /// which is processed at intervals to dispose handlers only when they become unreachable. Using long-lived
        /// <see cref="HttpClient"/> instances will prevent the underlying <see cref="HttpMessageHandler"/> from being
        /// disposed until all references are garbage-collected.
        /// </para>
        /// </remarks>
        public TimeSpan HandlerLifetime
        {
            get => _handlerLifetime;
            set
            {
                if (value != Timeout.InfiniteTimeSpan && value < MinimumHandlerLifetime)
                {
                    throw new ArgumentException($"Handler's lifetime is invalid value {nameof(value)}");
                }

                _handlerLifetime = value;
            }
        }

        /// <summary>
        /// 是否开启Http请求审计
        /// </summary>
        public bool IsEnableHttpCallingAuditing { get; private set; }

        /// <summary>
        /// 启用Http请求审计
        /// </summary>
        /// <returns></returns>
        public IHttpClientFactoryOptions EnableHttpCallingAuditing()
        {
            IsEnableHttpCallingAuditing = true;

            return this;
        }


        public IHttpClientFactoryOptions AddHttpMessageHandlerBuilderAction(Action<IHttpMessageHandlerBuilder> builderAction)
        {
            if (builderAction == null) throw new ArgumentNullException(nameof(builderAction));

            HttpMessageHandlerBuilderActions.Add(builderAction);


            return this;
        }

        public IHttpClientFactoryOptions AddHttpClientAction(Action<HttpClient> clientAction)
        {
            if (clientAction == null) throw new ArgumentNullException(nameof(clientAction));

            HttpClientActions.Add(clientAction);

            return this;
        }
    }
}
