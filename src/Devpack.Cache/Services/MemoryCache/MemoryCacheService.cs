using Microsoft.Extensions.Caching.Memory;

namespace Devpack.Cache.Services.MemoryCache
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheDefaultTmeout = TimeSpan.FromHours(1);

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool Get<TResult>(string key, out TResult cachedData)
        {
            var hasInCache = _memoryCache.TryGetValue(key, out var output);
            cachedData = output != default ? (TResult)output! : default!;
                
            return hasInCache;
        }

        public void Save(string key, object? data)
        {
            Save(key, data, _cacheDefaultTmeout);
        }

        public void Save(string key, object? data, TimeSpan lifetime)
        {
            _memoryCache.Set(key, data, lifetime);
        }
    }
}