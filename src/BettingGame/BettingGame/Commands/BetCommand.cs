namespace BettingGame.Commands;

public class BetCommand(IBetService betService, IWalletService walletService, Wallet wallet) : ICommand
{
    public async Task<Result> Execute(string[] inputArgs)
    {
        try
        {
            decimal betAmount = decimal.Parse(inputArgs[1], NumberStyles.Number, CultureInfo.InvariantCulture);

            if (betAmount > wallet.Balance)
            {
                throw new InsufficientBalanceException(wallet.Id, betAmount);
            }

            var gameResult = betService.Bet(betAmount);

            if (gameResult.IsWin)
            {
                var total = gameResult.WinAmount + betAmount;
                var profit = total - betAmount;

                await walletService.DepositAsync(wallet, profit);
            }
            else
            {
                await walletService.WithdrawAsync(wallet, betAmount);
            }

            string message = BuildOutcomeMessage(gameResult, wallet.Balance);
            return Result.Success(message);
        }
        catch (InsufficientBalanceException ex)
        {
            return Result.Failure(
                MessageConstants.BetErrorDueToInsufficientBalanceMessage,
                ex);
        }
        catch (ArgumentException ex)
        {
            return Result.Failure(ex.ParamName!, ex);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex);
        }
    }

    private static string BuildOutcomeMessage(GameResult gameResult, decimal walletBalance)
    {
        if (gameResult.IsWin)
        {
            return string.Join(
                " ",
                string.Format(MessageConstants.WinMessage, $"{gameResult.WinAmount:f2}"),
                string.Format(MessageConstants.BalanceReviewMessage, $"{walletBalance:f2}"));
        }
        
        return string.Join(
            " ",
            MessageConstants.LoseMessage,
            string.Format(MessageConstants.BalanceReviewMessage, $"{walletBalance:f2}"));
    }
}