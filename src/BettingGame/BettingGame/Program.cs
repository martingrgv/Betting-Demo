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
    Log.Fatal(ex, $"{nameof(Program)} execution error!");
}

HostApplicationBuilder CreateBuilder(string[] args)
{
    var builder = Host.CreateApplicationBuilder(args);
    builder.Configuration
        .AddJsonFile("appsettings.json", false)
        .AddEnvironmentVariables();

    builder.Logging.ClearProviders();
    
    builder.ConfigureContainer(new DefaultServiceProviderFactory(new ServiceProviderOptions
    {
        ValidateOnBuild = true,
        ValidateScopes = true
    }));

    builder.Services.AddLogging(builder.Configuration);
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApplicationServices();

    return builder;
}