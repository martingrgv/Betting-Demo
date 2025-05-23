namespace BettingGame.BetStrategies;

public class BetOutcomeResolver : IBetOutcomeResolver
{
    private const int LowRoll = 1;
    private const int HighRoll = 100;
    private readonly Random _random = new();

    public IBetOutcomeStrategy ResolveBetStrategy()
    {
        var roll = _random.Next(LowRoll, HighRoll + 1);

        return roll switch
        {
            <= 50 => new LoseStrategy(),
            <= 90 => new DoubleWinStrategy(),
            _ => new MultiplierWinStrategy()
        };
    }
}