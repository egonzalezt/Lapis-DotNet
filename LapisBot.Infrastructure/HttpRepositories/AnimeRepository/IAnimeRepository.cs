namespace LapisBot.Infrastructure.HttpRepositories.AnimeRepository;

using Infrastructure.Http.Anime.Dtos.RandomAnime;

public interface IAnimeRepository
{
    Task<RandomAnimeDto?> GetRandomAnime();
}
