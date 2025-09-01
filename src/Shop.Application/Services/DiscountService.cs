using Shop.Application.Interfaces;

namespace Shop.Application.Services;

public class DiscountService : IDiscountService
{
    private readonly Dictionary<string, decimal> _validCodes = new()
    {
        { "SAVE10", 0.10m },
        { "FREESHIP", 0m }
    };
    
    public bool IsValidDiscountCode(string code) => _validCodes.ContainsKey(code);
    
    public decimal GetDiscountPercentage(string code) => 
        _validCodes.TryGetValue(code, out var percentage) ? percentage : 0.0m;
}
