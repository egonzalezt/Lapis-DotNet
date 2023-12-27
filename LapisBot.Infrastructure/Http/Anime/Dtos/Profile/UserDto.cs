namespace LapisBot.Infrastructure.Http.Anime.Dtos.Profile;

public class UserDto
{
    public string username { get; set; }
    public StatisticsDto statistics { get; set; }
    public FavoritesDto favorites { get; set; }
    public ImagesDto images { get; set; }
}
