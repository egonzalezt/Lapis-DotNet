using Discord;
using Discord.Interactions;

namespace LapisBot.Commands;
public class RouletteCommands : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("roulette", "Like a roulette, The input should be a string of values separated by commas, like a,b,c,d")]
    public async Task Roulette(string input)
    {
        var elements = input.Split(',');

        var embed = new EmbedBuilder()
            .WithImageUrl("https://64.media.tumblr.com/42ded69e00d9bbbec44cc6b4e7f25a3f/tumblr_mw5v90a3t71rmvkpdo1_500.gif")
            .WithDescription("Spinning...")
            .Build();
        await RespondAsync("Ok let's begin with the roulette");

        var message = await Context.Channel.SendMessageAsync(embed: embed);

        await Task.Delay(3000);

        var randomIndex = new Random().Next(elements.Length);
        var chosenElement = elements[randomIndex];

        var newEmbed = new EmbedBuilder()
            .WithImageUrl("https://64.media.tumblr.com/3f67776a237025ceb796b16453901ba3/tumblr_plwp1hWk6S1v1hotuo1_540.gif")
            .WithDescription($"The chosen element is `{chosenElement}`")
            .Build();

        await message.ModifyAsync(msg => msg.Embeds = new[] { newEmbed });
    }

}
