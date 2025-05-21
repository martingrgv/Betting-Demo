using BettingGame.Data.Persistence;
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
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddSingleton<IGameEngine, GameEngine>();
        services.AddSingleton<CommandFactory>();
        
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