using System.Text.Json.Serialization;

namespace mp3_lyrics_service
{
  public class Song
  {
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("lyrics_custom")]
    public string Lyrics { get; set; }

    public Song(string name, string lyrics)
    {
      Name = name;
      Lyrics = lyrics;
    }
  }
}
