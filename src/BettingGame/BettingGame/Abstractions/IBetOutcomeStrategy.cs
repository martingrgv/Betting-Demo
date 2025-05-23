namespace BettingGame.Abstractions;

public interface IBetOutcomeStrategy
{
    decimal CalculateOutcome(decimal betAmount);
}