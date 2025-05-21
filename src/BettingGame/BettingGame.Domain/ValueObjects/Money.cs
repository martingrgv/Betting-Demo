namespace BettingGame.Domain.ValueObjects;

public record Money
{
    private Money(decimal amount)
    {
        Amount = amount;
    }
    
    public decimal Amount { get; }
    
    public override string ToString()
        => $"${Amount}";

    public static Money operator +(Money a, Money b) => new Money(a.Amount + b.Amount);

    public static Money operator -(Money a, Money b) => new Money(a.Amount - b.Amount);

    public static Money Create(decimal amount)
        => new(amount);
}