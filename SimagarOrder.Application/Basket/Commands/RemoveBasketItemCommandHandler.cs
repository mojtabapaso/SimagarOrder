using MediatR;
using SimagarOrder.Application.Basket.Events;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.Basket.Commands;

public class RemoveBasketItemCommandHandler(IBasketRepository basketRepository, IMediator mediator)
    : IRequestHandler<RemoveBasketItemCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(RemoveBasketItemCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetBasketWithAllItemsByUserIdAsync(request.UserId);
        if (basket is null)
        {
            return ServiceResult.NotFound("basket not found");
        }
        basket.RemoveItem(request.ProductId);
        await mediator.Send(new RestoreCacheBasketEvent(request.UserId), cancellationToken);
        return ServiceResult.Success();
    }
}
