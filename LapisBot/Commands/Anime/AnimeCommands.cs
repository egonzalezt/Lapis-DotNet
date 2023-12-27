namespace LapisBot.Commands.Anime;

using Discord;
using Discord.Interactions;
using Infrastructure.HttpRepositories.AnimeRepository;

public class AnimeCommands : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IAnimeRepository _animeRepository;
    private static readonly List<string> ProhibitedGenres = new() { "hentai" };

    public AnimeCommands(IAnimeRepository animeRepository)
    {
        _animeRepository = animeRepository;
    }

    [SlashCommand("random-anime", "Get random Anime")]
    public async Task MyAnimeListAnime()
    {
        var anime = await _animeRepository.GetRandomAnime();

        if (anime is null || anime.data is null)
        {
            await RespondAsync("Sorry we couldn't find any anime 😭", ephemeral: true);
            return;
        }

        if (anime.data.genres.Any(g => ProhibitedGenres.Contains(g.name, StringComparer.OrdinalIgnoreCase)))
        {
            await RespondAsync("Sorry the anime that I found is NSFW and this is a Christian channel", ephemeral: true);
            return;
        }

        var embed = new EmbedBuilder();

        if (!string.IsNullOrEmpty(anime.data.title))
        {
            embed.WithTitle(anime.data.title);
        }

        if (!string.IsNullOrEmpty(anime.data.mal_id))
        {
            embed.WithUrl($"https://myanimelist.net/anime/{anime.data.mal_id}");
        }

        if (!string.IsNullOrEmpty(anime.data.images?.jpg?.large_image_url))
        {
            embed.WithImageUrl(anime.data.images.jpg.large_image_url);
        }

        if (!string.IsNullOrEmpty(anime.data.synopsis))
        {
            embed.WithDescription(anime.data.synopsis);
        }

        if (anime.data.genres != null && anime.data.genres.Count > 0)
        {
            embed.AddField("Genres", string.Join(", ", anime.data.genres.Where(g => !ProhibitedGenres.Contains(g.name, StringComparer.OrdinalIgnoreCase)).Select(g => g.name)));
        }

        embed.WithColor(new Color(255, 136, 0))
             .WithFooter(footer => footer.Text = "Made by Vasitos Corp")
             .WithCurrentTimestamp();

        await RespondAsync(embed: embed.Build());
    }

}
