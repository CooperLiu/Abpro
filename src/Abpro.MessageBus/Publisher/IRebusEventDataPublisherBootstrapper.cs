using Rebus.Bus;

namespace Abpro.MessageBus.Publisher
{
    public interface IRebusEventDataPublisherBootstrapper
    {
        IBus Start();
    }
}