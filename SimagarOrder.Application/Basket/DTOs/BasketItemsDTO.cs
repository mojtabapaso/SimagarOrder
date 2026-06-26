namespace SimagarOrder.Application.Basket.DTOs;

public class BasketItemsDTO
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}