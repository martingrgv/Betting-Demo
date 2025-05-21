using BettingGame.Common;

namespace BettingGame.Abstractions;

public interface ICommand
{
    Task<Result> Execute(string[] inputArgs);
}