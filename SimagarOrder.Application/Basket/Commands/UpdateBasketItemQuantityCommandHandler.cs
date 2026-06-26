using MediatR;
using SimagarOrder.Application.Basket.Events;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.Basket.Commands;

public class UpdateBasketItemQuantityCommandHandler(IBasketRepository basketRepository, IMediator mediator) 
    : IRequestHandler<UpdateBasketItemQuantityCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(UpdateBasketItemQuantityCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetActiveBasketWithItemsByUserId(request.UserId);
        if (basket is not null)
        {
            basket.UpdateQuantity(request.ProductId, request.NewQuantity);
        }
        await mediator.Send(new RestoreCacheBasketEvent(request.UserId), cancellationToken);
        return ServiceResult.Success();

    }
}
