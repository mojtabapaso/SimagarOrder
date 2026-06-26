using MassTransit;
using SimagarOrder.Infrastructure.MarkerInterfaces;
namespace SimagarOrder.Infrastructure.Services.MessageBus;

public class BasketEventPublisher: IBasketEventPublisher
{
    private readonly IPublishEndpoint publishEndpoint;

    public BasketEventPublisher(IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }
    public async Task PublicAsync<TEvent>(TEvent @event) where TEvent : IEventDomain
    {
        await publishEndpoint.Publish(@event);

    }
}
