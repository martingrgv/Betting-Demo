using BettingGame.Common;

namespace BettingGame.Abstractions;

public interface IGameService
{
    Task<GameResult> Bet(decimal amount);
}