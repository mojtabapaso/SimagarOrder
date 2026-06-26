using SimagarOrder.Application.Basket.DTOs;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.MarkerInterfaces;

namespace SimagarOrder.Application.Basket.Queries;

public record GetOrCreateBasketQuery(long userId) : IQuery<ServiceResult<BasketDTO>>;
