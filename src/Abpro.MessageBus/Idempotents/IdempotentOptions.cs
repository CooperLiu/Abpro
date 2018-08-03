namespace Abpro.MessageBus.Idempotents
{
    /// <summary>
    /// �ݵ����������ʱ��
    /// </summary>
    public class IdempotentOptions
    {
        /// <summary>
        /// Ĭ��һ�����ʱ��
        /// </summary>
        private const int DefaultExpireTimeInSeconds = 60 * 60 * 24;

        /// <summary>
        /// Ĭ������
        /// </summary>
        public static readonly IdempotentOptions DefaultOptions = new IdempotentOptions();

        /// <summary>
        /// ��������
        /// </summary>
        public int? ExpireTimeInSeconds { get; set; }

        /// <summary>
        /// ���캯��
        /// </summary>
        public IdempotentOptions()
        {
            ExpireTimeInSeconds = null;
        }

        /// <summary>
        /// �����ݵ���Ч��
        /// </summary>
        /// <param name="expireTimeInSeconds"></param>
        public IdempotentOptions(int expireTimeInSeconds)
        {
            ExpireTimeInSeconds = expireTimeInSeconds > 0 ? expireTimeInSeconds : DefaultExpireTimeInSeconds;
        }

    }
}