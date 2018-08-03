using System.Threading.Tasks;
using Rebus.Subscriptions;

namespace Abpro.MessageBus.Consumer
{
    public class CacheSubcriptionStorage : ISubscriptionStorage
    {
        public Task<string[]> GetSubscriberAddresses(string topic)
        {
            throw new System.NotImplementedException();
        }

        public Task RegisterSubscriber(string topic, string subscriberAddress)
        {
            throw new System.NotImplementedException();
        }

        public Task UnregisterSubscriber(string topic, string subscriberAddress)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets whether the subscription storage is centralized and thus supports bypassing the usual subscription request
        /// (in a fully distributed architecture, a subscription is established by sending a <see cref="T:Rebus.Messages.Control.SubscribeRequest" />
        /// to the owner of a given topic, who then remembers the subscriber somehow - if the subscription storage is
        /// centralized, the message exchange can be bypassed, and the subscription can be established directly by
        /// having the subscriber register itself)
        /// </summary>
        public bool IsCentralized { get; } = true;
    }
}