using MediatR;

namespace SimagarOrder.Application.Basket.Events;
/*
 * برای وقتی که تغییراتی روی  سبد خرید اعمال میشه این کش رو پاک میکنه 
 * 
 */
public record RestoreCacheBasketEvent(long userId) : IRequest;
