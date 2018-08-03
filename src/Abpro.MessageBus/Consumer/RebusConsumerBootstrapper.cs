using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Dependency;
using Rebus.Auditing.Messages;
using Rebus.Bus;
using Rebus.CastleWindsor;
using Rebus.Config;
using Rebus.Handlers;

namespace Abpro.MessageBus.Consumer
{
    public class RebusConsumerBootstrapper : IRebusConsumerBootstrapper
    {
        private readonly IRebusConsumerConfig _config;

        private readonly IIocManager _iocManager;

        public RebusConsumerBootstrapper(
            IRebusConsumerConfig config,
            IIocManager iocManager)
        {
            _config = config;
            _iocManager = iocManager;
        }

        /// <summary>
        /// 启动Rebus
        /// </summary>
        /// <returns></returns>
        public IBus Start()
        {
            var configurer = Configure.With(new CastleWindsorContainerAdapter(_iocManager.IocContainer));

            if (_config.LoggingConfigurer != null) configurer.Logging(_config.LoggingConfigurer);

            if (_config.SerializerConfigurer != null) configurer.Serialization(_config.SerializerConfigurer);

            if (_config.OptionsConfigurer != null) configurer.Options(_config.OptionsConfigurer);

            configurer.Options(c =>
            {
                c.SetMaxParallelism(_config.MaxParallelism);
                c.SetNumberOfWorkers(_config.NumberOfWorkers);
            });

            if (_config.MessageAuditingEnabled)
            {
                configurer.Options(o => o.EnableMessageAuditing(_config.MessageAuditingQueueName));
            }

            if (_config.SubscriptionStorage != null) configurer.Subscriptions(_config.SubscriptionStorage);

            var mqMessageTypes = new List<Type>();

            //Register handlers first!
            foreach (var assembly in _config.AssemblysIncludeRebusMqMessageHandlers)
            {
                _iocManager.IocContainer.AutoRegisterHandlersFromAssembly(assembly);

                mqMessageTypes.AddRange(assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandleMessages<>)))
                    .SelectMany(t => t.GetInterfaces())
                    .Distinct()
                    .SelectMany(t => t.GetGenericArguments())
                    .Distinct());
            }

            var bus = configurer
                .Transport(c => c.UseRabbitMq(_config.MqConnectionString, _config.QueueName))
                .Start();

            //Subscribe messages
            mqMessageTypes = mqMessageTypes.Distinct().ToList();

            foreach (var mqMessageType in mqMessageTypes)
            {
                bus.Subscribe(mqMessageType);
            }

            return bus;
        }
    }
}