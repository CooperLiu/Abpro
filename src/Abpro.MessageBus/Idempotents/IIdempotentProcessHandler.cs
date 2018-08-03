using System;
using System.Threading.Tasks;

namespace Abpro.MessageBus.Idempotents
{
    /// <summary>
    /// 幂等协调完成处理器
    /// </summary>
    public interface IIdempotentProcessHandler : IDisposable
    {
        /// <summary>
        /// 是否已完成
        /// </summary>
        /// <returns></returns>
        Task<bool> HasProcessedAsync();

        /// <summary>
        /// 完成处理。如果在事务环境下，确保事务提交后，完成。
        /// </summary>
        /// <returns></returns>
        Task ProcessedAsync();

    }
}