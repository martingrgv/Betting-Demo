using BettingGame.BetStrategies;
using BettingGame.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BettingGame;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IBetService, BetService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IGameEngine, GameEngine>();
        services.AddScoped<CommandFactory>();
        services.AddSingleton<IBetOutcomeResolver, BetOutcomeResolver>();
        
        return services;
    }

    public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog(loggerConfiguration =>
        {
            loggerConfiguration.ReadFrom.Configuration(configuration);
        });
        
        return services;
    }
}