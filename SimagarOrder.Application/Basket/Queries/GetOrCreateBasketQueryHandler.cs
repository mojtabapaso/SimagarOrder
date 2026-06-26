using MediatR;
using SimagarOrder.Application.Basket.DTOs;
using SimagarOrder.Application.Common;
using SimagarOrder.Infrastructure.Persistence.Repositories;
using SimagarOrder.Infrastructure.Services.Redis;

namespace SimagarOrder.Application.Basket.Queries;

/// <summary>
/// هندلر مربوط به دریافت سبد خرید کاربر.
/// در صورت نبود سبد، یک سبد جدید ایجاد می‌شود.
/// همچنین برای کاهش دسترسی به دیتابیس از Redis استفاده می‌شود.
/// </summary>
public class GetOrCreateBasketQueryHandler(
    IBasketRepository basketRepository,
    IBasketCacheService basketCacheService)
    : IRequestHandler<GetOrCreateBasketQuery, ServiceResult<BasketDTO>>
{
    public async Task<ServiceResult<BasketDTO>> Handle(
        GetOrCreateBasketQuery request,
        CancellationToken cancellationToken)
    {
        // تولید کلید کش بر اساس شناسه کاربر
        var cacheKey = $"basket{request.userId}";

        // تلاش برای دریافت سبد خرید از Redis
        var basketDto = await basketCacheService.GetAsync<BasketDTO>(cacheKey);

        // در صورت نبود اطلاعات در کش، از دیتابیس خوانده می‌شود.
        if (basketDto is null)
        {
            // دریافت سبد خرید کاربر به همراه تمامی آیتم‌ها
            var basket = await basketRepository
                .GetBasketWithAllItemsByUserIdAsync(request.userId);

            // اگر کاربر هنوز سبد خریدی نداشته باشد، یک سبد جدید ایجاد می‌شود.
            if (basket is null)
            {
                basket = new Domain.Entities.Basket();
                basket.CreateBasket(request.userId);

                await basketRepository.AddAsync(basket);
            }

            // تبدیل موجودیت دامنه به DTO جهت ارسال به کلاینت
            basketDto = new BasketDTO
            {
                basketItemsDTOs = basket.Items?
                    .Select(item => new BasketItemsDTO
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    })
                    .ToList()
            };

            // ذخیره نتیجه در Redis برای درخواست‌های بعدی
            await basketCacheService.SetAsync(
                cacheKey,
                basketDto,
                TimeSpan.FromMinutes(5));
        }

        // بازگرداندن اطلاعات سبد خرید
        return ServiceResult<BasketDTO>.Success(basketDto);
    }
}