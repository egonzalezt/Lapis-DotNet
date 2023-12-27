namespace LapisBot.Commands.User;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Domain.Canvas;
using Domain.Canvas.Repositories;
using System.Runtime.InteropServices;

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
            var socketUser = user as SocketUser;
            if (socketUser != null)
            {
                var dmChannel = await socketUser.CreateDMChannelAsync();
                await dmChannel.SendMessageAsync($"Hey {user.Username}, {Context.User.Username} requires you on {channel.Name} and says: {message}");
                await RespondAsync($"An invocation message has been sent to {user.Username}.", ephemeral: true);
            }
            else
            {
                await RespondAsync("Cannot convert IUser to SocketUser.", ephemeral: true);
            }
        }
        else
        {
            await RespondAsync("Channel or user does not exist.", ephemeral: true);
        }
    }


    [SlashCommand("misc-avatar", "Get user custom avatar")]
    [RequireContext(ContextType.Guild)]
    [RequireBotPermission(GuildPermission.AttachFiles)]
    public async Task MiscCanvasAvatar(MiscCanvasType modificationType, SocketGuildUser user = null)
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
    public async Task OverlayCanvasAvatar(OverlayCanvasType modificationType, SocketGuildUser user = null)
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
