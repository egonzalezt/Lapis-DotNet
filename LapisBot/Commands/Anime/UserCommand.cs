namespace LapisBot.Commands.Anime;

using Discord;
using Discord.Interactions;
using Infrastructure.HttpRepositories.AnimeRepository;

public class UserCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IProfileRepository _profileRepository;
    public UserCommand(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    [SlashCommand("myanimelist-user", "Get my anime list user info")]
    public async Task MyAnimeListUser(string userName)
    {
        if (string.IsNullOrEmpty(userName))
        {
            await RespondAsync("Please send the username", ephemeral: true);
            return;
        }
        var userData = await _profileRepository.GetUserDataAsync(userName);
        if (userData is null)
        {
            await RespondAsync("Not found", ephemeral: true);
            return;
        }
        var topFavorites = userData.favorites.anime.Take(3).Select(a => a.title).ToList();
        var topFavoritesAnimes = string.Join("\n", topFavorites);
        if (topFavorites.Count == 0)
        {
            topFavoritesAnimes = "No favorite animes available.";
        }

        var embed = new EmbedBuilder()
            .WithTitle($"MyAnimeList Profile - {userData.username}")
            .WithUrl($"https://myanimelist.net/profile/{userData.username}")
            .WithThumbnailUrl(userData.images.jpg.image_url)
            .WithColor(new Color(255, 136, 0)) // Orange color
            .WithDescription($"Here are some stats for {userData.username}:")
            .AddField("Days Watched", $"{userData.statistics.anime.days_watched}", true)
            .AddField("Mean Score", $"{userData.statistics.anime.mean_score}", true)
            .AddField("Watching", $"{userData.statistics.anime.watching}", true)
            .AddField("Completed", $"{userData.statistics.anime.completed}", true)
            .AddField("On Hold", $"{userData.statistics.anime.on_hold}", true)
            .AddField("Dropped", $"{userData.statistics.anime.dropped}", true)
            .AddField("Plan To Watch", $"{userData.statistics.anime.plan_to_watch}", true)
            .AddField("Total Entries", $"{userData.statistics.anime.total_entries}", true)
            .AddField("Rewatched", $"{userData.statistics.anime.rewatched}", true)
            .AddField("Episodes Watched", $"{userData.statistics.anime.episodes_watched}", true)
            .AddField("Top 3 Favorite Animes", topFavoritesAnimes, false)
            .WithFooter(footer => footer.Text = "Made by Vasitos Corp")
            .WithCurrentTimestamp()
            .Build();
        await RespondAsync(embed: embed);
    }
}
