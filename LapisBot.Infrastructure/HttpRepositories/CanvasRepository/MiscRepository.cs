namespace LapisBot.Infrastructure.HttpRepositories.CanvasRepository;

using LapisBot.Domain.Canvas;
using LapisBot.Domain.Canvas.Repositories;
using LapisBot.Infrastructure.Http.ValidContentTypes;
using Microsoft.Extensions.Logging;
using System.Net.Http;

public class MiscRepository : IMiscRepository
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<MiscRepository> _logger;

    public MiscRepository(IHttpClientFactory clientFactory, ILogger<MiscRepository> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<(byte[]?, string?)> GetCanvasAsync(string mediaUrl, MiscCanvasType type)
    {
        using var httpClient = _clientFactory.CreateClient(nameof(MiscRepository));
        var url = $"canvas/misc/{type}?avatar={mediaUrl}";

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
