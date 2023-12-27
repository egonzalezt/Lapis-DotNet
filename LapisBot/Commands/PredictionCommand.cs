namespace LapisBot.Commands;

using Discord.Interactions;
using LapisBot.Configuration;
using Microsoft.Extensions.Options;
public class PredictionCommand : InteractionModuleBase<SocketInteractionContext>
{
    private readonly string[] emojis;

    public PredictionCommand(IOptions<EmojiConfiguration> emojiOptions)
    {
        emojis = emojiOptions.Value.Emojis;
    }

    [SlashCommand("predict", "Predict your future with emojis")]
    public async Task PredictionAsync(int numberOfEmojis = 10)
    {
        if (numberOfEmojis > 10)
        {
            var message = $"Invalid number of emojis, expected is must be less than 10, current input is {numberOfEmojis}";
            await ReplyAsync(message);
            return;
        }

        var selectedEmojis = new List<string>();
        var rnd = new Random();
        for (int i = 0; i < numberOfEmojis; i++)
        {
            var index = rnd.Next(emojis.Length);
            selectedEmojis.Add(emojis[index]);
        }
        string prediction = string.Join("", selectedEmojis);

        await RespondAsync(prediction);
    }
}
