using BettingGame.Abstractions;
using BettingGame.Common;

namespace BettingGame.Services;

public class GameService : IGameService
{
    public Task<GameResult> Bet(decimal amount)
    {
        throw new NotImplementedException();
    }
}