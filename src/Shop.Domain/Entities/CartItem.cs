using Shop.Domain.ValueObjects;

namespace Shop.Domain.Entities;

public class CartItem
{
    public Guid ProductId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Money UnitPrice { get; set; } = Money.Zero;
    public int Quantity { get; set; }
    public bool IsDiscounted { get; set; }
    public decimal DiscountPercent { get; set; }
    
    public Money GetLineTotal() => GetEffectivePrice() * Quantity;
    
    public Money GetEffectivePrice() => IsDiscounted 
        ? UnitPrice * (1 - DiscountPercent)
        : UnitPrice;
    
    public bool IsEligibleForDiscountCode => !IsDiscounted;
}
