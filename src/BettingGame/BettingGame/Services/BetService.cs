namespace BettingGame.Services;

public class BetService(IBetOutcomeResolver betOutcomeResolver) : IBetService
{
    private const decimal MinBetAmount = 1;
    private const decimal MaxBetAmount = 10;
    
    public GameResult Bet(decimal amount)
    {
        if (amount < MinBetAmount ||  amount > MaxBetAmount)
        {
            throw new ArgumentOutOfRangeException($"{nameof(amount)} must be between {MinBetAmount} and {MaxBetAmount}");
        }
        
        var outcomeStrategy = betOutcomeResolver.ResolveBetStrategy();
        var winAmount = outcomeStrategy.CalculateOutcome(amount);
        bool isWin = winAmount > 0;
        
        var result = new GameResult(
            IsWin: isWin,
            BetAmount: amount,
            WinAmount: winAmount);

        return result;
    }
}