namespace LapisBot.Infrastructure.HttpRepositories.AnimeRepository;

using Infrastructure.Http.Anime.Dtos.Profile;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class ProfileRepository : IProfileRepository
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<ProfileRepository> _logger;
    private const string MyAnimeListTitle = "MyAnimeList";
    public ProfileRepository(IHttpClientFactory clientFactory, ILogger<ProfileRepository> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<UserDto?> GetUserDataAsync(string username)
    {
        using var client = _clientFactory.CreateClient(MyAnimeListTitle);
        var response = await client.GetAsync($"/v4/users/{username}/full");

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogWarning("There is no respose getting the user {USER}", username);
            return null;
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var userData = JsonConvert.DeserializeObject<UserWrapperDto>(jsonResponse);

        if (userData is null)
        {
            return null;
        }

        return userData.data;
    }
}
