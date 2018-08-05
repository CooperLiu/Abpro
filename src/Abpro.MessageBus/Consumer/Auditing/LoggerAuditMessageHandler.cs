using Castle.Core.Logging;

namespace Abpro.MessageBus.Consumer.Auditing
{
    public class LoggerAuditMessageHandler : IAuditMessageHandler
    {
        private readonly ILogger _logger;

        public LoggerAuditMessageHandler(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.Create(typeof(LoggerAuditMessageHandler));
        }

        public void Handle(RebusAuditMessage message)
        {
            _logger.Debug($"----{nameof(LoggerAuditMessageHandler)}----- \r\n {message.ToString()}");
        }
    }
}
