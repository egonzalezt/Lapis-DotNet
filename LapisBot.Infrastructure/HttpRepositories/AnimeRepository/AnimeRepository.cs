namespace LapisBot.Infrastructure.HttpRepositories.AnimeRepository;

using Infrastructure.Http.Anime.Dtos.RandomAnime;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class AnimeRepository : IAnimeRepository
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<AnimeRepository> _logger;
    private const string MyAnimeListTitle = "MyAnimeList";
    public AnimeRepository(IHttpClientFactory clientFactory, ILogger<AnimeRepository> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<RandomAnimeDto?> GetRandomAnime()
    {
        using var client = _clientFactory.CreateClient(MyAnimeListTitle);
        var response = await client.GetAsync($"/v4/random/anime");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("Something happens getting the random anime, {Status}", response.StatusCode);
            return null;
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var randomAnime = JsonConvert.DeserializeObject<RandomAnimeDto>(jsonResponse);

        if (randomAnime is null)
        {
            return null;
        }

        return randomAnime;
    }
}
