using BettingGame.Domain.Abstractions;
using BettingGame.Domain.ValueObjects;
using BettingGame.Shared.Exceptions;

namespace BettingGame.Domain.Entities;

public class Wallet : Entity<Guid>
{
    private Wallet(Guid id, Money initialBalance)
    {
        Id = id;
        Balance = initialBalance;
    }
    
    public Money Balance { get; private set; }

    public void Deposit(Money money)
    {
        if (money.Amount < 0)
        {
            throw new DomainException($"Cannot deposit negative amount!");
        }
        
        Balance += money;
    }
    
    public void Withdraw(Money money)
    {
        if (money.Amount < 0)
        {
            throw new DomainException($"Cannot withdraw negative amount!");
        }
        
        Balance -= money;
    }

    public static Wallet Create(Guid id, Money initialBalance)
        => new(id, initialBalance);
}