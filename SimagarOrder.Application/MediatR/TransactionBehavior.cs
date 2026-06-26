using MediatR;
using SimagarOrder.Infrastructure.MarkerInterfaces;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.MediatR;

/// <summary>
/// Pipeline Behavior مسئول مدیریت تراکنش برای تمامی Commandها.
/// Queryها بدون تراکنش اجرا می‌شوند.
/// </summary>
public class TransactionBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // تنها Commandها داخل تراکنش اجرا می‌شوند.
        // Queryها تغییری در داده‌ها ایجاد نمی‌کنند و نیازی به تراکنش ندارند.
        if (request is not ICommand<TResponse>)
            return await next();

        try
        {
            // شروع تراکنش
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            // اجرای Handler اصلی
            var response = await next();

            // ذخیره تمامی تغییرات ایجاد شده
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // نهایی کردن تراکنش
            await unitOfWork.CommitAsync(cancellationToken);

            return response;
        }
        catch
        {
            // در صورت بروز خطا، تمامی تغییرات بازگردانده می‌شوند.
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}