using MassTransit;
using SimagarOrder.Infrastructure.Persistence.Repositories;
//using SimagarOrder.Application.Basket.Events;

namespace SimagarOrder.Infrastructure.Services.MessageBus;

public class BasketEventConsumer : IConsumer //<BasketItemAddedEvent>
{
    private readonly IBasketRepository basketRepository;
    private readonly IUnitOfWork unitOfWork;

    public BasketEventConsumer(IBasketRepository basketRepository,IUnitOfWork unitOfWork)
    {
        this.basketRepository = basketRepository;
        this.unitOfWork = unitOfWork;
    }
    public async Task Consume(ConsumeContext context)//<BasketItemAddedEvent> context)
    {
        //var basket = await basketRepository.FindByIdAsync(context.Message.BasketId);
        //basket.MarkAsUpdated();
        //basketRepository.Update(basket);
        //await unitOfWork.SaveChangesAsync();
    }
}