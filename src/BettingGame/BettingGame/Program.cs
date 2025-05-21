using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

try
{
    var app = CreateBuilder(args).Build();

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;

    await app.ApplyMigrations(services);

    var engine = services.GetRequiredService<IGameEngine>();
    await engine.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, $"{nameof(Program)} error!");
}

HostApplicationBuilder CreateBuilder(string[] args)
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Configuration.AddJsonFile("appsettings.json", false);

    builder.Logging.ClearProviders();

    builder.Services.AddLogging(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplicationServices();

    return builder;
}