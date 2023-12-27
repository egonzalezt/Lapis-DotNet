namespace LapisBot.Domain.Canvas.Repositories;

public interface IWelcomeRepository
{
    Task<(byte[]?, string?)> GetWelcomeCanvasAsync(string profileUrl, string userName, string token);
}
