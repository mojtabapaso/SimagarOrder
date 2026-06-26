using MediatR;
using SimagarOrder.Application.Basket.Events;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.Basket.Commands;

/// <summary>
/// هندلر مربوط به پاک کردن سبد خرید کاربر
/// </summary>
public class ClearBasketCommandHandler(
    IBasketRepository basketRepository,
    IMediator mediator)
    : IRequestHandler<ClearBasketCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        ClearBasketCommand request,
        CancellationToken cancellationToken)
    {
        // دریافت سبد خرید کاربر به همراه تمامی آیتم‌های آن
        // برای حذف آیتم‌ها باید آن‌ها نیز بارگذاری شده باشند.
        var basket = await basketRepository
            .GetBasketWithAllItemsByUserIdAsync(request.UserId);

        // در صورت نبودن سبد خرید، نتیجه NotFound برگردانده می‌شود.
        if (basket is null)
        {
            return ServiceResult.NotFound("basket is not found");
        }

        // پاک کردن تمامی آیتم‌های سبد خرید.
        // از آنجایی که منطق حذف آیتم‌ها در Domain قرار دارد،
        // این عملیات از طریق متد دامنه انجام می‌شود.
        basket.ClearBasket();

        // بروزرسانی کش سبد خرید پس از اعمال تغییرات
        await mediator.Send(
            new RestoreCacheBasketEvent(request.UserId),
            cancellationToken);

        // بازگرداندن نتیجه موفق عملیات
        return ServiceResult.Success();
    }
}