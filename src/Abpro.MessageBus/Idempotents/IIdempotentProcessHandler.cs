using System;
using System.Threading.Tasks;

namespace Abpro.MessageBus.Idempotents
{
    /// <summary>
    /// �ݵ�Э����ɴ�����
    /// </summary>
    public interface IIdempotentProcessHandler : IDisposable
    {
        /// <summary>
        /// �Ƿ������
        /// </summary>
        /// <returns></returns>
        Task<bool> HasProcessedAsync();

        /// <summary>
        /// ��ɴ�����������񻷾��£�ȷ�������ύ����ɡ�
        /// </summary>
        /// <returns></returns>
        Task ProcessedAsync();

    }
}