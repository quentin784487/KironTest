using KironTest.Caching.Contracts;
using System.Runtime.Caching;

namespace KironTest.Caching
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly ObjectCache _cache;

        public MemoryCacheService()
        {
            _cache = MemoryCache.Default;
        }

        public T Get<T>(string key)
        {
            return _cache[key] is T value ? value : default;
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.Add(expiration) };
            _cache.Set(key, value, policy);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
