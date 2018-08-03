using System;
using Rebus.Config;

namespace Abpro.MessageBus.Publisher
{
    public class RebusEventDataPublisherConfig : IRebusEventDataPublisherConfig
    {
        /// <summary>
        /// Mq队列地址
        /// </summary>
        public string MqConnectionString { get; private set; }

        /// <summary>
        /// 是否启用消息审计
        /// </summary>
        public bool EnabledMessageAuditing { get; private set; }

        /// <summary>
        /// 消息审计队列
        /// </summary>
        public string MessageAuditingQueueName { get; private set; }

        /// <summary>
        /// 日志配置器
        /// </summary>
        public Action<RebusLoggingConfigurer> LoggingConfigurer { get; private set; }

        /// <summary>
        /// 连接Mq队列地址
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IRebusEventDataPublisherConfig ConnectionTo(string connectionString)
        {
            MqConnectionString = connectionString;
            return this;
        }

        /// <summary>
        /// 是否启用消息审计
        /// </summary>
        /// <param name="messageAuditingQueueName"></param>
        /// <returns></returns>
        public IRebusEventDataPublisherConfig EnableMessageAuditing(string messageAuditingQueueName)
        {
            EnabledMessageAuditing = true;
            MessageAuditingQueueName = messageAuditingQueueName;
            return this;
        }

        /// <summary>
        /// 配置日志
        /// </summary>
        /// <param name="loggingConfigurer"></param>
        /// <returns></returns>
        public IRebusEventDataPublisherConfig UseLogging(Action<RebusLoggingConfigurer> loggingConfigurer)
        {
            LoggingConfigurer = loggingConfigurer;
            return this;
        }

    }
}