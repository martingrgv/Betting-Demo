using System.Globalization;
using BettingGame.Exceptions;

namespace BettingGame.Commands;

public class WithdrawCommand(IWalletService walletService, Wallet wallet) : ICommand
{
    public async Task<Result> Execute(string[] inputArgs)
    {
        try
        {
            decimal amount = decimal.Parse(inputArgs[1], NumberStyles.Number, CultureInfo.InvariantCulture);
            
            await walletService.WithdrawAsync(wallet, amount);
            Log.Information("Withdraw of ${amount} has been made on wallet: {WalletId}", amount, wallet.Id);

            string message = string.Join(
                " ",
                string.Format(MessageConstants.SuccessWithdrawMessage, amount),
                string.Format(MessageConstants.BalanceReviewMessage, wallet.Balance));

            return Result.Success(message);
        }
        catch (Exception ex)
        {
            if (ex is InsufficientBalanceException)
            {
                return Result.Failure(MessageConstants.InsufficientBalanceWithdrawMessage, ex);
            }
            
            return Result.Failure(ex);
        }
    }
}