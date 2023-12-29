namespace LapisBot.Commands.Moderation.Channels;

using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Linq;

[DefaultMemberPermissions(GuildPermission.ManageChannels)]
public class ChannelCommands : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("mod-enable-channel", "Enable a specific channel.")]
    [RequireBotPermission(GuildPermission.ManageChannels)]
    public async Task EnableChannel(SocketGuildChannel channel, IRole? role = null)
    {
        if (channel != null)
        {
            var targetRole = role ?? Context.Guild.EveryoneRole;
            var overwritePermissions = new OverwritePermissions(sendMessages: PermValue.Allow);
            await channel.AddPermissionOverwriteAsync(targetRole, overwritePermissions);
            await RespondAsync($"The channel `{channel.Name}` has been enabled.", ephemeral: true);
        }
        else
        {
            await RespondAsync($"Channel does not exist.", ephemeral: true);
        }
    }

    [SlashCommand("mod-disable-channel", "Disable a specific channel.")]
    [RequireBotPermission(GuildPermission.ManageChannels)]
    public async Task DisableChannel(SocketGuildChannel channel, IRole? role = null)
    {
        if (channel != null)
        {
            var targetRole = role ?? Context.Guild.EveryoneRole;
            var overwritePermissions = new OverwritePermissions(sendMessages: PermValue.Deny);
            await channel.AddPermissionOverwriteAsync(targetRole, overwritePermissions);
            await RespondAsync($"The channel `{channel.Name}` has been disabled.", ephemeral: true);
        }
        else
        {
            await RespondAsync($"Channel does not exist.", ephemeral: true);
        }
    }

    [SlashCommand("mod-create-channel", "Creates a new text channel.")]
    [RequireBotPermission(GuildPermission.ManageChannels)]
    public async Task CreateChannel(string name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            await Context.Guild.CreateTextChannelAsync(name);
            await RespondAsync($"Channel `{name}` has been created.", ephemeral: true);
        }
        else
        {
            await RespondAsync("Please provide a valid name for the channel.", ephemeral: true);
        }
    }

    [SlashCommand("mod-assign-role-to-channel", "Assigns a role to a channel.")]
    [RequireBotPermission(GuildPermission.ManageRoles | GuildPermission.ManageChannels)]
    public async Task AssignRoleToChannel(SocketGuildChannel channel, IRole role)
    {
        if (channel != null && role != null)
        {
            var overwritePermissions = new OverwritePermissions(viewChannel: PermValue.Allow);
            await channel.AddPermissionOverwriteAsync(role, overwritePermissions);
            await RespondAsync($"Role `{role.Name}` has been granted access to the channel `{channel.Name}`.", ephemeral: true);
        }
        else
        {
            await RespondAsync("Channel or role does not exist.", ephemeral: true);
        }
    }

    [SlashCommand("mod-revoke-role-from-channel", "Revokes a role's access to a channel.")]
    [RequireBotPermission(GuildPermission.ManageRoles | GuildPermission.ManageChannels)]
    public async Task RevokeRoleFromChannel(SocketGuildChannel channel, IRole role)
    {
        if (channel != null && role != null)
        {
            var overwritePermissions = new OverwritePermissions(viewChannel: PermValue.Deny);
            await channel.AddPermissionOverwriteAsync(role, overwritePermissions);
            await RespondAsync($"Role `{role.Name}` has been removed from the channel `{channel.Name}`.", ephemeral: true);
        }
        else
        {
            await RespondAsync("Channel or role does not exist.", ephemeral: true);
        }
    }

    [SlashCommand("mod-add-user-to-channel", "Adds a user to a channel.")]
    [RequireBotPermission(GuildPermission.ManageChannels)]
    public async Task AddUserToChannel(SocketGuildChannel channel, IUser user)
    {
        if (channel != null && user != null)
        {
            var overwritePermissions = new OverwritePermissions(viewChannel: PermValue.Allow);
            await channel.AddPermissionOverwriteAsync(user, overwritePermissions);
            await RespondAsync($"User `{user.Username}` has been added to the channel `{channel.Name}`.", ephemeral: true);
        }
        else
        {
            await RespondAsync("Channel or user does not exist.", ephemeral: true);
        }
    }

    [SlashCommand("mod-revoke-user-from-channel", "Removes a user from a channel.")]
    [RequireBotPermission(GuildPermission.ManageChannels)]
    public async Task RemoveUserFromChannel(SocketGuildChannel channel, IUser user)
    {
        if (channel != null && user != null)
        {
            var overwritePermissions = new OverwritePermissions(viewChannel: PermValue.Deny);
            await channel.AddPermissionOverwriteAsync(user, overwritePermissions);
            await RespondAsync($"User `{user.Username}` has been removed from the channel `{channel.Name}`.", ephemeral: true);
        }
        else
        {
            await RespondAsync("Channel or user does not exist.", ephemeral: true);
        }
    }

    [SlashCommand("mod-set-slowmode", "Set the slow mode for a specific channel.")]
    [RequireBotPermission(GuildPermission.ManageChannels)]
    public async Task SetSlowMode(SocketGuildChannel channel, int delay = 5)
    {
        if (channel is SocketTextChannel textChannel)
        {
            await textChannel.ModifyAsync(x => x.SlowModeInterval = delay);
            await RespondAsync($"Slow mode has been enabled on the channel `{textChannel.Name}` with a delay of {delay} seconds.", ephemeral: true);
        }
        else
        {
            await RespondAsync("Only text channels can have slow mode enabled.", ephemeral: true);
        }
    }

    [SlashCommand("mod-delete-messages", "Deletes the specified amount of messages.")]
    [RequireUserPermission(GuildPermission.Administrator)]
    [RequireBotPermission(ChannelPermission.ManageMessages)]
    public async Task DeleteMessages(int amount)
    {
        const int maximunDeletePerRequest = 20;
        if (amount <= 0)
        {
            await RespondAsync("Please specify a positive number of messages to delete.", ephemeral: true);
            return;
        }

        if (amount > maximunDeletePerRequest)
        {
            await RespondAsync($"Sorry, but I can only delete a maximum of {maximunDeletePerRequest} messages per request", ephemeral: true);
            return;
        }

        await RespondAsync("Starting deletion...");

        var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();

        if (Context.Channel is IMessageChannel messageChannel)
        {
            var batches = messages.Chunk(4);

            foreach (var batch in batches)
            {
                foreach (var message in batch)
                {
                    _ = messageChannel.DeleteMessageAsync(message);
                    await Task.Delay(1000);
                }
            }
        }

        await FollowupAsync($"Deleted {amount} messages.", ephemeral: true);
    }
}
