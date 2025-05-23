using BettingGame.BetStrategies;
using BettingGame.Data;
using BettingGame.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
    
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new ArgumentNullException("Connection string cannot be empty!");
        
        services.AddDbContext<BettingDbContext>( options =>
        {
            options.UseSqlServer(connectionString);
        });
        
        return services;
    }

    public static async Task ApplyMigrations(this IHost host, IServiceProvider scopeServices)
    {
        var context = scopeServices.GetRequiredService<BettingDbContext>();
        await context.Database.MigrateAsync();
    }
}