namespace LapisBot.Domain.Canvas.Repositories;

public interface IOverlayRepository
{
    Task<(byte[]?, string?)> GetCanvasAsync(string mediaUrl, OverlayCanvasType type);
}
