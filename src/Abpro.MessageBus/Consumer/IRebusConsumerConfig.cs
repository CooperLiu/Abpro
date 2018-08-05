using System;
using System.Reflection;
using Abpro.MessageBus.Consumer.Auditing;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Serialization;
using Rebus.Subscriptions;


namespace Abpro.MessageBus.Consumer
{
    public interface IRebusConsumerConfig
    {
        /// <summary>
        /// RabbitMq连接字符串
        /// </summary>
        string MqConnectionString { get; }

        /// <summary>
        /// 队列名
        /// </summary>
        string QueueName { get; }

        /// <summary>
        /// 最大并行数
        /// </summary>
        int MaxParallelism { get; }

        /// <summary>
        /// 最大Worker数
        /// </summary>
        int NumberOfWorkers { get; }

        /// <summary>
        /// 消息审计是否开启，默认不开启
        /// </summary>
        bool MessageAuditingEnabled { get; }

        /// <summary>
        /// 消息审计队列名，默认不启用
        /// </summary>
        string MessageAuditingQueueName { get; }

        /// <summary>
        /// 包含RebusMqMessageHandler的程序集(自动订阅消息和自动注册handler)
        /// </summary>
        Assembly[] AssemblysIncludeRebusMqMessageHandlers { get; }

        /// <summary>
        /// 配置日志组件
        /// </summary>
        Action<RebusLoggingConfigurer> LoggingConfigurer { get; }

        /// <summary>
        /// 其他选项配置
        /// </summary>
        Action<OptionsConfigurer> OptionsConfigurer { get; }

        /// <summary>
        /// 序列化组件配置
        /// </summary>
        Action<StandardConfigurer<ISerializer>> SerializerConfigurer { get; }

        /// <summary>
        /// 订阅者持久化
        /// </summary>
        Action<StandardConfigurer<ISubscriptionStorage>> SubscriptionStorage { get; }

        Action<RebusAuditMessage> AuditMessageHandler { get; }

        /// <summary>
        /// 连接RabbitMq
        /// </summary>
        /// <param name="mqConnectionString">RabbitMq连接字符串</param>
        /// <returns></returns>
        IRebusConsumerConfig ConnectTo(string mqConnectionString);

        /// <summary>
        /// 队列名
        /// </summary>
        /// <param name="queueName">使用队列名</param>
        /// <returns></returns>
        IRebusConsumerConfig UseQueue(string queueName);

        /// <summary>
        /// 配置最大并行数
        /// </summary>
        /// <param name="maxParallelism">最大并行数</param>
        /// <returns></returns>
        IRebusConsumerConfig SetMaxParallelism(int maxParallelism);

        /// <summary>
        /// 配置最大Worker数量
        /// </summary>
        /// <param name="numberOfWorkers">最大Worker数</param>
        /// <returns></returns>
        IRebusConsumerConfig SetNumberOfWorkers(int numberOfWorkers);

        /// <summary>
        /// 启用消息审计
        /// </summary>
        /// <param name="messageAuditingQueueName">消息审计队列名</param>
        /// <returns></returns>
        IRebusConsumerConfig EnableMessageAuditing(string messageAuditingQueueName, Action<RebusAuditMessage> auditMessageHandler = null);

        /// <summary>
        /// 注册Rebus Handlers
        /// </summary>
        /// <param name="assemblies">包含Rebus Handlers的程序集</param>
        /// <returns></returns>
        IRebusConsumerConfig RegisterHandlerInAssemblies(params Assembly[] assemblies);

        /// <summary>
        /// 配置日志组件
        /// </summary>
        /// <param name="loggingConfigurer"></param>
        /// <returns></returns>
        IRebusConsumerConfig UseLogging(Action<RebusLoggingConfigurer> loggingConfigurer);
        /// <summary>
        /// 配置其他选项
        /// </summary>
        /// <param name="optionsConfigurer"></param>
        /// <returns></returns>
        IRebusConsumerConfig UseOptions(Action<OptionsConfigurer> optionsConfigurer);

        /// <summary>
        /// 配置序列化
        /// </summary>
        /// <param name="serializerConfigurer"></param>
        /// <returns></returns>
        IRebusConsumerConfig UseSerializer(Action<StandardConfigurer<ISerializer>> serializerConfigurer);

    }
}