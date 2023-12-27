namespace LapisBot.Infrastructure.Http.Anime.Dtos.Profile;

public class UserWrapperDto
{
    public UserDto? data { get; set; }
}

public class AnimeDto
{
    public double? days_watched { get; set; }
    public double? mean_score { get; set; }
    public int? watching { get; set; }
    public int? completed { get; set; }
    public int? on_hold { get; set; }
    public int? dropped { get; set; }
    public int? plan_to_watch { get; set; }
    public int? total_entries { get; set; }
    public int? rewatched { get; set; }
    public int? episodes_watched { get; set; }
}
