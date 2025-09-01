using Shop.Domain.Entities;
using Shop.Domain.DTOs;

namespace Shop.Application.Interfaces;

public interface ICartService
{
    ShopCart CreateCart(string country);
    ShopCart? GetCart(Guid cartId);
    ShopCart AddItem(Guid cartId, Guid productId, int quantity);
    ShopCart AddMultipleItems(Guid cartId, List<AddItemRequest> items);
    ShopCart RemoveItem(Guid cartId, Guid productId, int? quantity = null);
    ShopCart ApplyDiscountCode(Guid cartId, string code);
    ShopCart SetShipping(Guid cartId, string country);
    CartTotalsResponse GetTotals(Guid cartId);
}
