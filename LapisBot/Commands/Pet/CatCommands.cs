namespace LapisBot.Commands.Pet;

using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class CatCommands : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<DogCommands> _logger;

    public CatCommands(IHttpClientFactory clientFactory, ILogger<DogCommands> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    [SlashCommand("cat", "Get random cat image")]
    [RequireBotPermission(GuildPermission.AttachFiles)]
    public async Task GetRandomCatImage()
    {
        try
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://api.thecatapi.com/v1/images/search");
            var content = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(content);

            if (data is null)
            {
                await RespondAsync("Sorry, I can't find cat images 😞");
                return;
            }

            string imageUrl = data[0].url.ToString();

            var embed = new EmbedBuilder()
              .WithImageUrl(imageUrl)
              .WithFooter(footer => footer.Text = "Made by Vasitos Corp")
              .Build();

            await RespondAsync(embed: embed);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e.Message);
            await RespondAsync("Sorry, I can't find dog images 😞");
        }
    }
}
