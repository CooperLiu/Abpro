using Rebus.Bus;

namespace Abpro.MessageBus.Consumer
{
    public interface IRebusConsumerBootstrapper
    {
        /// <summary>
        /// 启动Rebus
        /// </summary>
        /// <returns></returns>
        IBus Start();
    }
}