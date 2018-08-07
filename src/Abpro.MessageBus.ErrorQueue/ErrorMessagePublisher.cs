using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Rebus.Bus;
using Rebus.Messages;

namespace Abpro.MessageBus.ErrorQueue
{
    public class ErrorMessagePublisher : IErrorMessagePublisher
    {
        private readonly IBus _bus;

        public ErrorMessagePublisher(IBus bus)
        {
            _bus = bus;
        }


        public async Task Republish(ErrorMessage message)
        {
            var dic = new Dictionary<string, string>
            {
                {Headers.MessageId, message.MessageId},
                {Headers.SourceQueue, message.MessageQueue},
                {Headers.Type, message.MessageType},
                {Headers.SentTime, DateTime.SpecifyKind(message.SentTime, DateTimeKind.Local).ToString("O")},
                {Headers.ContentType, message.ContentType},
                {Headers.ReturnAddress, message.MessageQueue},
                {Headers.Intent, Headers.IntentOptions.PointToPoint},
                {Headers.CorrelationId, message.MessageId},
                {Headers.CorrelationSequence, "0"}
            };
            var t = new TransportMessage(dic, Encoding.UTF8.GetBytes(message.Body));

            var rebus = GetPrivateField<RebusBus>(_bus, "_innerBus");
            await CallPrivateMethod<Task>(rebus, "SendTransportMessage", message.MessageQueue, t);
        }

        private static T GetPrivateField<T>(object instance, string fieldname)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo field = type.GetField(fieldname, flag);
            return (T)field.GetValue(instance);
        }

        private static T CallPrivateMethod<T>(object instance, string name, params object[] param)
        {
            BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            MethodInfo method = type.GetMethod(name, flag);
            return (T)method.Invoke(instance, param);
        }
    }
}