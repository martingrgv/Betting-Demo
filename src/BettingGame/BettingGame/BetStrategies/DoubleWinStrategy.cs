namespace BettingGame.BetStrategies;

public class DoubleWinStrategy : IBetOutcomeStrategy
{
    public decimal CalculateOutcome(decimal betAmount) => betAmount * 2;
}