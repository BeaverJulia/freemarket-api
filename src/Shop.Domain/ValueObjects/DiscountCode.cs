namespace Shop.Domain.ValueObjects;

public record DiscountCode(string Code, decimal Percentage)
{
    public static readonly DiscountCode Save10 = new("SAVE10", 0.10m);
    public static readonly DiscountCode FreeShip = new("FREESHIP", 0m);
}
