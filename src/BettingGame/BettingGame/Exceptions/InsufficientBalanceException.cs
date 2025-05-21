namespace BettingGame.Exceptions;

public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException(Guid id, decimal amount)
        : base($"Cannot withdraw ${amount} from wallet with ID: {id}")
    {

    }
}