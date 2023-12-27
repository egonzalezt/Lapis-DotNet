namespace LapisBot.Setup;

using Configuration;
using Discord;
using Discord.WebSocket;
using Domain.Canvas.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Utils;

public class DiscordStartupService : IHostedService
{
    private readonly DiscordSocketClient _discord;
    private readonly ILogger<DiscordSocketClient> _logger;
    private readonly DiscordConfiguration _discordConfiguration;
    private readonly IWelcomeRepository _welcomeRepository;

    public DiscordStartupService(
        DiscordSocketClient discord,
        ILogger<DiscordSocketClient> logger,
        IOptions<DiscordConfiguration> discordConfiguration,
        IWelcomeRepository welcomeRepository
        )
    {
        _discord = discord;
        _logger = logger;
        _discordConfiguration = discordConfiguration.Value;
        _discord.Log += msg => LogHelper.OnLogAsync(_logger, msg);
        _welcomeRepository = welcomeRepository;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _discord.UserJoined += AnnounceJoinedUser;
        _discord.UserLeft += AnnounceUserLeft;
        await _discord.LoginAsync(TokenType.Bot, _discordConfiguration.Token);
        await _discord.SetGameAsync("now with / commands");
        await _discord.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _discord.LogoutAsync();
        await _discord.StopAsync();
    }

    private static async Task AnnounceUserLeft(SocketGuild guild, SocketUser user)
    {
        if (user.IsBot)
        {
            return;
        }

        var channel = guild.DefaultChannel;
        if (channel != null)
        {
            await channel.SendMessageAsync($"Goodbye {user.Username}, we will miss you! NOT EPICO");
        }
        return;
    }

    private async Task AnnounceJoinedUser(SocketGuildUser user)
    {
        var channel = user.Guild.DefaultChannel;
        var result = await _welcomeRepository.GetWelcomeCanvasAsync(user.GetAvatarUrl(), user.GlobalName, _discordConfiguration.SraToken);
        if (result.Item1 is null)
        {
            await channel.SendMessageAsync($"Welcome {user.Mention} to {channel.Guild.Name} EPICO");
            return;
        }

        if (!user.IsBot)
        {
            var dmChannel = await user.CreateDMChannelAsync();
            var embed = new EmbedBuilder()
           .WithTitle($"Welcome {user.Username} to {channel.Guild.Name} EPICO")
           .WithImageUrl("https://cdn.discordapp.com/attachments/812190063924084748/1189624147291164702/download_1.gif")
           .WithColor(new Color(255, 136, 0));
            await dmChannel.SendMessageAsync(embed: embed.Build());
        }

        using var ms = new MemoryStream(result.Item1);
        var img = new Image(ms);
        await channel.SendFileAsync(img.Stream, $"LapisCanvasWelcome{Guid.NewGuid()}.{result.Item2}");
        return;
    }
}
