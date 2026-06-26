using MediatR;
using SimagarOrder.Application.Basket.Events;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.Basket.Commands;

public class ClearBasketCommandHandler(IBasketRepository basketRepository, IMediator mediator) 
    : IRequestHandler<ClearBasketCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
    {
        //اینجا میشد فقط خود سبد رو پاک کرد و منطقی روی مقداریش نداشت 
        var basket = await basketRepository.GetBasketWithAllItemsByUserIdAsync(request.UserId);
        if (basket is null)
        {
            return ServiceResult.NotFound("basket is not found");
        }
        basket.ClearBasket();
        await mediator.Send(new RestoreCacheBasketEvent(request.UserId), cancellationToken);
        return ServiceResult.Success();
    }
}
