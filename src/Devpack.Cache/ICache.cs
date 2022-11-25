namespace Devpack.Cache
{
    public interface ICache
    {
        TResult GetFromMemory<TResult>(string cacheKey, Func<TResult> func);
        TResult GetFromMemory<TResult>(string cacheKey, TimeSpan lifetime, Func<TResult> func);
        Task<TResult> GetFromMemoryAsync<TResult>(string cacheKey, Func<Task<TResult>> func);
        Task<TResult> GetFromMemoryAsync<TResult>(string cacheKey, TimeSpan lifetime, Func<Task<TResult>> func);
    }
}