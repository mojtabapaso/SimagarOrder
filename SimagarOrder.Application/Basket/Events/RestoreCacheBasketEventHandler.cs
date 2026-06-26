using MediatR;
using SimagarOrder.Infrastructure.Services.Redis;

namespace SimagarOrder.Application.Basket.Events;

public class RestoreCacheBasketEventHandler : IRequestHandler<RestoreCacheBasketEvent>
{
    private readonly IBasketCacheService basketCacheService;

    public RestoreCacheBasketEventHandler(IBasketCacheService basketCacheService)
    {
        this.basketCacheService = basketCacheService;
    }
    public async Task Handle(RestoreCacheBasketEvent request, CancellationToken cancellationToken)
    {
        await basketCacheService.RemoveAsync("basket"+request.userId.ToString());
    }
}