using SimagarOrder.Domain.Enums;

namespace SimagarOrder.Domain.Entities;

/// <summary>
/// موجودیت سبد خرید
/// </summary>
public class Basket
{
    public Basket()
    {
    }

    private Basket(long userId)
    {
        UserId = userId;
        Status = BasketStatus.Active;
        CreatedAt = DateTime.Now;
        Items = new List<BasketItem>();
    }

    public long Id { get; private set; }

    public long UserId { get; private set; }

    public BasketStatus Status { get; private set; }

    public List<BasketItem> Items { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? LastUpdatedAt { get; private set; }

    /// <summary>
    /// ایجاد یک سبد خرید جدید برای کاربر
    /// </summary>
    public Basket CreateBasket(long userId)
    {
        return new Basket(userId);
    }

    /// <summary>
    /// افزودن کالا به سبد خرید
    /// </summary>
    public void AddItem(long productId, int quantity, decimal unitPrice = 0)
    {
        var existingItem = Items
            .FirstOrDefault(x => x.ProductId == productId);

        if (existingItem is null)
        {
            Items.Add(new BasketItem(productId, quantity, unitPrice));
        }
        else
        {
            // تعداد هر کالا در سبد نباید از 10 عدد بیشتر شود.
            if (existingItem.Quantity + quantity > 10)
            {
                throw new DomainException("حداکثر تعداد هر کالا 10 عدد است.");
            }

            existingItem.IncreaseQuantity(quantity);
        }

        // بررسی قوانین مربوط به مجموع مبلغ سبد
        ValidateTotalPrice();
    }

    /// <summary>
    /// بروزرسانی زمان آخرین تغییر سبد خرید
    /// </summary>
    public void MarkAsUpdated()
    {
        LastUpdatedAt = DateTime.Now;
    }

    /// <summary>
    /// تغییر وضعیت سبد به منقضی شده
    /// </summary>
    public void Expire()
    {
        Status = BasketStatus.Expired;
    }

    /// <summary>
    /// بروزرسانی تعداد یک کالا در سبد خرید
    /// </summary>
    public void UpdateQuantity(long productId, int newQuantity)
    {
        // تعداد هر کالا در سبد نباید از 10 عدد بیشتر شود.
        if (newQuantity > 10)
            throw new DomainException("حداکثر تعداد هر کالا 10 عدد است.");

        var item = Items.FirstOrDefault(x => x.ProductId == productId);

        if (item is null)
            throw new DomainException("کالا یافت نشد.");

        item.SetQuantity(newQuantity);

        // بررسی قوانین مربوط به مجموع مبلغ سبد
        ValidateTotalPrice();
    }

    /// <summary>
    /// حذف یک کالا از سبد خرید
    /// </summary>
    public void RemoveItem(long productId)
    {
        var item = Items.FirstOrDefault(x => x.ProductId == productId);

        if (item is not null)
            Items.Remove(item);
    }

    /// <summary>
    /// بررسی قوانین مربوط به مجموع مبلغ سبد خرید
    /// </summary>
    private void ValidateTotalPrice()
    {
        var totalPrice = Items.Sum(x => x.Quantity * x.UnitPrice);

        // مجموع مبلغ سبد نباید از سقف مجاز بیشتر شود.
        if (totalPrice > 50_000_000)
            throw new DomainException("مبلغ سبد از حد مجاز بیشتر است.");
    }

    /// <summary>
    /// تغییر تعداد یک کالا در سبد خرید
    /// </summary>
    public void NewQuantity(long productId, int newQuantity)
    {
        var item = Items.FirstOrDefault(x => x.ProductId == productId);

        if (item is not null)
            item.SetQuantity(newQuantity);
    }

    /// <summary>
    /// حذف تمامی کالاهای سبد خرید
    /// </summary>
    public void ClearBasket()
    {
        // فقط آیتم‌های سبد حذف می‌شوند و خود سبد باقی می‌ماند.
        // این متد محل مناسبی برای افزودن منطق‌های احتمالی آینده نیز خواهد بود.
        Items.Clear();
    }
}