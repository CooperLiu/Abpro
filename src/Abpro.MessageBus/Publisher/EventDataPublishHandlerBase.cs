using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Events.Bus.Handlers;
using Castle.Core.Logging;

namespace Abpro.MessageBus.Publisher
{
    public abstract class EventDataPublishHandlerBase<TEventData, TMqMessage>
        : IEventHandler<TEventData>, ITransientDependency
        where TEventData : EventData
        where TMqMessage : class
    {
        protected readonly IUnitOfWorkManager UnitOfWorkManager;

        public ILogger Logger { get; set; }

        public IMessageBus MessageBus { get; set; }

        public EventDataPublishHandlerBase(IUnitOfWorkManager unitOfWorkManager)
        {
            UnitOfWorkManager = unitOfWorkManager;
            Logger = NullLogger.Instance;
        }

        public virtual void HandleEvent(TEventData eventData)
        {
            if (UnitOfWorkManager.Current == null)
            {
                MessageBus.Publish(ConvertEventDataToMqMessage(eventData));
            }
            else
            {
                UnitOfWorkManager.Current.Completed += (sender, e) => MessageBus.Publish(ConvertEventDataToMqMessage(eventData));
            }
        }

        /// <summary>
        /// 转换EventData为MqMessage，默认采用eventData.MapTo <see cref="TMqMessage"/>
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        protected virtual TMqMessage ConvertEventDataToMqMessage(TEventData eventData)
        {
            return eventData.MapTo<TMqMessage>();
        }
    }
}