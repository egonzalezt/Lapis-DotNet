using Discord;
using Discord.Interactions;
using LapisBot.Infrastructure.HttpRepositories.AnimeRepository;
using Newtonsoft.Json;

namespace LapisBot.Commands.Anime;
public class AnimeCommands : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IAnimeRepository _animeRepository;
    public AnimeCommands(IAnimeRepository animeRepository)
    {
        _animeRepository = animeRepository;
    }

    [SlashCommand("random-anime", "Get random Anime")]
    public async Task MyAnimeListAnime()
    {
        var anime = await _animeRepository.GetRandomAnime();

        if(anime is null)
        {
            await RespondAsync("Sorry we couldn't find any anime 😭", ephemeral: true);
            return;
        }

        var embed = new EmbedBuilder()
            .WithTitle(anime.data.title)
            .WithUrl($"https://myanimelist.net/anime/{anime.data.mal_id}")
            .WithImageUrl(anime.data.images.jpg.large_image_url)
            .WithDescription(anime.data.synopsis)
            .AddField("Genres", string.Join(", ", anime.data.genres.Select(g => g.name)))
            .WithColor(new Color(255, 136, 0))
            .WithFooter(footer => footer.Text = "Made by Vasitos Corp")
            .WithCurrentTimestamp();

        await RespondAsync(embed: embed.Build());
    }

}
