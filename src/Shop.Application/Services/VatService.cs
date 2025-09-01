using Shop.Application.Interfaces;
using Shop.Domain.ValueObjects;

namespace Shop.Application.Services;

public class VatService : IVatService
{
    public decimal GetVatRate(CountryCode country) => country.Value switch
    {
        "GB" => 0.20m,
        _ => 0m
    };
}
