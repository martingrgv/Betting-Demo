using System.Reflection.Metadata.Ecma335;
using BettingGame.Application.Abstractions;
using Serilog;

namespace BettingGame.Application.Common;

public class GameEngine : IGameEngine
{
    public Task RunAsync()
    {
        Log.Information("Engine ran");
        
        return Task.CompletedTask;
    }
}