using SimagarOrder.Domain.Enums;

namespace SimagarOrder.Domain.Entities;

public class Basket
{
    public Basket()
    {

    }
    private Basket(long UserId)
    {
        this.UserId = UserId;
        this.Status = BasketStatus.Active;
        this.CreatedAt = DateTime.Now;
        this.Items = new List<BasketItem>();
    }
    public long Id { get; private set; }
    public long UserId { get; private set; }
    public BasketStatus Status { get; private set; }
    public List<BasketItem> Items { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastUpdatedAt { get; private set; }
    public Basket CreateBasket(long UserId)
    {
        return new Basket(UserId);
    }
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
            if (existingItem.Quantity >= 10)
            {
                throw new DomainException("حداکثر تعداد هر کالا 10 عدد است.");
            }
            existingItem.IncreaseQuantity(quantity);
        }
        ValidateTotalPrice();

    }
    public void MarkAsUpdated()
    {
        this.LastUpdatedAt = DateTime.Now;
    }
    public void Expire()
    {
        this.Status = BasketStatus.Expired;
    }
    public void UpdateQuantity(long productId, int newQuantity)
    {
        if (newQuantity > 10)
            throw new DomainException("حداکثر تعداد هر کالا 10 عدد است.");

        var item = Items.FirstOrDefault(x => x.ProductId == productId);

        if (item is null)
            throw new DomainException("کالا یافت نشد.");

        item.SetQuantity(newQuantity);

        ValidateTotalPrice();
    }
    public void RemoveItem(long productId)
    {
        var item = Items.FirstOrDefault(x => x.ProductId == productId);
        if (item is not null)
            Items.Remove(item);
    }
    private void ValidateTotalPrice()
    {
        var totalPrice = Items.Sum(x => x.Quantity * x.UnitPrice);

        if (totalPrice > 50_000_000)
            throw new DomainException("مبلغ سبد از حد مجاز بیشتر است.");
    }
    public void NewQuantity(long productId, int NewQuantity)
    {
        var item = Items.FirstOrDefault(x => x.ProductId == productId);
        if (item is not null)
            item.SetQuantity(NewQuantity);
    }
    public void ClearBasket()
    {
        Items.Clear();
        // میشد این هم ننوشت ولی بد نبود وجود داشتنش شاید یه روزی یه منطق جدید اینجا اضافه شد
    }

}