using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Abpro.WebApiClient.Factory
{
    internal class DefaultHttpMessageHandlerBuilder : HttpMessageHandlerBuilder, IHttpMessageHandlerBuilder
    {
        private string _name;

        public override string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override HttpMessageHandler PrimaryHandler { get; set; } = new HttpClientHandler();  

        public override IList<DelegatingHandler> AdditionalHandlers => new List<DelegatingHandler>();

        public override HttpMessageHandler Build()
        {
            if (PrimaryHandler == null) throw new InvalidOperationException($"{PrimaryHandler} must not be null");

            return CreateHandlerPipeline(PrimaryHandler, AdditionalHandlers);
        }
    }
}