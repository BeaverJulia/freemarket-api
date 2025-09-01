using Shop.Application.Services;
using Shop.Domain.Entities;
using Shop.Domain.ValueObjects;
using Xunit;

namespace Shop.Application.Tests;

public class PricingServiceTests
{
    private readonly PricingService _pricingService;
    
    public PricingServiceTests()
    {
        _pricingService = new PricingService();
    }
    
    [Fact]
    public void CalculateSubtotalExVat_WithItems_ReturnsCorrectTotal()
    {
        var cart = new ShopCart
        {
            Id = Guid.NewGuid(),
            Country = CountryCode.GB,
            Items = new List<CartItem>
            {
                new() { UnitPrice = new Money(5.00m), Quantity = 2, IsDiscounted = false },
                new() { UnitPrice = new Money(1.20m), Quantity = 1, IsDiscounted = true, DiscountPercent = 0.10m }
            }
        };
        var result = _pricingService.CalculateSubtotalExVat(cart);
        Assert.Equal(11.08m, result.Amount);
    }
    
    [Fact]
    public void CalculateDiscountCodeSavings_WithSAVE10_ReturnsCorrectSavings()
    {
        var cart = new ShopCart
        {
            Id = Guid.NewGuid(),
            Country = CountryCode.GB,
            Items = new List<CartItem>
            {
                new() { UnitPrice = new Money(5.00m), Quantity = 2, IsDiscounted = false },
                new() { UnitPrice = new Money(1.20m), Quantity = 1, IsDiscounted = true, DiscountPercent = 0.10m }
            }
        };
        var result = _pricingService.CalculateDiscountCodeSavings(cart, "SAVE10");
        Assert.Equal(1.00m, result.Amount);
    }
    
    [Fact]
    public void CalculateDiscountCodeSavings_WithFREESHIP_ReturnsZero()
    {
        var cart = new ShopCart
        {
            Id = Guid.NewGuid(),
            Country = CountryCode.GB,
            Items = new List<CartItem>
            {
                new() { UnitPrice = new Money(5.00m), Quantity = 1, IsDiscounted = false }
            }
        };
        var result = _pricingService.CalculateDiscountCodeSavings(cart, "FREESHIP");
        Assert.Equal(0m, result.Amount);
    }
    
    [Fact]
    public void CalculateVat_WithGBAndDiscount_ReturnsCorrectVAT()
    {
        var cart = new ShopCart
        {
            Id = Guid.NewGuid(),
            Country = CountryCode.GB,
            Items = new List<CartItem>
            {
                new() { UnitPrice = new Money(5.00m), Quantity = 2, IsDiscounted = false },
                new() { UnitPrice = new Money(1.20m), Quantity = 1, IsDiscounted = true, DiscountPercent = 0.10m }
            }
        };
        var discountCodeSavings = new Money(1.00m);
        var result = _pricingService.CalculateVat(cart, discountCodeSavings);
        Assert.Equal(2.02m, result.Amount);
    }
    
    [Fact]
    public void CalculateVat_WithNonGB_ReturnsZero()
    {
        var cart = new ShopCart
        {
            Id = Guid.NewGuid(),
            Country = CountryCode.EU,
            Items = new List<CartItem>
            {
                new() { UnitPrice = new Money(5.00m), Quantity = 1, IsDiscounted = false }
            }
        };
        var discountCodeSavings = Money.Zero;
        var result = _pricingService.CalculateVat(cart, discountCodeSavings);
        Assert.Equal(0m, result.Amount);
    }
}
