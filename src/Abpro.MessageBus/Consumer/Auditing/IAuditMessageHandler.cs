namespace Abpro.MessageBus.Consumer.Auditing
{
    public interface IAuditMessageHandler
    {
        void Handle(RebusAuditMessage message);
    }
}