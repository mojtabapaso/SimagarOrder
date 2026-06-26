using FluentValidation;
using SimagarOrder.Application.Basket.Commands;

namespace SimagarOrder.Application.Basket.Validator;

public class AddBasketItemValidator : AbstractValidator<AddItemToBasketCommand>
{
    public AddBasketItemValidator()
    {
        RuleFor(x => x.ProductId).NotNull().NotEmpty().GreaterThan(0).WithMessage("Product Id can't 0 ");
        RuleFor(x => x.Quantity).NotNull().NotEmpty().LessThanOrEqualTo(10);
    }
}