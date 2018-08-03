using System;
using Rebus.Config;

namespace Abpro.MessageBus.Publisher
{
    public interface IRebusEventDataPublisherConfig
    {
        /// <summary>
        /// Mq队列地址
        /// </summary>
        string MqConnectionString { get; }

        /// <summary>
        /// 是否启用消息审计
        /// </summary>
        bool EnabledMessageAuditing { get; }

        /// <summary>
        /// 消息审计队列
        /// </summary>
        string MessageAuditingQueueName { get; }

        /// <summary>
        /// 日志配置器
        /// </summary>
        Action<RebusLoggingConfigurer> LoggingConfigurer { get; }

        /// <summary>
        /// 连接Mq队列地址
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        IRebusEventDataPublisherConfig ConnectionTo(string connectionString);

        /// <summary>
        /// 是否启用消息审计
        /// </summary>
        /// <param name="messageAuditingQueueName"></param>
        /// <returns></returns>
        IRebusEventDataPublisherConfig EnableMessageAuditing(string messageAuditingQueueName);

        /// <summary>
        /// 配置日志
        /// </summary>
        /// <param name="loggingConfigurer"></param>
        /// <returns></returns>
        IRebusEventDataPublisherConfig UseLogging(Action<RebusLoggingConfigurer> loggingConfigurer);
    }
}