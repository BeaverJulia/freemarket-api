using Shop.Application.Interfaces;
using Shop.Domain.Entities;
using Shop.Domain.DTOs;
using Shop.Domain.Exceptions;
using Shop.Domain.ValueObjects;
using Shop.Infrastructure.Interfaces;

namespace Shop.Application.Services;

public class CartService : ICartService
{
    private readonly ICartStore _cartStore;
    private readonly ICatalogStore _catalogStore;
    private readonly IPricingService _pricingService;
    private readonly IDiscountService _discountService;
    private readonly IShippingService _shippingService;
    
    public CartService(
        ICartStore cartStore,
        ICatalogStore catalogStore,
        IPricingService pricingService,
        IDiscountService discountService,
        IShippingService shippingService)
    {
        _cartStore = cartStore;
        _catalogStore = catalogStore;
        _pricingService = pricingService;
        _discountService = discountService;
        _shippingService = shippingService;
    }
    
    public ShopCart CreateCart(string country)
    {
        var cart = new ShopCart
        {
            Id = Guid.NewGuid(),
            Country = CountryCode.FromString(country),
            CreatedUtc = DateTime.UtcNow
        };
        
        _cartStore.Save(cart);
        return cart;
    }
    
    public ShopCart? GetCart(Guid cartId) 
    {
        return _cartStore.GetById(cartId);
    }
    
    public ShopCart AddItem(Guid cartId, Guid productId, int quantity)
    {
        var cart = _cartStore.GetById(cartId) ?? throw new CartNotFoundException(cartId);
        var product = _catalogStore.GetById(productId) ?? throw new ProductNotFoundException(productId);
        
        var cartItem = new CartItem
        {
            ProductId = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            UnitPrice = product.UnitPrice,
            Quantity = quantity,
            IsDiscounted = product.IsDiscounted,
            DiscountPercent = product.DiscountPercent
        };
        
        cart.AddItem(cartItem);
        _cartStore.Save(cart);
        return cart;
    }
    
    public ShopCart AddMultipleItems(Guid cartId, List<AddItemRequest> items)
    {
        var cart = _cartStore.GetById(cartId) ?? throw new CartNotFoundException(cartId);
        
        foreach (var item in items)
        {
            var product = _catalogStore.GetById(item.ProductId) ?? throw new ProductNotFoundException(item.ProductId);
            
            var cartItem = new CartItem
            {
                ProductId = product.Id,
                Sku = product.Sku,
                Name = product.Name,
                UnitPrice = product.UnitPrice,
                Quantity = item.Quantity,
                IsDiscounted = product.IsDiscounted,
                DiscountPercent = product.DiscountPercent
            };
            
            cart.AddItem(cartItem);
        }
        
        _cartStore.Save(cart);
        return cart;
    }
    
    public ShopCart RemoveItem(Guid cartId, Guid productId, int? quantity = null)
    {
        var cart = _cartStore.GetById(cartId) ?? throw new CartNotFoundException(cartId);
        cart.RemoveItem(productId, quantity);
        _cartStore.Save(cart);
        return cart;
    }
    
    public ShopCart ApplyDiscountCode(Guid cartId, string code)
    {
        var cart = _cartStore.GetById(cartId) ?? throw new CartNotFoundException(cartId);
        
        if (!_discountService.IsValidDiscountCode(code))
            throw new InvalidDiscountCodeException(code);
        
        cart.AppliedDiscountCode = code;
        _cartStore.Save(cart);
        return cart;
    }
    
    public ShopCart SetShipping(Guid cartId, string country)
    {
        var cart = _cartStore.GetById(cartId) ?? throw new CartNotFoundException(cartId);
        cart.Country = CountryCode.FromString(country);
        _cartStore.Save(cart);
        return cart;
    }
    
    public CartTotalsResponse GetTotals(Guid cartId)
    {
        var cart = _cartStore.GetById(cartId) ?? throw new CartNotFoundException(cartId);
        
        var subtotalExVat = _pricingService.CalculateSubtotalExVat(cart);
        var discountCodeSavings = cart.AppliedDiscountCode != null 
            ? _pricingService.CalculateDiscountCodeSavings(cart, cart.AppliedDiscountCode)
            : Money.Zero;
        var vat = _pricingService.CalculateVat(cart, discountCodeSavings);
        var shipping = _shippingService.CalculateShipping(cart, cart.Country.Value);
        
        var grandTotalExVat = subtotalExVat - discountCodeSavings + shipping;
        var grandTotalIncVat = grandTotalExVat + vat;
        
        return new CartTotalsResponse
        {
            SubtotalExVat = subtotalExVat,
            DiscountCodeSavings = discountCodeSavings,
            Vat = vat,
            Shipping = shipping,
            GrandTotalExVat = grandTotalExVat,
            GrandTotalIncVat = grandTotalIncVat
        };
    }
}
