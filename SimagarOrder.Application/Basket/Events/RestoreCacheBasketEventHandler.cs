using MediatR;
using SimagarOrder.Infrastructure.Services.Redis;

namespace SimagarOrder.Application.Basket.Events;

/// <summary>
/// هندلر مربوط به بروزرسانی کش سبد خرید
/// </summary>
public class RestoreCacheBasketEventHandler : IRequestHandler<RestoreCacheBasketEvent>
{
    private readonly IBasketCacheService basketCacheService;

    public RestoreCacheBasketEventHandler(
        IBasketCacheService basketCacheService)
    {
        this.basketCacheService = basketCacheService;
    }

    public async Task Handle(
        RestoreCacheBasketEvent request,
        CancellationToken cancellationToken)
    {
        // حذف سبد خرید از کش.
        // در درخواست بعدی، اطلاعات جدید از دیتابیس خوانده شده
        // و مجدداً در کش ذخیره خواهند شد.
        await basketCacheService.RemoveAsync(
            "basket" + request.userId.ToString());
    }
}