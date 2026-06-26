using MediatR;
using Microsoft.Extensions.Logging;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;

namespace SimagarOrder.Application.Basket.Commands;

public class ExpireBasketsCommandHandler(IBasketRepository basketRepository ,ILogger<ExpireBasketsCommandHandler> logger) 
    : IRequestHandler<ExpireBasketsCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(ExpireBasketsCommand request, CancellationToken cancellationToken)
    {
        var baskets = await basketRepository.GetExpiredBasketsAsync();
        baskets.ForEach(x => x.Expire());
        logger.LogInformation("baskets expired");
        return  ServiceResult.Success();
    }
}