using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Application.Basket.Commands;

public record AddItemToBasketCommand(long UserId, long ProductId, int Quantity, decimal UunitPrice)
    : ICommand<ServiceResult>;
/*
 unitPriceke
نگفته شده بود توی تسک اش
 */