namespace LapisBot.Infrastructure.HttpRepositories.CanvasRepository;

using Domain.Canvas.Repositories;
using Infrastructure.Http.ValidContentTypes;
using Microsoft.Extensions.Logging;

public class WelcomeRepository : IWelcomeRepository
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<WelcomeRepository> _logger;

    public WelcomeRepository(IHttpClientFactory clientFactory, ILogger<WelcomeRepository> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }
    public async Task<(byte[]?, string?)> GetWelcomeCanvasAsync(string profileUrl, string userName, string token)
    {
        var httpClient = _clientFactory.CreateClient(nameof(MiscRepository));
        var url = $"welcome/img/1/stars?type=join&username={userName}&avatar={profileUrl}&guildName=1&memberCount=1&textcolor=blue&key={token}";
        try
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var contentType = response.Content.Headers.ContentType?.MediaType;
            if (contentType is null)
            {
                _logger.LogWarning("No content type found");
                return (null, null);
            }
            if (!IsSupportedImageFormat(contentType, out var imageFormat))
            {
                _logger.LogWarning("Received an unsupported image format: {ContentType}", contentType);
                return (null, null);
            }

            var resultBytes = await response.Content.ReadAsByteArrayAsync();
            return (resultBytes, imageFormat);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error making HTTP request");
            return (null, null);
        }
    }

    private static bool IsSupportedImageFormat(string contentType, out string imageFormat)
    {
        foreach (ImageFormat format in Enum.GetValues(typeof(ImageFormat)))
        {
            var formatString = format.ToString().ToLowerInvariant();
            if (contentType?.ToLowerInvariant().Contains(formatString) == true)
            {
                imageFormat = formatString;
                return true;
            }
        }

        imageFormat = null;
        return false;
    }
}
