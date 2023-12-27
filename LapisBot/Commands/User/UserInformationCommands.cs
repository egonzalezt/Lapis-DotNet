namespace LapisBot.Commands.User;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Domain.Canvas;
using Domain.Canvas.Repositories;

public class UserInformationCommands : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IMiscRepository _miscRepository;
    private readonly IOverlayRepository _overlayRepository;

    public UserInformationCommands(IMiscRepository miscRepository, IOverlayRepository overlayRepository)
    {
        _miscRepository = miscRepository;
        _overlayRepository = overlayRepository;

    }

    [SlashCommand("invocate", "Invites a user to a channel.")]
    [RequireUserPermission(GuildPermission.SendMessages)]
    [RequireBotPermission(GuildPermission.SendMessages)]
    public async Task Invocate(SocketGuildChannel channel, IUser user, string message)
    {
        if (channel != null && user != null)
        {
            if (user.IsBot)
            {
                await RespondAsync("Psst this is a bot, we don't talk to each other.", ephemeral: true);
                return;
            }

            if (user is SocketUser socketUser)
            {
                var dmChannel = await socketUser.CreateDMChannelAsync();
                await dmChannel.SendMessageAsync($"Hey {user.Username}, {Context.User.Username} requires you on {channel.Name} and says: {message}");
                await RespondAsync($"An invocation message has been sent to {user.Username}.", ephemeral: true);
            }
            else
            {
                await RespondAsync("Cannot process user", ephemeral: true);
            }
        }
        else
        {
            await RespondAsync("Channel or user does not exist.", ephemeral: true);
        }
    }

    [SlashCommand("user-info", "Shows user info in an embed.")]
    public async Task UserInfo(IUser? user = null)
    {
        const int imageSize = 2048;
        user ??= Context.User;

        string statusEmoji;
        switch (user.Status)
        {
            case UserStatus.Online:
                statusEmoji = ":green_circle:";
                break;
            case UserStatus.Offline:
                statusEmoji = ":black_circle:";
                break;
            case UserStatus.Idle:
                statusEmoji = ":yellow_circle:";
                break;
            case UserStatus.DoNotDisturb:
                statusEmoji = ":red_circle:";
                break;
            default:
                statusEmoji = ":grey_question:";
                break;
        }

        var embed = new EmbedBuilder()
           .WithTitle(user.Username)
           .AddField("Status", statusEmoji, true)
           .AddField("CreatedAt", user.CreatedAt, false)
           .WithImageUrl(user.GetAvatarUrl(size: imageSize))
           .WithColor(new Color(255, 136, 0))
           .WithFooter(footer => footer.Text = "Made by Vasitos Corp")
           .WithCurrentTimestamp();

        await RespondAsync(embed: embed.Build());
    }




    [SlashCommand("misc-avatar", "Get user custom avatar")]
    [RequireContext(ContextType.Guild)]
    [RequireBotPermission(GuildPermission.AttachFiles)]
    public async Task MiscCanvasAvatar(MiscCanvasType modificationType, SocketGuildUser? user = null)
    {
        user ??= (SocketGuildUser)Context.User;
        var avatarUrl = user.GetAvatarUrl();
        var result = await _miscRepository.GetCanvasAsync(avatarUrl, modificationType);
        if (result.Item1 is null)
        {
            await RespondAsync("Sorry, I can't process your request 😞", ephemeral: true);
            return;
        }

        using var ms = new MemoryStream(result.Item1);
        var img = new Image(ms);
        await RespondWithFileAsync(img.Stream, $"LapisCanvas{Guid.NewGuid()}.{result.Item2}");
    }

    [SlashCommand("overlay-avatar", "Get user custom avatar")]
    [RequireContext(ContextType.Guild)]
    [RequireBotPermission(GuildPermission.AttachFiles)]
    public async Task OverlayCanvasAvatar(OverlayCanvasType modificationType, SocketGuildUser? user = null)
    {
        user ??= (SocketGuildUser)Context.User;
        var avatarUrl = user.GetAvatarUrl();
        var result = await _overlayRepository.GetCanvasAsync(avatarUrl, modificationType);
        if (result.Item1 is null)
        {
            await RespondAsync("Sorry, I can't process your request 😞", ephemeral: true);
            return;
        }

        using var ms = new MemoryStream(result.Item1);
        var img = new Image(ms);
        await RespondWithFileAsync(img.Stream, $"LapisCanvas{Guid.NewGuid()}.{result.Item2}");
    }
}
