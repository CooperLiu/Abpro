# Abpro

This project is a toolset based on abp framework.

# Modules

- Abpro.MessageBus

  使用Rebus扩展`EventBus`，提供分布式消息机制。消息队列为RabbitMq。 基于Abp 1.4.2（Framework） 
  
- Abpro.MessageBus.ErrorQueue
  
  消费Rebus错误队列，提供message republish 能力。基于Abp 1.4.2（Framework）

- Abpro.MessageBus.ApiAuditing

  将Abp接口审计日志，使用RabbitMq异步持久化。持久化机制可以使用ELK技术栈进行日志分析。基于Abp 1.4.2（Framework）
  
 - Abpro.AuditLogging.Kafka 
  将Abp接口审计日志，使用kafka异步持久化。基于Abp 1.5.0（Framework）
  
