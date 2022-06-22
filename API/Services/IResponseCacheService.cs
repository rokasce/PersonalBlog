namespace API.Services;

public interface IResponseCacheService
{
   Task CacheResponseAsync(string key, object? response, TimeSpan timeToLive);
   Task<string?> GetCachedResponseAsync(string cacheKey);
}