using StackExchange.Redis;
using System.Text.Json;

namespace SimagarOrder.Infrastructure.Services.Redis;

public class BasketCacheService : IBasketCacheService
{
    private readonly IDatabase _db;

    public BasketCacheService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiry)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json, expiry);
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;
        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task RemoveAsync(string key)
    {
        var value = await _db.StringGetAsync(key);
        if (value.IsNullOrEmpty) 
            return;
        await _db.KeyDeleteAsync(key);
    }
}
