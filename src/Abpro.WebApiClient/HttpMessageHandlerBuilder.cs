using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Abpro.WebApiClient
{
    public interface IHttpMessageHandlerBuilder
    {
        string Name { get; set; }
        HttpMessageHandler PrimaryHandler { get; set; }
        IList<DelegatingHandler> AdditionalHandlers { get; }
        HttpMessageHandler Build();
    }

    public abstract class HttpMessageHandlerBuilder
    {
        public abstract string Name { get; set; }

        public abstract HttpMessageHandler PrimaryHandler { get; set; }

        public abstract IList<DelegatingHandler> AdditionalHandlers { get; }

        public abstract HttpMessageHandler Build();

        protected static HttpMessageHandler CreateHandlerPipeline(HttpMessageHandler primaryHandler, IEnumerable<DelegatingHandler> additionalHandlers)
        {
            if (primaryHandler == null) throw new ArgumentNullException(nameof(primaryHandler));
            if (additionalHandlers == null) throw new ArgumentNullException(nameof(additionalHandlers));

            var additionalHandlersList = additionalHandlers as IReadOnlyList<DelegatingHandler> ?? additionalHandlers.ToArray();

            var next = primaryHandler;
            for (int i = additionalHandlersList.Count - 1; i >= 0; i--)
            {
                var handler = additionalHandlersList[i];

                if (handler == null)
                {
                    throw new InvalidOperationException($"the additional {nameof(additionalHandlers)} must not contain null value");
                }

                if (handler.InnerHandler != null)
                {
                    throw new InvalidOperationException($"the {nameof(DelegatingHandler.InnerHandler)} must be null.");
                }


                handler.InnerHandler = next;
                next = handler;
            }

            return next;

        }
    }

    internal class DefaultHttpMessageHandlerBuilder : HttpMessageHandlerBuilder, IHttpMessageHandlerBuilder
    {
        private string _name;

        public override string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override HttpMessageHandler PrimaryHandler { get; set; } = new HttpClientHandler(); //need some more configuration 

        public override IList<DelegatingHandler> AdditionalHandlers => new List<DelegatingHandler>();

        public override HttpMessageHandler Build()
        {
            if (PrimaryHandler == null) throw new InvalidOperationException($"{PrimaryHandler} must not be null");

            return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
        }
    }

}
