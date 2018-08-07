using System.Threading.Tasks;

namespace Abpro.MessageBus.ErrorQueue
{
    public interface IErrorMessagePublisher
    {
        Task Republish(ErrorMessage message);
    }
}