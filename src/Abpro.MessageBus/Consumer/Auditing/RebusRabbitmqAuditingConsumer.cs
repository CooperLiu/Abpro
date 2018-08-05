using System;
using System.Collections.Generic;
using System.Text;
using Abp.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Abpro.MessageBus.Consumer.Auditing
{
    public class RebusRabbitmqAuditingConsumer : IRebusAuditingConsumer
    {
        private readonly IRebusConsumerConfig _config;

        public RebusRabbitmqAuditingConsumer(IRebusConsumerConfig config)
        {
            _config = config;
        }

        public void HandleAuditedMessage()
        {
            if (!_config.MessageAuditingEnabled)
            {
                return;
            }

            var connectionEndPointList = new List<AmqpTcpEndpoint>() { new AmqpTcpEndpoint(new Uri(_config.MqConnectionString)) };

            var factory = new ConnectionFactory();
            factory.Uri = _config.MqConnectionString;
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(30);

            using (var connection = factory.CreateConnection(connectionEndPointList))
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: _config.MessageAuditingQueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false
                        );

                    //channel.QueueBind()

                    var consumer = new EventingBasicConsumer(channel);

                    //channel.BasicConsume(_config.MessageAuditingQueueName, true, consumer);

                    /// channel.QueueBind(_config.MessageAuditingQueueName,)

                    consumer.Received += (sender, e) =>
                    {
                        var msg = new RebusAuditMessage()
                        {
                            MessageId = e.BasicProperties.Headers["rbs2-msg-id"].ToString(),
                            CorrelationId = e.BasicProperties.Headers["rbs2-corr-id"].ToString(),
                            CorrelateSeq = e.BasicProperties.Headers["rbs2-corr-seq"].To<int>(),
                            MessageType = e.BasicProperties.Headers["rbs2-msg-type"].ToString(),
                            ContentType = e.BasicProperties.Headers["rbs2-content-type"].ToString(),
                            ActionIntent = e.BasicProperties.Headers["rbs2-intent"].ToString(),
                            SentTime = e.BasicProperties.Headers["rbs2-senttime"].To<DateTimeOffset>(),
                            AuditTime = e.BasicProperties.Headers["rbs2-audit-copytime"].To<DateTimeOffset>(),
                            AuditMachine = e.BasicProperties.Headers["rbs2-audit-machine"].ToString(),
                            HandleQueue = e.BasicProperties.Headers["rbs2-audit-handlequeue"].ToString(),
                            HandleTime = e.BasicProperties.Headers["rbs2-audit-handletime"].To<DateTimeOffset>(),
                            Body = Encoding.UTF8.GetString(e.Body)
                        };

                        _config.AuditMessageHandler(msg);
                    };
                }
            }
        }

    }
}
