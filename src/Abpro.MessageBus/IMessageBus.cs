using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rebus.Bus;

namespace Abpro.MessageBus
{
    public interface IMessageBus
    {
        IBus MessageBus { get; set; }

        void Publish(object mqMessage, Dictionary<string, string> optionalHeaders = null);

        Task PublishAsync(object message, Dictionary<string, string> optionalHeaders = null);

        Task DeferAsync(TimeSpan delay, object message, Dictionary<string, string> optionalHeaders = null);
    }

    public class NullMessageBus : IMessageBus
    {
        public static readonly NullMessageBus Instance = new NullMessageBus();

        public IBus MessageBus { get; set; }

        public void Publish(object mqMessage, Dictionary<string, string> optionalHeaders = null)
        {

        }

        public Task PublishAsync(object message, Dictionary<string, string> optionalHeaders = null)
        {
            return Task.CompletedTask;
        }

        public Task DeferAsync(TimeSpan delay, object message, Dictionary<string, string> optionalHeaders = null)
        {
            return Task.CompletedTask;
        }
    }
}
