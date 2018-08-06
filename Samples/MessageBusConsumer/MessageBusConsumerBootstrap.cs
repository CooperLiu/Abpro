using Abp;
using NLog;
using NLog.Config;
using System.Configuration;
using Abpro.MessageBus.Consumer;
using Abpro.MessageBus.Consumer.Auditing;

namespace MessageBusConsumer
{
    public class MessageBusConsumerBootstrap
    {
        private static readonly AbpBootstrapper _bs = AbpBootstrapper.Create<MessageBusConsumerModule>();


        public void Start()
        {
            LogManager.Configuration = new XmlLoggingConfiguration(ConfigurationManager.AppSettings["NlogConfigFilePath"]);
            _bs.Initialize();

        }

        public void Stop()
        {
            _bs.Dispose();
        }
    }
}