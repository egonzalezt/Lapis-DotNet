namespace LapisBot.Infrastructure.Http.Anime.Dtos.RandomAnime;

public class RandomAnimeDto
{
    public Data data { get; set; }
}

public class Data
{
    public string mal_id { get; set; }
    public Images images { get; set; }
    public string title { get; set; }
    public string synopsis { get; set; }
    public List<Genre> genres { get; set; }
}

public class Images
{
    public Jpg jpg { get; set; }
}

public class Jpg
{
    public string large_image_url { get; set; }
}

public class Genre
{
    public string name { get; set; }
}
