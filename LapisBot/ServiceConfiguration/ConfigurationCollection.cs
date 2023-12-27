namespace LapisBot.ServiceConfiguration;

using Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigurationCollection
{
    public static void ConfigureDiscordConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DiscordConfiguration>(configuration.GetSection("DiscordConfiguration"));
        services.Configure<EmojiConfiguration>(configuration.GetSection("EmojiConfiguration"));

    }
}