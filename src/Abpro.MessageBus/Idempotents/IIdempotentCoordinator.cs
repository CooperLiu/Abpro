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
}