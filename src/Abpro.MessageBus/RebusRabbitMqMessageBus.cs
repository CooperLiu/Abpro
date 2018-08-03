using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Threading;
using Rebus.Bus;

namespace Abpro.MessageBus
{
    public class RebusRabbitMqMessageBus : IMessageBus
    {
        public IBus MessageBus { get; set; }

        public RebusRabbitMqMessageBus()
        {
        }

        public void Publish(object message, Dictionary<string, string> optionalHeaders = null)
        {
            AsyncHelper.RunSync(() => MessageBus.Publish(message, optionalHeaders));
        }

        public async Task PublishAsync(object message, Dictionary<string, string> optionalHeaders = null)
        {
            await MessageBus.Publish(message, optionalHeaders);
        }

        public async Task DeferAsync(TimeSpan delay, object message, Dictionary<string, string> optionalHeaders = null)
        {
            await MessageBus.Defer(delay, message, optionalHeaders);
        }


    }
}