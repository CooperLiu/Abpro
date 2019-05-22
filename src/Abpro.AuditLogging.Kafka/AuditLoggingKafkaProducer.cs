using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Dependency;
using Castle.Core.Logging;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace Abpro.AuditLogging.Kafka
{
    public class AuditLoggingKafkaProducer : IAuditingStore, ITransientDependency
    {
        private readonly ProducerConfig _config;

        public ILogger Logger { get; set; }


        private readonly IAuditLoggingKafkaConfig _kafkaConfig;
        public AuditLoggingKafkaProducer(IAuditLoggingKafkaConfig kafkaConfig)
        {
            _kafkaConfig = kafkaConfig;
            _config = new ProducerConfig() { BootstrapServers = kafkaConfig.BootstrapServers };
            Logger = NullLogger.Instance;
        }


        public async Task SaveAsync(AuditInfo auditInfo)
        {
            var m = new AuditInfoMqMessage();
            m.TenantId = auditInfo.TenantId;
            m.UserId = auditInfo.UserId;
            m.ImpersonatorUserId = auditInfo.ImpersonatorUserId;
            m.ImpersonatorTenantId = auditInfo.ImpersonatorTenantId;
            m.ServiceName = auditInfo.ServiceName;
            m.MethodName = auditInfo.MethodName;
            m.Parameters = auditInfo.Parameters;
            m.ExecutionTime = auditInfo.ExecutionTime;
            m.ExecutionDuration = auditInfo.ExecutionDuration;
            m.ClientIpAddress = auditInfo.ClientIpAddress;
            m.ClientName = auditInfo.ClientName;
            m.BrowserInfo = auditInfo.BrowserInfo;
            m.CustomData = auditInfo.CustomData;


            if (auditInfo.Exception != null)
            {
                m.CustomData += $" {auditInfo.Exception.Message}";
                m.Exception = auditInfo.Exception.StackTrace;
                if (auditInfo.Exception.InnerException != null)
                {
                    m.CustomData += $" {auditInfo.Exception.InnerException.Message}";
                    m.Exception = auditInfo.Exception.InnerException.StackTrace;
                }
            }

            try
            {
                var auditLoggingTopic = _kafkaConfig.Topic;
                var flushTimeout = TimeSpan.FromSeconds(_kafkaConfig.FlushTimeout);

                using (var producer = new ProducerBuilder<Null, AuditInfoMqMessage>(_config).SetValueSerializer(new JsonUtf8Serializer<AuditInfoMqMessage>()).Build())
                {
                    await producer
                        .ProduceAsync(auditLoggingTopic, new Message<Null, AuditInfoMqMessage>() { Value = m })
                        .ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                            {
                                Logger.Error($"error producing message: {task.Exception?.Message}");
                            }
                            else
                            {
                                Logger.Debug($"produced to: {task.Result.TopicPartitionOffset}");
                            }


                        });

                    producer.Flush(flushTimeout);
                }
            }
            catch (Exception e)
            {
                Logger.Warn("AuditLoggingKafkaProducer error producing message", e);
            }


        }
    }

    public class JsonUtf8Serializer<T> : ISerializer<T> where T : class
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            var jsonData = JsonConvert.SerializeObject(data);

            return Encoding.UTF8.GetBytes(jsonData);
        }
    }

    public class JsonUtf8Deserializer<T> : IDeserializer<T> where T : new()
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            var jsonData = Encoding.UTF8.GetString(data.ToArray());

            return JsonConvert.DeserializeObject<T>(jsonData);
        }
    }
}