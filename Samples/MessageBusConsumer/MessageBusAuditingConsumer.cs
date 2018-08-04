using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rebus.Bus;

namespace MessageBusConsumer
{
    public class MessageBusAuditingConsumer
    {
        private readonly IBus _bus;

        public MessageBusAuditingConsumer(IBus bus)
        {
            _bus = bus;
        }

        public void HandleMqAuditedMessage()
        {
            _bus.
        }
    }
}
