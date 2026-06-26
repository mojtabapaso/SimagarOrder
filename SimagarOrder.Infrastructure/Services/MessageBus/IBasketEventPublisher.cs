using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Infrastructure.Services.MessageBus;

public interface IBasketEventPublisher: IScopedDependency
{
    //Task PublishBasketItemAddedAsync(BasketItemAddedEvent basketItemAddedEvent);
    Task PublicAsync<TEvent>(TEvent @event) where TEvent : IEventDomain;
}
