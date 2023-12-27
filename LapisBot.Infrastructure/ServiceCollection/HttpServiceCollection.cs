using LapisBot.Infrastructure.HttpRepositories.CanvasRepository;
using Microsoft.Extensions.DependencyInjection;

namespace LapisBot.Infrastructure.ServiceCollection;

internal static class HttpServiceCollection
{
    public static void AddHttpClientsConfiguration(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpClient(nameof(MiscRepository), client =>
        {
            client.BaseAddress = new Uri("https://some-random-api.com/");
        });
        services.AddHttpClient("MyAnimeList", client =>
        {
            client.BaseAddress = new Uri("https://api.jikan.moe/");
        });
    }
}
