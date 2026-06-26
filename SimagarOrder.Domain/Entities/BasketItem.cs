namespace SimagarOrder.Domain.Entities;

/// <summary>
/// موجودیت آیتم سبد خرید
/// </summary>
public class BasketItem
{
    public BasketItem()
    {
    }

    public BasketItem(long productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public long Id { get; private set; }

    public long ProductId { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public long BasketId { get; private set; }

    public Basket Basket { get; private set; } = null!;

    /// <summary>
    /// افزایش تعداد کالا
    /// </summary>
    public void IncreaseQuantity(int quantity)
    {
        Quantity += quantity;
    }

    /// <summary>
    /// تعیین تعداد جدید برای کالا
    /// </summary>
    public void SetQuantity(int quantity)
    {
        Quantity = quantity;
    }
}