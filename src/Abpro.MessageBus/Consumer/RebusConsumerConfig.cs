using System;
using System.Reflection;
using Abp;
using Abp.Dependency;
using Abpro.MessageBus.Consumer.Auditing;
using Newtonsoft.Json;
using Rebus.Config;
#if NET461
using Rebus.NewtonsoftJson;
#endif
using Rebus.Serialization;
using Rebus.Subscriptions;

namespace Abpro.MessageBus.Consumer
{
    public class RebusConsumerConfig : IRebusConsumerConfig
    {
        public RebusConsumerConfig()
        {
            MessageAuditingEnabled = false;
            MaxParallelism = 1;
            NumberOfWorkers = 1;
#if NET461
            SerializerConfigurer = c => c.UseNewtonsoftJson(new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
#endif

        }

        /// <summary>
        /// RabbitMq连接字符串
        /// </summary>
        public string MqConnectionString { get; private set; }

        /// <summary>
        /// 队列名
        /// </summary>
        public string QueueName { get; private set; }

        /// <summary>
        /// 最大并行数
        /// </summary>
        public int MaxParallelism { get; private set; }

        /// <summary>
        /// 最大Worker数
        /// </summary>
        public int NumberOfWorkers { get; private set; }

        /// <summary>
        /// 消息审计是否开启，默认不开启
        /// </summary>
        public bool MessageAuditingEnabled { get; private set; }

        /// <summary>
        /// 消息审计队列名，默认不启用
        /// </summary>
        public string MessageAuditingQueueName { get; private set; }

        /// <summary>
        /// 包含RebusMqMessageHandler的程序集(自动订阅消息和自动注册handler)
        /// </summary>
        public Assembly[] AssemblysIncludeRebusMqMessageHandlers { get; private set; }

        /// <summary>
        /// 配置日志组件
        /// </summary>
        public Action<RebusLoggingConfigurer> LoggingConfigurer { get; private set; }

        /// <summary>
        /// 其他选项配置
        /// </summary>
        public Action<OptionsConfigurer> OptionsConfigurer { get; private set; }

        /// <summary>
        /// 序列化组件配置
        /// </summary>
        public Action<StandardConfigurer<ISerializer>> SerializerConfigurer { get; private set; }

        /// <summary>
        /// 订阅者持久化
        /// </summary>
        public Action<StandardConfigurer<ISubscriptionStorage>> SubscriptionStorage { get; private set; }

        public Action<RebusAuditMessage> AuditMessageHandler { get; private set; }

        public IRebusConsumerConfig ConnectTo(string mqConnectionString)
        {
            MqConnectionString = mqConnectionString ?? throw new ArgumentNullException(nameof(mqConnectionString));
            return this;
        }

        public IRebusConsumerConfig UseQueue(string queueName)
        {
            QueueName = queueName ?? throw new ArgumentNullException(nameof(queueName));

            return this;
        }

        public IRebusConsumerConfig SetMaxParallelism(int maxParallelism)
        {
            if (maxParallelism <= 0) throw new ArgumentOutOfRangeException(nameof(maxParallelism));

            MaxParallelism = maxParallelism;

            return this;
        }

        public IRebusConsumerConfig SetNumberOfWorkers(int numberOfWorkers)
        {
            if (numberOfWorkers <= 0) throw new ArgumentOutOfRangeException(nameof(numberOfWorkers));

            NumberOfWorkers = numberOfWorkers;

            return this;
        }

        public IRebusConsumerConfig EnableMessageAuditing(string messageAuditingQueueName, Action<RebusAuditMessage> auditMessageHandler = null)
        {
            MessageAuditingEnabled = true;
            MessageAuditingQueueName = messageAuditingQueueName ?? throw new ArgumentNullException(nameof(messageAuditingQueueName));

            //Action<RebusAuditMessage> handler = (message) => IocManager.Instance.Resolve<IAuditMessageHandler>().Handle(message);

            //AuditMessageHandler = auditMessageHandler ?? handler;

            return this;
        }

        public IRebusConsumerConfig RegisterHandlerInAssemblies(params Assembly[] assemblies)
        {
            AssemblysIncludeRebusMqMessageHandlers = assemblies ?? throw new ArgumentNullException(nameof(assemblies));

            return this;
        }

        public IRebusConsumerConfig UseLogging(Action<RebusLoggingConfigurer> loggingConfigurer)
        {
            LoggingConfigurer = loggingConfigurer ?? throw new ArgumentNullException(nameof(loggingConfigurer));

            return this;
        }

        public IRebusConsumerConfig UseOptions(Action<OptionsConfigurer> optionsConfigurer)
        {
            OptionsConfigurer = optionsConfigurer ?? throw new ArgumentNullException(nameof(optionsConfigurer));

            return this;
        }

        public IRebusConsumerConfig UseSerializer(Action<StandardConfigurer<ISerializer>> serializerConfigurer)
        {
            SerializerConfigurer = serializerConfigurer ?? throw new ArgumentNullException(nameof(serializerConfigurer));
            return this;
        }

    }
}