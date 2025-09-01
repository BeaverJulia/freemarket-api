using Shop.Domain.Entities;

namespace Shop.Infrastructure.Interfaces;

public interface ICatalogStore
{
    Product? GetById(Guid id);
    IEnumerable<Product> GetAll();
}
