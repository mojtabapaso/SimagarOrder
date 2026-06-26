using MediatR;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.Basket.Events;

/// <summary>
/// هندلر مربوط به رویداد افزودن کالا به سبد خرید
/// </summary>
public class BasketItemAddedEventHandler(IBasketRepository basketRepository)
    : IRequestHandler<BasketItemAddedEvent>
{
    public async Task Handle(
        BasketItemAddedEvent request,
        CancellationToken cancellationToken)
    {
        // دریافت سبد خرید بر اساس شناسه
        var basket = await basketRepository.FindByIdAsync(request.basketId);

        // بروزرسانی زمان آخرین تغییر سبد خرید
        // این مقدار برای تشخیص سبدهای منقضی و مدیریت آن‌ها استفاده می‌شود.
        basket.MarkAsUpdated();
    }
}