using Shop.Domain.Entities;
using Shop.Domain.ValueObjects;

namespace Shop.Application.Interfaces;

public interface IPricingService
{
    Money CalculateSubtotalExVat(ShopCart cart);
    Money CalculateDiscountCodeSavings(ShopCart cart, string discountCode);
    Money CalculateVat(ShopCart cart, Money discountCodeSavings);
}
