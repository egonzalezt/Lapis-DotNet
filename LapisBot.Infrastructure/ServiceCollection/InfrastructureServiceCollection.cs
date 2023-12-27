namespace LapisBot.Infrastructure.ServiceCollection;

using Domain.Canvas.Repositories;
using Infrastructure.HttpRepositories.CanvasRepository;
using LapisBot.Infrastructure.HttpRepositories.AnimeRepository;
using Microsoft.Extensions.DependencyInjection;

public static class InfrastructureServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpClientsConfiguration();
        services.AddSingleton<IMiscRepository, MiscRepository>();
        services.AddSingleton<IOverlayRepository, OverlayRepository>();
        services.AddSingleton<IWelcomeRepository, WelcomeRepository>();
        services.AddSingleton<IProfileRepository, ProfileRepository>();
        services.AddSingleton<IAnimeRepository, AnimeRepository>();
    }
}
