using System.Globalization;

namespace BettingGame.Commands;

public class WithdrawCommand(IWalletService walletService, Wallet wallet) : ICommand
{
    public async Task<Result> Execute(string[] inputArgs)
    {
        try
        {
            decimal amount = decimal.Parse(inputArgs[1], NumberStyles.Number, CultureInfo.InvariantCulture);
            
            await walletService.DepositAsync(wallet, amount);
            Log.Information("Deposit of ${amount} has been made on wallet: {WalletId}", amount, wallet.Id);

            string message = string.Join(
                " ",
                string.Format(MessageConstants.SuccessWithdrawMessage, amount),
                string.Format(MessageConstants.BalanceReviewMessage, wallet.Balance));

            return Result.Success(message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }
}