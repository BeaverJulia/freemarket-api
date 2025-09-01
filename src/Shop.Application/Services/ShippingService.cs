using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Domain.ValueObjects;

namespace Shop.Application.Services;

public class ShippingService : IShippingService
{
    public Money CalculateShipping(ShopCart cart, string country)
    {
        var countryCode = CountryCode.FromString(country);
        var subtotalExVat = cart.GetSubtotalExVatAll();

        if (cart.AppliedDiscountCode == "FREESHIP" && countryCode == CountryCode.GB)
            return Money.Zero;

        if (countryCode == CountryCode.GB && subtotalExVat.Amount >= 50m)
            return Money.Zero;

        if (countryCode.Value == "GB")
            return new Money(4.99m);
        else if (countryCode.Value == "EU")
            return new Money(9.99m);
        else
            return new Money(14.99m);
    }
}
