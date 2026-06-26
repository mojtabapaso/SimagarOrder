using MediatR;

namespace SimagarOrder.Application.Basket.Events;

public record BasketItemAddedEvent(long basketId) : IRequest;
