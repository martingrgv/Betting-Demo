using BettingGame.Application.Abstractions;
using BettingGame.Application.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var app = CreateBuilder(args).Build();

var engine = app.Services.GetService<IGameEngine>();
await engine.RunAsync();


HostApplicationBuilder CreateBuilder(string[] args)
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Configuration.AddJsonFile("appsettings.json", false);
    
    builder.Logging.ClearProviders();
    builder.Services.AddSerilog(configuration =>
    {
        configuration.ReadFrom.Configuration(builder.Configuration);
    });
    
    builder.Services.AddSingleton<IGameEngine, GameEngine>();

    return builder;
}