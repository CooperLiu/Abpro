using System;

namespace Abp.WebApi.Client
{
    public interface IHttpMessageHandlerBuilderFilter
    {
        /// <summary>
        /// Applies additional initialization to the <see cref="HttpMessageHandlerBuilder"/>
        /// </summary>
        /// <param name="next">A delegate which will run the next <see cref="IHttpMessageHandlerBuilderFilter"/>.</param>
        Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next);
    }
}