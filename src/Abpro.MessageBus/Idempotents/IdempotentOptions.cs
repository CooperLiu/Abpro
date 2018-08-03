namespace Abpro.MessageBus.Idempotents
{
    /// <summary>
    /// 幂等配置项：过期时间
    /// </summary>
    public class IdempotentOptions
    {
        /// <summary>
        /// 默认一天过期时间
        /// </summary>
        private const int DefaultExpireTimeInSeconds = 60 * 60 * 24;

        /// <summary>
        /// 默认配置
        /// </summary>
        public static readonly IdempotentOptions DefaultOptions = new IdempotentOptions();

        /// <summary>
        /// 过期秒数
        /// </summary>
        public int? ExpireTimeInSeconds { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public IdempotentOptions()
        {
            ExpireTimeInSeconds = null;
        }

        /// <summary>
        /// 配置幂等有效期
        /// </summary>
        /// <param name="expireTimeInSeconds"></param>
        public IdempotentOptions(int expireTimeInSeconds)
        {
            ExpireTimeInSeconds = expireTimeInSeconds > 0 ? expireTimeInSeconds : DefaultExpireTimeInSeconds;
        }

    }
}