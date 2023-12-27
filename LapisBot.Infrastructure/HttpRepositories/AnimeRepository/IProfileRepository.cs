using LapisBot.Infrastructure.Http.Anime.Dtos.Profile;

namespace LapisBot.Infrastructure.HttpRepositories.AnimeRepository;

public interface IProfileRepository
{
    Task<UserDto?> GetUserDataAsync(string username);
}
