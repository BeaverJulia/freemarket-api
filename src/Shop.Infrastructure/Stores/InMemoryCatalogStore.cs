using Shop.Domain.Entities;
using Shop.Domain.ValueObjects;
using Shop.Infrastructure.Interfaces;

namespace Shop.Infrastructure.Stores;

public class InMemoryCatalogStore : ICatalogStore
{
    private readonly Dictionary<Guid, Product> _products;
    
    public InMemoryCatalogStore()
    {
        _products = new Dictionary<Guid, Product>
        {
            {
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                new Product
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Sku = "APP-001",
                    Name = "Apple",
                    UnitPrice = new Money(0.50m),
                    IsDiscounted = false,
                    DiscountPercent = 0m
                }
            },
            {
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                new Product
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Sku = "BREAD-001",
                    Name = "Bread",
                    UnitPrice = new Money(1.20m),
                    IsDiscounted = true,
                    DiscountPercent = 0.10m
                }
            },
            {
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                new Product
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Sku = "MILK-001",
                    Name = "Milk",
                    UnitPrice = new Money(0.90m),
                    IsDiscounted = false,
                    DiscountPercent = 0m
                }
            },
            {
                Guid.Parse("44444444-4444-4444-4444-444444444444"),
                new Product
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Sku = "COFFEE-001",
                    Name = "Coffee",
                    UnitPrice = new Money(5.00m),
                    IsDiscounted = false,
                    DiscountPercent = 0m
                }
            }
        };
    }
    
    public Product? GetById(Guid id) => _products.TryGetValue(id, out var product) ? product : null;
    
    public IEnumerable<Product> GetAll() => _products.Values;
}
