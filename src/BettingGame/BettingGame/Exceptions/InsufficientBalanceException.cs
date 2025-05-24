namespace BettingGame.Exceptions;

public class InsufficientBalanceException(Guid id, decimal amount)
    : Exception($"Cannot withdraw ${amount} from wallet with ID: {id}")
{
    public static void ThrowIfBalanceNegative(Wallet wallet, decimal amount)
    {
        if (wallet.Balance - amount >= 0)
            return;
            
        throw new InsufficientBalanceException(wallet.Id, amount);
    }
}