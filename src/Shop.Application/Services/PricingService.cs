using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Domain.ValueObjects;

namespace Shop.Application.Services;

public class PricingService : IPricingService
{
    public Money CalculateSubtotalExVat(ShopCart cart) 
    {
        return cart.GetSubtotalExVatAll();
    }
    
    public Money CalculateDiscountCodeSavings(ShopCart cart, string discountCode)
    {
        if (discountCode == "FREESHIP") return Money.Zero;
        
        var eligibleSubtotal = cart.GetSubtotalExVatEligible();
        var discountPercentage = discountCode switch
        {
            "SAVE10" => 0.10m,
            _ => 0m
        };
        
        return (eligibleSubtotal * discountPercentage).Round();
    }
    
    public Money CalculateVat(ShopCart cart, Money discountCodeSavings)
    {
        var vatRate = cart.Country == CountryCode.GB ? 0.20m : 0m;
        var vatableAmount = cart.GetSubtotalExVatAll() - discountCodeSavings;
        return (vatableAmount * vatRate).Round();
    }
}
