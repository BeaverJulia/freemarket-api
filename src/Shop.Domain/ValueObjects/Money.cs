namespace Shop.Domain.ValueObjects;

public record Money(decimal Amount)
{
    public static Money Zero => new(0.0m);
    
    public static Money operator +(Money left, Money right) => new(left.Amount + right.Amount);
    public static Money operator -(Money left, Money right) => new(left.Amount - right.Amount);
    public static Money operator *(Money left, decimal right) => new(left.Amount * right);
    
    public Money Round(int decimals = 2) => new(Math.Round(Amount, decimals, MidpointRounding.ToEven));
}
