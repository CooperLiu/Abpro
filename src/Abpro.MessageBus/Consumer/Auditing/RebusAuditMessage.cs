using System;

namespace Abpro.MessageBus.Consumer.Auditing
{
    public class RebusAuditMessage
    {
        /// <summary>
        /// 消息Id，Headers[rbs2-msg-id]
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 消息关联Id，Headers[rbs2-corr-id]
        /// </summary>
        public string CorrelationId { get; set; }

        /// <summary>
        /// 消息关联顺序，Headers[rbs2-corr-seq]
        /// </summary>
        public int CorrelateSeq { get; set; }

        /// <summary>
        /// 消息类型，Headers[rbs2-msg-type]
        /// </summary>
        public string MessageType { get; set; }

        /// <summary>
        /// 消息内容，Headers[rbs2-content-type]
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 消息行为：Send or publish ，Headers[rbs2-intent]
        /// </summary>
        public string ActionIntent { get; set; }

        /// <summary>
        /// 消息发出时间，Headers[rbs2-senttime]
        /// </summary>
        public DateTimeOffset SentTime { get; set; }

        /// <summary>
        /// 消息审计时间，Headers[rbs2-audit-copytime]
        /// </summary>
        public DateTimeOffset AuditTime { get; set; }

        /// <summary>
        /// 客户端名，Headers[rbs2-audit-machine]
        /// </summary>
        public string AuditMachine { get; set; }

        /// <summary>
        /// 消息处理队列，Headers[rbs2-audit-handlequeue]
        /// </summary>
        public string HandleQueue { get; set; }

        /// <summary>
        /// 消息处理时间，Headers[rbs2-audit-handletime]
        /// </summary>
        public DateTimeOffset HandleTime { get; set; }

        /// <summary>
        /// 消息体
        /// </summary>
        public string Body { get; set; }

        public override string ToString()
        {
            return $"MessageId:{MessageId} \r\n CorrelationId:{CorrelationId} \r\n CorrelateSeq:{CorrelateSeq}\r\n MessageType:{MessageType}\r\n ContentType:{ContentType} \r\n ActionIntent:{ActionIntent}" +
                $"SentTime:{SentTime} \r\n AuditTime:{AuditTime} \r\n AuditMachine:{AuditMachine} \r\n HandleQueue:{HandleQueue} \r\n HandleTime:{HandleTime} \r\n Body:{Body}";
        }

        //var msg = new RebusAuditMessage()
        //{
        //    MessageId = e.BasicProperties.Headers["rbs2-msg-id"].ToString(),
        //    CorrelationId = e.BasicProperties.Headers["rbs2-corr-id"].ToString(),
        //    CorrelateSeq = e.BasicProperties.Headers["rbs2-corr-seq"].To<int>(),
        //    MessageType = e.BasicProperties.Headers["rbs2-msg-type"].ToString(),
        //    ContentType = e.BasicProperties.Headers["rbs2-content-type"].ToString(),
        //    ActionIntent = e.BasicProperties.Headers["rbs2-intent"].ToString(),
        //    SentTime = e.BasicProperties.Headers["rbs2-senttime"].To<DateTimeOffset>(),
        //    AuditTime = e.BasicProperties.Headers["rbs2-audit-copytime"].To<DateTimeOffset>(),
        //    AuditMachine = e.BasicProperties.Headers["rbs2-audit-machine"].ToString(),
        //    HandleQueue = e.BasicProperties.Headers["rbs2-audit-handlequeue"].ToString(),
        //    HandleTime = e.BasicProperties.Headers["rbs2-audit-handletime"].To<DateTimeOffset>(),
        //    Body = Encoding.UTF8.GetString(e.Body)
        //};
    }
}
