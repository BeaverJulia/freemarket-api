using Shop.Domain.Entities;
using Shop.Domain.ValueObjects;

namespace Shop.Application.Interfaces;

public interface IShippingService
{
    Money CalculateShipping(ShopCart cart, string country);
}
