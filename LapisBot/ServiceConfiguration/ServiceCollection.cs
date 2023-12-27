namespace LapisBot.ServiceConfiguration;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Handlers.Interaction;
using Infrastructure.ServiceCollection;
using Setup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SeriLogThemesLibrary;

public static class ServiceCollection
{
    public static void AddDiscordServices(this IServiceCollection services)
    {
        var socketConfig = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.GuildBans
        };
        var socketClient = new DiscordSocketClient(socketConfig);

        services.AddSingleton(socketClient);
        services.AddSingleton<InteractionService>();
        services.AddHostedService<InteractionHandlingService>();
        services.AddHostedService<DiscordStartupService>();
        services.AddInfrastructure();
    }

    public static void AddLoggingServices(this IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console(theme: SeriLogCustomThemes.Theme1())
            .CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });
    }
}
