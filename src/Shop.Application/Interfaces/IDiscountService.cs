namespace Shop.Application.Interfaces;

public interface IDiscountService
{
    bool IsValidDiscountCode(string code);
    decimal GetDiscountPercentage(string code);
}
