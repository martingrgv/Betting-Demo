namespace BettingGame.BetStrategies;

public class MultiplierWinStrategy : IBetOutcomeStrategy
{
    private const int LowMultiplier = 2;
    private const int HighMultiplier = 10;
    private readonly Random _random = new();

    public decimal CalculateOutcome(decimal betAmount)
    {
        var multiplier = GenerateRandomMultiplier(LowMultiplier, HighMultiplier);
        return betAmount * multiplier;
    }

    private decimal GenerateRandomMultiplier(int min, int max)
    {
        if (min >= max)
        {
            throw new ArgumentException($"{nameof(min)} must be less than {nameof(max)} value");
        }

        var range = (double)(max - min);
        var point = _random.NextDouble();
        return min + (decimal)(range * point);
    }
}