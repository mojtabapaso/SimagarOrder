using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Application.Basket.Commands;

public record ExpireBasketsCommand : ICommand<ServiceResult>;

