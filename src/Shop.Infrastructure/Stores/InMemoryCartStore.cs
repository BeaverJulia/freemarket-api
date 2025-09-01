using Shop.Domain.Entities;
using Shop.Infrastructure.Interfaces;
using System.Collections.Concurrent;

namespace Shop.Infrastructure.Stores;

public class InMemoryCartStore : ICartStore
{
    private readonly ConcurrentDictionary<Guid, ShopCart> _carts = new();
    
    public ShopCart? GetById(Guid id) => _carts.TryGetValue(id, out var cart) ? cart : null;
    
    public void Save(ShopCart cart) => _carts.AddOrUpdate(cart.Id, cart, (key, oldValue) => cart);
    
    public void Delete(Guid id) => _carts.TryRemove(id, out _);
}
