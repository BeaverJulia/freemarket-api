using Shop.Domain.Entities;

namespace Shop.Infrastructure.Interfaces;

public interface ICartStore
{
    ShopCart? GetById(Guid id);
    void Save(ShopCart cart);
    void Delete(Guid id);
}
