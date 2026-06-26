using MediatR;
using SimagarOrder.Application.Basket.Events;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;
using BasketEntity = SimagarOrder.Domain.Entities.Basket;
namespace SimagarOrder.Application.Basket.Commands;

public class AddItemToBasketCommandHandler(IBasketRepository basketRepository,IMediator mediator ,IUnitOfWork unitOfWork) 
    : IRequestHandler<AddItemToBasketCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(AddItemToBasketCommand request, CancellationToken cancellationToken)
    {
        var basket = await basketRepository.GetActiveBasketWithItemsByUserId(request.UserId);
        if (basket is null)
        {
            basket = new BasketEntity();
            basket = basket.CreateBasket(request.UserId);
            basket.AddItem(request.ProductId, request.Quantity, request.UunitPrice);
            await basketRepository.AddAsync(basket);
        }
        else
        {
            basket.AddItem(request.ProductId, request.Quantity, request.UunitPrice);
            basketRepository.Update(basket);
        }
        // چون آیدی توی دیتابیس میخوره و نیاز بهش داریم نمیشه دیگه منتظر تراکنش بعدش مونده همنجای میزنیم تا ثبت بشه و آدیش رو رو بگیریم 
        await unitOfWork.SaveChangesAsync(cancellationToken);

        long basketId = await basketRepository.GetActiveBasketIdByUserId(request.UserId);
        await mediator.Send(new BasketItemAddedEvent(basketId), cancellationToken);
        await mediator.Send(new RestoreCacheBasketEvent(request.UserId), cancellationToken);
        return ServiceResult.Success();
        
    }
}

