using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abpro.MessageBus.Publisher;
using MessageBusMqMessage;

namespace MessageBusPublisher
{
    internal static class TestEventDataTrigger
    {
        public static void TestTheFistCaseEventDataTrigger()
        {
            EventBus.Default.Trigger(new TheFistEventData() { Count = 1, Name = "John", Age = 1 });
        }


    }

    public class TheFistEventData : EventData
    {
        public int Count { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }
    }

    public class TestEventDataPublisher : EventDataPublishHandlerBase<TheFistEventData, TheFirstMqMessage>, ITransientDependency
    {
        public TestEventDataPublisher(IUnitOfWorkManager unitOfWorkManager)
            : base(unitOfWorkManager)
        {
        }

        protected override TheFirstMqMessage ConvertEventDataToMqMessage(TheFistEventData eventData)
        {
            return new TheFirstMqMessage()
            {
                Count = eventData.Count,
                Name = eventData.Name,
                Age = eventData.Age
            };
        }
    }



}
