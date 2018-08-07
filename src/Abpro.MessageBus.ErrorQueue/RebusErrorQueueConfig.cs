using System;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Serialization;

namespace Abpro.MessageBus.ErrorQueue
{
    public class RebusErrorQueueConfig : IRebusErrorQueueConfig
    {
        public RebusErrorQueueConfig()
        {
            QueueName = "error";
            MaxParallelism = 1;
            NumberOfWorkers = 1;
        }

        /// <summary>
        /// Mq队列地址
        /// </summary>
        public string MqConnectionString { get; private set; }

        /// <summary>
        /// 错误队列名称
        /// </summary>
        public string QueueName { get; private set; }


        /// <summary>
        /// 日志配置器
        /// </summary>
        public Action<RebusLoggingConfigurer> LoggingConfigurer { get; private set; }

        /// <summary>
        /// 最大并行数
        /// </summary>
        public int MaxParallelism { get; private set; }

        /// <summary>
        /// 最大Worker数
        /// </summary>
        public int NumberOfWorkers { get; private set; }

        /// <summary>
        /// 错误消息处理
        /// </summary>
        public Func<IHandleMessages<ErrorMessage>> ErrorMessageHandler { get; private set; }

        /// <summary>
        /// 序列化组件配置
        /// </summary>
        public Action<StandardConfigurer<ISerializer>> SerializerConfigurer { get; private set; }

        /// <summary>
        /// 连接Mq
        /// </summary>
        /// <param name="mqConnectionString"></param>
        /// <returns></returns>
        public IRebusErrorQueueConfig ConnectTo(string mqConnectionString)
        {
            MqConnectionString = mqConnectionString ?? throw new ArgumentNullException(nameof(mqConnectionString));
            return this;
        }

        /// <summary>
        /// 使用队列名称
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public IRebusErrorQueueConfig UseQueue(string queueName)
        {
            QueueName = queueName ?? throw new ArgumentNullException(nameof(queueName));

            return this;
        }

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="loggingConfigurer"></param>
        /// <returns></returns>
        public IRebusErrorQueueConfig UseLogging(Action<RebusLoggingConfigurer> loggingConfigurer)
        {
            LoggingConfigurer = loggingConfigurer ?? throw new ArgumentNullException(nameof(loggingConfigurer));

            return this;
        }

        /// <summary>
        /// 设置最大并行数
        /// </summary>
        /// <param name="maxParallelism"></param>
        /// <returns></returns>
        public IRebusErrorQueueConfig SetMaxParallelism(int maxParallelism)
        {
            if (maxParallelism <= 0) throw new ArgumentOutOfRangeException(nameof(maxParallelism));

            MaxParallelism = maxParallelism;

            return this;
        }

        /// <summary>
        /// 设置最大工作线程数
        /// </summary>
        /// <param name="numberOfWorkers"></param>
        /// <returns></returns>
        public IRebusErrorQueueConfig SetNumberOfWorkers(int numberOfWorkers)
        {
            if (numberOfWorkers <= 0) throw new ArgumentOutOfRangeException(nameof(numberOfWorkers));

            NumberOfWorkers = numberOfWorkers;

            return this;
        }

        /// <summary>
        /// 注册错误处理类
        /// </summary>
        /// <param name="handlerFunc"></param>
        /// <returns></returns>
        public IRebusErrorQueueConfig UserErrorMessageHandler(Func<IHandleMessages<ErrorMessage>> handlerFunc)
        {
            ErrorMessageHandler = handlerFunc ?? throw new ArgumentNullException(nameof(handlerFunc));

            return this;
        }
    }
}