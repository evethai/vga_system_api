namespace Api.Services
{
    public interface ICacheService
    {
        Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task<string> GetCacheResponseAsync<T>(string cacheKey);
        Task RemoveCacheResponseAsync(string pattern);
    }
}
