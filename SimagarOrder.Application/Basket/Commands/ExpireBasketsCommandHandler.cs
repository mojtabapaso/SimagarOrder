using MediatR;
using Microsoft.Extensions.Logging;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.Basket.Commands;

/// <summary>
/// هندلر مربوط به منقضی کردن سبدهای خریدی که زمان اعتبار آن‌ها به پایان رسیده است.
/// </summary>
public class ExpireBasketsCommandHandler(
    IBasketRepository basketRepository,
    ILogger<ExpireBasketsCommandHandler> logger)
    : IRequestHandler<ExpireBasketsCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        ExpireBasketsCommand request,
        CancellationToken cancellationToken)
    {
        // دریافت تمامی سبدهای خریدی که زمان اعتبار آن‌ها به پایان رسیده است.
        var baskets = await basketRepository.GetExpiredBasketsAsync();

        // تغییر وضعیت هر سبد به حالت منقضی از طریق منطق دامنه
        baskets.ForEach(x => x.Expire());

        // ثبت لاگ جهت اطلاع از اجرای موفق عملیات
        logger.LogInformation("baskets expired");

        // بازگرداندن نتیجه موفق عملیات
        return ServiceResult.Success();
    }
}