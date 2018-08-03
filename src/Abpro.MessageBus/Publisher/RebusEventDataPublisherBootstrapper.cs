using Abp.Dependency;
using Abp.Extensions;
using Rebus.Auditing.Messages;
using Rebus.Bus;
using Rebus.Config;

namespace Abpro.MessageBus.Publisher
{
    public class RebusEventDataPublisherBootstrapper : IRebusEventDataPublisherBootstrapper
    {
        private readonly IRebusEventDataPublisherConfig _config;

        private readonly IIocManager _iocManager;

        public RebusEventDataPublisherBootstrapper(IRebusEventDataPublisherConfig config, IIocManager iocManager)
        {
            _config = config;
            _iocManager = iocManager;
        }


        public IBus Start()
        {
            var configurer = Configure.With(new CastleWindsorContainerAdapter(_iocManager.IocContainer));

            if (_config.LoggingConfigurer != null) configurer.Logging(_config.LoggingConfigurer);

            if (_config.EnabledMessageAuditing && !_config.MessageAuditingQueueName.IsNullOrEmpty())
            {
                configurer.Options(o => o.EnableMessageAuditing(_config.MessageAuditingQueueName));
            }

            return configurer
                .Transport(t => t.UseRabbitMqAsOneWayClient(_config.MqConnectionString))
                .Start();

        }
    }
}