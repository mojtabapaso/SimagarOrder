using MediatR;
using SimagarOrder.Application.Basket.DTOs;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;
using SimagarOrder.Infrastructure.Services.Redis;

namespace SimagarOrder.Application.Basket.Queries;

public class GetOrCreateBasketQueryHandler(IBasketRepository basketRepository,
    IBasketCacheService basketCacheService)
    : IRequestHandler<GetOrCreateBasketQuery, ServiceResult<BasketDTO>>
{

    public async Task<ServiceResult<BasketDTO>> Handle(GetOrCreateBasketQuery request,CancellationToken cancellationToken)
    {
        var cacheKey = $"basket{request.userId}";

        var basketDto = await basketCacheService.GetAsync<BasketDTO>(cacheKey);

        if (basketDto is null)
        {
            
            var basket = await basketRepository.GetBasketWithAllItemsByUserIdAsync(request.userId);

            if (basket is null)
            {

                basket = new Domain.Entities.Basket();
                basket.CreateBasket(request.userId);
                await basketRepository.AddAsync(basket);
            }

            basketDto = new BasketDTO
            {
                basketItemsDTOs = basket.Items?
                    .Select(item => new BasketItemsDTO
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    })
                    .ToList()
            };

            await basketCacheService.SetAsync(
                cacheKey,
                basketDto,
                TimeSpan.FromMinutes(5));
        }

        return  ServiceResult<BasketDTO>.Success(basketDto);
    }
}
