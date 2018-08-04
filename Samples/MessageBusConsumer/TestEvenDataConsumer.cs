using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abpro.MessageBus.Consumer;
using MessageBusMqMessage;
using Rebus.Handlers;
using Abp.Dependency;
using Castle.Core.Logging;

namespace MessageBusConsumer
{
    public class TestEvenDataConsumer : RebusConsumerMqHandlerBase, IHandleMessages<TheFirstMqMessage>, ISingletonDependency
    {
        public ILogger Logger { get; set; }

        public TestEvenDataConsumer()
        {
            Logger = NullLogger.Instance;
        }

        public Task Handle(TheFirstMqMessage message)
        {
            var context = MessageContext;
            return Task.CompletedTask;
        }
    }
}
