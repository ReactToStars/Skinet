namespace Core.Interfaces
{
    public interface IResponseCacheService
    {
        Task CacheResponceAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string cacheKey);
    }
}