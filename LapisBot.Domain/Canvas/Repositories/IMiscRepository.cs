namespace LapisBot.Domain.Canvas.Repositories;

public interface IMiscRepository
{
    Task<(byte[]?, string?)> GetCanvasAsync(string mediaUrl, MiscCanvasType type);
}
