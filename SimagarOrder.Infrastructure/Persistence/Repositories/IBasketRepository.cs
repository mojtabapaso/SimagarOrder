using SimagarOrder.Domain.Entities;
using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Infrastructure.Persistence.Repositories;

public interface IBasketRepository :IGenericServices<Basket>, IScopedDependency
{
    Task<Basket?> GetActiveBasketWithItemsByUserId(long UserId);
    Task<Basket?> GetBasketWithAllItemsByUserIdAsync(long userId);
    Task<Basket?> GetBasketByUserIdAsync(long userId);
    Task<long> GetActiveBasketIdByUserId(long userId);
    Task<List<Basket>> GetExpiredBasketsAsync();



}
