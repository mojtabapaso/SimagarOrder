namespace SimagarOrder.Application.Basket.DTOs;

public class AddBasketItemDTO
{
    public long UserId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
