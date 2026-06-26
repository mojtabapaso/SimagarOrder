using MediatR;
using SimagarOrder.Application.Basket.Events;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.Basket.Commands;

/// <summary>
/// هندلر مربوط به بروزرسانی تعداد یک آیتم در سبد خرید
/// </summary>
public class UpdateBasketItemQuantityCommandHandler(
    IBasketRepository basketRepository,
    IMediator mediator)
    : IRequestHandler<UpdateBasketItemQuantityCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        UpdateBasketItemQuantityCommand request,
        CancellationToken cancellationToken)
    {
        // دریافت سبد خرید فعال کاربر به همراه آیتم‌های آن
        var basket = await basketRepository
            .GetActiveBasketWithItemsByUserId(request.UserId);

        // در صورت وجود سبد خرید، تعداد کالای موردنظر بروزرسانی می‌شود.
        if (basket is not null)
        {
            basket.UpdateQuantity(
                request.ProductId,
                request.NewQuantity);
        }

        // بروزرسانی کش سبد خرید پس از اعمال تغییرات
        await mediator.Send(
            new RestoreCacheBasketEvent(request.UserId),
            cancellationToken);

        // بازگرداندن نتیجه موفق عملیات
        return ServiceResult.Success();
    }
}