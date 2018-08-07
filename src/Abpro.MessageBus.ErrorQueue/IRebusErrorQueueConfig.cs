using System;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Serialization;

namespace Abpro.MessageBus.ErrorQueue
{
    public interface IRebusErrorQueueConfig
    {
        /// <summary>
        /// Mq队列地址
        /// </summary>
        string MqConnectionString { get; }

        /// <summary>
        /// 错误队列名称
        /// </summary>
        string QueueName { get; }

        /// <summary>
        /// 日志配置器
        /// </summary>
        Action<RebusLoggingConfigurer> LoggingConfigurer { get; }

        /// <summary>
        /// 最大并行数
        /// </summary>
        int MaxParallelism { get; }

        /// <summary>
        /// 最大Worker数
        /// </summary>
        int NumberOfWorkers { get; }

        /// <summary>
        /// 错误消息处理
        /// </summary>
        Func<IHandleMessages<ErrorMessage>> ErrorMessageHandler { get; }


        /// <summary>
        /// 序列化组件配置
        /// </summary>
        Action<StandardConfigurer<ISerializer>> SerializerConfigurer { get; }

        /// <summary>
        /// 连接Mq
        /// </summary>
        /// <param name="mqConnectionString"></param>
        /// <returns></returns>
        IRebusErrorQueueConfig ConnectTo(string mqConnectionString);

        /// <summary>
        /// 使用队列名称
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        IRebusErrorQueueConfig UseQueue(string queueName);

        /// <summary>
        /// 日志
        /// </summary>
        /// <param name="loggingConfigurer"></param>
        /// <returns></returns>
        IRebusErrorQueueConfig UseLogging(Action<RebusLoggingConfigurer> loggingConfigurer);

        /// <summary>
        /// 设置最大并行数
        /// </summary>
        /// <param name="maxParallelism"></param>
        /// <returns></returns>
        IRebusErrorQueueConfig SetMaxParallelism(int maxParallelism);

        /// <summary>
        /// 设置最大工作线程数
        /// </summary>
        /// <param name="numberOfWorkers"></param>
        /// <returns></returns>
        IRebusErrorQueueConfig SetNumberOfWorkers(int numberOfWorkers);

        /// <summary>
        /// 注册错误处理类
        /// </summary>
        /// <param name="handlerFunc"></param>
        /// <returns></returns>
        IRebusErrorQueueConfig UserErrorMessageHandler(Func<IHandleMessages<ErrorMessage>> handlerFunc);
    }
}