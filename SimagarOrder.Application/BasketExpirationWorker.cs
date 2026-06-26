using MediatR;
using Microsoft.Extensions.Hosting;
using SimagarOrder.Application.Basket.Commands;

namespace SimagarOrder.Application;

/// <summary>
/// سرویس پس‌زمینه برای بررسی دوره‌ای سبدهای خرید و منقضی کردن سبدهایی
/// که زمان اعتبار آن‌ها به پایان رسیده است.
/// </summary>
public class BasketExpirationWorker(IMediator mediator) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // تا زمانی که برنامه در حال اجرا باشد، این حلقه ادامه خواهد داشت.
        while (!stoppingToken.IsCancellationRequested)
        {
            // اجرای Command مربوط به منقضی کردن سبدهای خرید
            await mediator.Send(new ExpireBasketsCommand());

            // یک دقیقه منتظر مانده و سپس مجدداً عملیات را تکرار می‌کند.
            await Task.Delay(
                TimeSpan.FromMinutes(1),
                stoppingToken);
        }
    }
}