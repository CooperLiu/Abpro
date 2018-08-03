using Abp;
using NLog;
using NLog.Config;
using System.Configuration;

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