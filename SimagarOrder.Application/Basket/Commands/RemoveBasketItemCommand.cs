using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Application.Basket.Commands;

public record RemoveBasketItemCommand(long UserId, long ProductId) : ICommand<ServiceResult>;
