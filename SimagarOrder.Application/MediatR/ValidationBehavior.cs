using FluentValidation;
using MediatR;
using FluentValidationException = FluentValidation.ValidationException;

namespace SimagarOrder.Application.MediatR;

/// <summary>
/// Pipeline Behavior مسئول اعتبارسنجی درخواست‌ها قبل از اجرای Handler.
/// در صورت وجود خطای اعتبارسنجی، درخواست اجرا نخواهد شد.
/// </summary>
public class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // اگر Validatorی برای این درخواست ثبت نشده باشد،
        // مستقیماً Handler اصلی اجرا می‌شود.
        if (_validators.Any())
        {
            // ایجاد Context موردنیاز FluentValidation
            var context = new ValidationContext<TRequest>(request);

            // اجرای تمامی Validatorهای ثبت شده به صورت همزمان
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // جمع‌آوری تمامی خطاهای اعتبارسنجی
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // در صورت وجود خطا، اجرای Handler متوقف شده
            // و Exception مربوط به اعتبارسنجی پرتاب می‌شود.
            if (failures.Count != 0)
            {
                throw new FluentValidationException(failures);
            }
        }

        // در صورت معتبر بودن درخواست، اجرای Handler اصلی
        return await next();
    }
}