namespace BettingGame.Abstractions;

public interface IBetOutcomeResolver
{
    IBetOutcomeStrategy ResolveBetStrategy();
}