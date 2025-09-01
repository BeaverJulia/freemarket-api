using Shop.Domain.ValueObjects;

namespace Shop.Domain.Entities;

public class ShopCart
{
    public Guid Id { get; set; }
    public CountryCode Country { get; set; } = CountryCode.GB;
    public List<CartItem> Items { get; set; } = new();
    public string? AppliedDiscountCode { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    
    public Money GetSubtotalExVatEligible() => 
        Items.Where(i => i.IsEligibleForDiscountCode)
             .Aggregate(Money.Zero, (sum, item) => sum + item.GetLineTotal());
    
    public Money GetSubtotalExVatDiscounted() => 
        Items.Where(i => !i.IsEligibleForDiscountCode)
             .Aggregate(Money.Zero, (sum, item) => sum + item.GetLineTotal());
    
    public Money GetSubtotalExVatAll() => 
        Items.Aggregate(Money.Zero, (sum, item) => sum + item.GetLineTotal());
    
    public void AddItem(CartItem item)
    {
        var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            Items.Add(item);
        }
    }
    
    public void RemoveItem(Guid productId, int? quantity = null)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null) return;
        
        if (quantity == null || item.Quantity <= quantity)
        {
            Items.Remove(item);
        }
        else
        {
            item.Quantity = item.Quantity - quantity.Value;
        }
    }
}
