using Shop.Domain.ValueObjects;

namespace Shop.Domain.DTOs;

public record CreateCartRequest(string Country);

public record AddItemRequest(Guid ProductId, int Quantity);

public record AddMultipleItemsRequest(List<AddItemRequest> Items);

public record ApplyDiscountCodeRequest(string Code);

public record SetShippingRequest(string Country);

public record CartTotalsResponse
{
    public Money SubtotalExVat { get; set; } = Money.Zero;
    public Money DiscountCodeSavings { get; set; } = Money.Zero;
    public Money Vat { get; set; } = Money.Zero;
    public Money Shipping { get; set; } = Money.Zero;
    public Money GrandTotalExVat { get; set; } = Money.Zero;
    public Money GrandTotalIncVat { get; set; } = Money.Zero;
}
