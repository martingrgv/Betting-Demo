using BettingGame.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace BettingGame.Commands;

public class CommandFactory(IServiceProvider serviceProvider)
{
    public ICommand Create<T>(params object[] parameters)
        where T : ICommand
    {
        return (ICommand)ActivatorUtilities.CreateInstance(serviceProvider, typeof(T), parameters);
    }
}