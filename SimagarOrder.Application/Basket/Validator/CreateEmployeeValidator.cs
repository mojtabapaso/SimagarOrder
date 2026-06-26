using FluentValidation;
using SimagarOrder.Application.Basket.Commands;

namespace SimagarOrder.Application.Basket.Validator;

/// <summary>
/// اعتبارسنجی اطلاعات مربوط به افزودن کالا به سبد خرید
/// </summary>
public class AddBasketItemValidator : AbstractValidator<AddItemToBasketCommand>
{
    public AddBasketItemValidator()
    {
        // شناسه محصول باید معتبر و بزرگ‌تر از صفر باشد.
        RuleFor(x => x.ProductId)
            .NotNull()
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Product Id can't 0 ");

        // تعداد کالا باید مشخص شده و حداکثر 10 عدد باشد.
        RuleFor(x => x.Quantity)
            .NotNull()
            .NotEmpty()
            .LessThanOrEqualTo(10);
    }
}