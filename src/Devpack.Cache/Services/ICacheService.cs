namespace Devpack.Cache.Services.MemoryCache
{
    public interface ICacheService
    {
        bool Get<TResult>(string key, out TResult cachedData);
        void Save(string key, object? data);
        void Save(string key, object? data, TimeSpan lifetime);
    }
}