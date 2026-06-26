using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Application.Basket.Commands;

public record ClearBasketCommand(long UserId) : ICommand<ServiceResult>;
