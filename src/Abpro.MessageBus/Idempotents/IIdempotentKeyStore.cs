using System;
using System.Threading.Tasks;
using Abp.Runtime.Caching;

namespace Abpro.MessageBus.Idempotents
{
    public interface IIdempotentKeyStore
    {
        string Get(string mutexTicketId);

        Task<string> GetAsync(string mutexTicketId);

        void Set(string mutexTicketId, string value, TimeSpan? timeout = null);

        Task SetAsync(string mutexTicketId, string value, TimeSpan? timeout = null);

        void Remove(string mutexTicketId);

        Task RemoveAsync(string mutexTicketId);
    }

    public class CacheIdempotentKeyStore : IIdempotentKeyStore
    {
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// 幂等Key
        /// </summary>
        private const string IdempotentKey = "IdempotentKey";

        public CacheIdempotentKeyStore(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public string Get(string mutexTicketId)
        {
            return _cacheManager
                .GetCache<string, string>(IdempotentKey)
                .GetOrDefault(mutexTicketId);
        }

        public async Task<string> GetAsync(string mutexTicketId)
        {
            return await _cacheManager
                .GetCache<string, string>(IdempotentKey)
                .GetOrDefaultAsync(mutexTicketId);
        }

        public void Set(string mutexTicketId, string value, TimeSpan? timeout = null)
        {
            _cacheManager
               .GetCache<string, string>(IdempotentKey)
                .Set(mutexTicketId, value, timeout);
        }

        public async Task SetAsync(string mutexTicketId, string value, TimeSpan? timeout = null)
        {
            await _cacheManager
                .GetCache<string, string>(IdempotentKey)
                .SetAsync(mutexTicketId, value, timeout);
        }

        public void Remove(string mutexTicketId)
        {
            _cacheManager.GetCache<string, string>(IdempotentKey).Remove(mutexTicketId);

        }

        public async Task RemoveAsync(string mutexTicketId)
        {
            await _cacheManager.GetCache<string, string>(IdempotentKey).RemoveAsync(mutexTicketId);
        }
    }


}