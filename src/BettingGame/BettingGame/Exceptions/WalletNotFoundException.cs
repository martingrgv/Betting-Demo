namespace BettingGame.Exceptions;

public class WalletNotFoundException : Exception
{
    public WalletNotFoundException(Guid id)
     : base($"Wallet with {id} ID was not found")
    {

    }
}