using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Infrastructure.Services.Redis;

public interface IBasketCacheService :IScopedDependency
{
    Task SetAsync<T>(string key, T value, TimeSpan expiry);
    Task<T?> GetAsync<T>(string key);
    Task RemoveAsync(string key);
}
