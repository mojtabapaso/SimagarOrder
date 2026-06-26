using MediatR;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Application.Basket.Commands;

public record UpdateBasketItemQuantityCommand(long UserId, long ProductId, int NewQuantity) : ICommand<ServiceResult>;