using System.Diagnostics;
using System.Net.Http.Json;

namespace mp3_lyrics_service
{
  public class TagManager
  {
    private readonly TagManagerOptions _options;
    private readonly ILogger<TagManager> _logger;
    static readonly HttpClient client = new();

    public TagManager(ILogger<TagManager> logger, TagManagerOptions options)
    {
      _logger = logger;
      _options = options;
      client.BaseAddress = new Uri(_options.BaseUri);
    }

    public async void ManageUpdate(string filePath)
    {
      using var tfile = GetFileHandle(filePath);

      if (tfile.Tag.Lyrics == null) return;

      string fileName = filePath.Split('\\').Last().Trim().Replace(".mp3", "");
      Song song = new(fileName, tfile.Tag.Lyrics);

      HttpResponseMessage response = await client.PostAsJsonAsync(_options.PostUri, song);
      _logger.LogWarning($"{(response.IsSuccessStatusCode ? "Success" : "Error")} - {response.StatusCode}");
    }

    // https://stackoverflow.com/a/37154588/13363519
    private static TagLib.File GetFileHandle(string path, int timeoutMs = 2000)
    {
      var time = Stopwatch.StartNew();
      while (time.ElapsedMilliseconds < timeoutMs)
      {
        try
        {
          return TagLib.File.Create(path);
        }
        catch (IOException e)
        {
          // access error
          if (e.HResult != -2147024864) throw;
        }
      }

      throw new TimeoutException($"Failed to get a read handle to {path} within {timeoutMs}ms.");
    }

  }
}
