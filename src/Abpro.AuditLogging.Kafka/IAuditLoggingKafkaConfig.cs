using System;
using Abp.Configuration.Startup;

namespace Abpro.AuditLogging.Kafka
{
    public interface IAuditLoggingKafkaConfig
    {
        string BootstrapServers { get; set; }

        string Topic { get; set; }

        int FlushTimeout { get; set; }

    }

    public class AuditLoggingKafkaConfig : IAuditLoggingKafkaConfig
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; } = "api-audit-log";
        public int FlushTimeout { get; set; } = 30;
    }


    public static class AuditLoggingKafkaConfigExtensions
    {
        public static IAuditLoggingKafkaConfig UseAuditLoggingKafkaProducer(this IModuleConfigurations configuration)
        {
            return configuration.AbpConfiguration.GetOrCreate("Modules.Abp.AuditLoggingKafkaProducer", () => configuration.AbpConfiguration.IocManager.Resolve<IAuditLoggingKafkaConfig>());
        }

        public static IAuditLoggingKafkaConfig SetBootstrapServers(this IAuditLoggingKafkaConfig config, string bootstrapServers)
        {
            config.BootstrapServers = bootstrapServers;

            return config;
        }

        public static IAuditLoggingKafkaConfig SetKafkaTopic(this IAuditLoggingKafkaConfig config, string topic)
        {
            config.Topic = topic;

            return config;
        }

        public static IAuditLoggingKafkaConfig SetFlushTimeout(this IAuditLoggingKafkaConfig config, int seconds)
        {
            config.FlushTimeout = seconds;

            return config;
        }
    }

}