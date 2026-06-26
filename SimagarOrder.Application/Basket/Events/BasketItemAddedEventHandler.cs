using MediatR;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.Basket.Events;

public class BasketItemAddedEventHandler(IBasketRepository basketRepository) : IRequestHandler<BasketItemAddedEvent>
{
    public async Task Handle(BasketItemAddedEvent request, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.FindByIdAsync(request.basketId);
        basket.MarkAsUpdated();
    }
}
