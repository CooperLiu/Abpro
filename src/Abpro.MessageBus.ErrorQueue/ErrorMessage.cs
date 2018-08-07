using System;

namespace Abpro.MessageBus.ErrorQueue
{
    public class ErrorMessage
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 消息Id
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 原队列
        /// </summary>
        public string MessageQueue { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 消息发出时间
        /// </summary>
        public DateTime SentTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 错误明细
        /// </summary>
        public string ErrorDetail { get; set; }

        /// <summary>
        /// 消息行为意图
        /// </summary>
        public string Intent { get; set; }

        /// <summary>
        /// 消息内容类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
