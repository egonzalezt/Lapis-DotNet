﻿namespace LapisBot.Commands.Pet
{
    using Discord.Interactions;
    using Newtonsoft.Json;
    using Discord;
    using Microsoft.Extensions.Logging;

    public class DogCommands : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<DogCommands> _logger;

        public DogCommands(IHttpClientFactory clientFactory, ILogger<DogCommands> logger)
        {
            _clientFactory = clientFactory;
            _logger = logger;
        }

        [SlashCommand("dog", "Get random dog image")]
        [RequireBotPermission(GuildPermission.AttachFiles)]
        public async Task GetRandomCatImage()
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.GetAsync("https://dog.ceo/api/breeds/image/random");
                var content = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(content);

                if(data is null)
                {
                    await RespondAsync("Sorry, I can't find dog images 😞");
                    return;
                }

                string imageUrl = data.message.ToString();

                var embed = new EmbedBuilder()
                  .WithImageUrl(imageUrl)
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
}
