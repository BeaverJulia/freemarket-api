using Shop.Domain.ValueObjects;

namespace Shop.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public Money UnitPrice { get; set; } = Money.Zero;
    public bool IsDiscounted { get; set; }
    public decimal DiscountPercent { get; set; }
    
    public Money GetEffectivePrice() => IsDiscounted 
        ? UnitPrice * (1 - DiscountPercent)
        : UnitPrice;
}
