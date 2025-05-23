using BettingGame.Common;

namespace BettingGame.Abstractions;

public interface IBetService
{
    GameResult Bet(decimal amount);
}