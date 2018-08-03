namespace Abpro.MessageBus.Idempotents
{
    /// <summary>
    /// 幂等协调器
    /// </summary>
    public interface IIdempotentCoordinator : IIdempotentProcessHandler
    {
        /// <summary>
        /// 开始处理幂等操作
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        IIdempotentProcessHandler Begin(string ticketId, IdempotentOptions options = null);

        /// <summary>
        /// 释放票据
        /// </summary>
        /// <param name="ticketId"></param>
        void Release(string ticketId);


    }
}