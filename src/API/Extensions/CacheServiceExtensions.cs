using API.Cache;
using API.Services;

namespace API.Extensions;

public static class CacheServiceExtensions
{
    public static void AddCacheServices(this IServiceCollection services, IConfiguration configuration)
    {
        var redisCacheSettings = new RedisCacheSettings();
        configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
        services.AddSingleton(redisCacheSettings);

        if (!redisCacheSettings.Enabled)
        {
            return;
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisCacheSettings.ConnectionString;
        });

        services.AddSingleton<IResponseCacheService, ResponseCacheService>();
    }
}