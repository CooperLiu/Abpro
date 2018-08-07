using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rebus.Extensions;
using Rebus.Messages;
using Rebus.Serialization;

namespace Abpro.MessageBus.ErrorQueue
{
    public class ErrorMessageSerializer : ISerializer
    {
        public static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public Task<TransportMessage> Serialize(Message message)
        {
            var jsonText = JsonConvert.SerializeObject(message.Body,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            var bytes = Encoding.UTF8.GetBytes(jsonText);
            var headers = message.Headers.Clone();

            return Task.FromResult(new TransportMessage(headers, bytes));
        }

        public Task<Message> Deserialize(TransportMessage transportMessage)
        {
            var message = new ErrorMessage
            {
                MessageType = transportMessage.Headers.GetValueOrNull(Headers.Type) ?? "",
                MessageId = transportMessage.Headers.GetValue(Headers.MessageId),
                ErrorDetail = transportMessage.Headers.GetValueOrNull(Headers.ErrorDetails) ?? "",
                Body = Encoding.UTF8.GetString(transportMessage.Body),
                MessageQueue = transportMessage.Headers.GetValueOrNull(Headers.SourceQueue) ?? transportMessage.Headers.GetValueOrNull(Headers.ReturnAddress) ?? "",
                SentTime = DateTime.Parse(transportMessage.Headers.GetValueOrNull(Headers.SentTime) ?? DateTime.Now.ToString()),
                ContentType = transportMessage.Headers.GetValueOrNull(Headers.ContentType) ?? "",
                Intent = transportMessage.Headers.GetValueOrNull(Headers.Intent) ?? ""
            };

            return Task.FromResult(new Message(transportMessage.Headers.Clone(), message));
        }
    }
}