namespace Abpro.MessageBus.ErrorQueue
{
    public interface IRebusErrorQueueBootstrapper
    {
        /// <summary>
        /// 启动
        /// </summary>
        void Start();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();
    }
}