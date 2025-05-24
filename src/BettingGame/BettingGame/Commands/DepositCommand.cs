namespace BettingGame.Commands;

public class DepositCommand(IWalletService walletService, Wallet wallet) : ICommand
{
    public async Task<Result> Execute(string[] inputArgs)
    {
        try
        {
            decimal amount = decimal.Parse(inputArgs[1], NumberStyles.Number, CultureInfo.InvariantCulture);
            ArgumentOutOfRangeException.ThrowIfNegative(amount);
            
            await walletService.DepositAsync(wallet, amount);
            Log.Information("Deposit of ${amount} has been made on wallet: {WalletId}", amount, wallet.Id);

            string message = string.Join(
                " ",
                string.Format(MessageConstants.SuccessDepositMessage, amount),
                string.Format(MessageConstants.BalanceReviewMessage, $"{wallet.Balance:f2}"));

            return Result.Success(message);
        }
        catch (Exception ex)
        {
            return Result.Failure(MessageConstants.CommonErrorMessage, ex);
        }
    }
}