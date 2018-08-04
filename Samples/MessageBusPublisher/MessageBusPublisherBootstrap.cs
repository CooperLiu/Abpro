using Abp;
using NLog;
using NLog.Config;
using System.Configuration;

namespace MessageBusPublisher
{
    internal class MessageBusPublisherBootstrap
    {
        private static readonly AbpBootstrapper _bs = AbpBootstrapper.Create<MessageBusPublisherModule>();

        public void Start()
        {
            LogManager.Configuration = new XmlLoggingConfiguration(ConfigurationManager.AppSettings["NlogConfigFilePath"]);
            _bs.Initialize();
            TestEventDataTrigger.TestTheFistCaseEventDataTrigger();
        }

        public void Stop()
        {
            _bs.Dispose();
        }
    }
}