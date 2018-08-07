using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Abpro.MessageBus.Idempotents
{
    /// <summary>
    /// �ݵ�Э����
    /// </summary>
    public interface IIdempotentCoordinator : IIdempotentProcessHandler
    {
        /// <summary>
        /// ��ʼ�����ݵȲ���
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IIdempotentProcessHandler Begin(string ticketId, IdempotentOptions options = null);

        /// <summary>
        /// �ͷ�Ʊ��
        /// </summary>
        /// <param name="ticketId"></param>
        void Release(string ticketId);


    }

    public class InMemoryIdempotentCoordinator : IIdempotentCoordinator
    {
        public static readonly InMemoryIdempotentCoordinator InMemoryInstance = new InMemoryIdempotentCoordinator();

        private IdempotentOptions _options;

        private readonly ConcurrentDictionary<string, string> IdempontentKeys = new ConcurrentDictionary<string, string>();

        private string _ticketId;

        public IIdempotentProcessHandler Begin(string ticketId, IdempotentOptions options = null)
        {
            _options = options;
            _ticketId = ticketId;
            return this;
        }

        public void Dispose()
        {
        }

        public Task<bool> HasProcessedAsync()
        {
            var isProccessed = IdempontentKeys.ContainsKey(_ticketId);

            return Task.FromResult(isProccessed);
        }

        public Task ProcessedAsync()
        {
            IdempontentKeys.GetOrAdd(_ticketId, "ok");
            return Task.CompletedTask;
        }

        public void Release(string ticketId)
        {
            IdempontentKeys.TryRemove(ticketId, out string ticket);
        }
    }
}