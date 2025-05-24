namespace BettingGame.Data.Models;

public class Wallet
{
    private Wallet()
    {
        // For EF Core
    }
    
    public Wallet(Guid id, Guid playerId, decimal initialBalance)
    {
        Id = id;
        PlayerId = playerId;
        Balance = initialBalance;
    }
    
    public Guid Id { get; private set; }
    public Guid PlayerId { get; private set; }
    public decimal Balance { get; private set; }

    public void UpdateAmount(decimal amount)
    {
        try
        {
            checked
            {
                Balance += amount;
            }
        }
        catch (OverflowException ex)
        {
            throw new InvalidOperationException("Could not update value! Overflow may occur.", ex);
        }
    }
}