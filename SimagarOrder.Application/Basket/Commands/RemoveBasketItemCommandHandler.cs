using MediatR;
using SimagarOrder.Application.Basket.Events;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.Basket.Commands;

/// <summary>
/// هندلر مربوط به حذف یک آیتم از سبد خرید
/// </summary>
public class RemoveBasketItemCommandHandler(
    IBasketRepository basketRepository,
    IMediator mediator)
    : IRequestHandler<RemoveBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        RemoveBasketItemCommand request,
        CancellationToken cancellationToken)
    {
        // دریافت سبد خرید کاربر به همراه تمامی آیتم‌های آن
        var basket = await basketRepository
            .GetBasketWithAllItemsByUserIdAsync(request.UserId);

        // در صورت نبودن سبد خرید، نتیجه NotFound برگردانده می‌شود.
        if (basket is null)
        {
            return ServiceResult.NotFound("basket not found");
        }

        // حذف کالای موردنظر از سبد خرید از طریق منطق دامنه
        basket.RemoveItem(request.ProductId);

        // بروزرسانی کش سبد خرید پس از اعمال تغییرات
        await mediator.Send(
            new RestoreCacheBasketEvent(request.UserId),
            cancellationToken);

        // بازگرداندن نتیجه موفق عملیات
        return ServiceResult.Success();
    }
}