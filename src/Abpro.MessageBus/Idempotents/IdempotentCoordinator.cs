using System;
using System.Threading.Tasks;
using Abp;
using Abp.Extensions;

namespace Abpro.MessageBus.Idempotents
{
    /// <summary>
    /// 幂等协调器
    /// </summary>
    public class IdempotentCoordinator : IIdempotentCoordinator
    {
        private readonly IIdempotentKeyStore _idempotentKeyStore;

        private string _ticketId;

        private volatile bool _isProcessCalled;
        private volatile bool _isDisposed;

        private IdempotentOptions _options;

        private const string DidNotCallProcessedMethodExceptionMessage = "Did not call Processed method of a idempotent coordinator.";

        /// <summary>
        /// 互斥变量不能与当前锁定变量相同
        /// </summary>
        private const string MutexTicketShouldNotBeSameWithCurrentTicket = "Mutex Ticket Should Not Be Same With Current Ticket";


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="idempotentKeyStore"></param>
        public IdempotentCoordinator(IIdempotentKeyStore idempotentKeyStore)
        {
            _idempotentKeyStore = idempotentKeyStore;
        }


        /// <summary>
        /// 开始处理幂等操作
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public IIdempotentProcessHandler Begin(string ticketId, IdempotentOptions options = null)
        {
            _options = options ?? IdempotentOptions.DefaultOptions;
            this._ticketId = ticketId;
            return this;
        }


        /// <summary>
        /// 释放票据
        /// </summary>
        /// <param name="ticketId"></param>
        public void Release(string ticketId)
        {
            Check.NotNullOrWhiteSpace(ticketId, nameof(ticketId));

            _idempotentKeyStore.Remove(ticketId);

        }

        /// <summary>
        /// 是否已完成
        /// </summary>
        /// <returns></returns>
        public async Task<bool> HasProcessedAsync()
        {
            var t = await _idempotentKeyStore.GetAsync(_ticketId);

            this._isProcessCalled = !t.IsNullOrEmpty();

            return !t.IsNullOrEmpty();
        }



        /// <summary>
        /// 完成处理
        /// </summary>
        /// <returns></returns>
        public async Task ProcessedAsync()
        {
           await _idempotentKeyStore.SetAsync(_ticketId, $"{new DateTimeOffset(DateTime.Now).ToString()},{_options.ExpireTimeInSeconds}", _options.ExpireTimeInSeconds.HasValue ? TimeSpan.FromSeconds(_options.ExpireTimeInSeconds.Value) : (TimeSpan?)null);
            
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

            if (_isDisposed)
            {
                return;
            }

            if (!_isProcessCalled)//尽量不触发异常
            {
                return;
            }

            this._isDisposed = true;

        }

    }

}