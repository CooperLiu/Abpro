using Abp;
using Abp.Dependency;
using Abpro.MessageBus.Idempotents;
using Rebus.Pipeline;

namespace Abpro.MessageBus.Consumer
{
    public class RebusConsumerMqHandlerBase : ITransientDependency
    {
        public IIdempotentCoordinator IdempotentCoordinator { get; set; }

        public IMessageContext MessageContext { get; set; }

        public RebusConsumerMqHandlerBase()//(string localizationSourceName)
        {
            //LocalizationSourceName = localizationSourceName;

            IdempotentCoordinator = InMemoryIdempotentCoordinator.InMemoryInstance;
        }
    }
}
