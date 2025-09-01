using Shop.Domain.ValueObjects;

namespace Shop.Application.Interfaces;

public interface IVatService
{
    decimal GetVatRate(CountryCode country);
}
