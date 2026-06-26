using MediatR;
using Microsoft.Extensions.Hosting;
using SimagarOrder.Application.Basket.Commands;

namespace SimagarOrder.Application;

public class BasketExpirationWorker(IMediator mediator) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await mediator.Send(new ExpireBasketsCommand());

            await Task.Delay(TimeSpan.FromMinutes(1),stoppingToken);
        }
    }
}
