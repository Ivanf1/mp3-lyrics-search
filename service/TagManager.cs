using System.Diagnostics;
using System.Net.Http.Json;

namespace mp3_lyrics_service
{
  public class TagManager
  {
    static readonly HttpClient client = new()
    {
      BaseAddress = new Uri("http://localhost:8983/solr/mp3_lyrics/")
    };
    private readonly ILogger<TagManager> _logger;

    public TagManager(ILogger<TagManager> logger)
    {
      _logger = logger;
    }

    public async void ManageUpdate(string filePath)
    {
      using (var tfile = GetFileHandle(filePath))
      {
        string fileName = filePath.Split('\\').Last().Trim().Replace(".mp3", "");
        Song song = new(fileName, tfile.Tag.Lyrics);

        HttpResponseMessage response = await client.PostAsJsonAsync("update/json/docs", song);
        _logger.LogWarning($"{(response.IsSuccessStatusCode ? "Success" : "Error")} - {response.StatusCode}");

        tfile.Dispose();
      }
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
          if (e.HResult != -2147024864)
            throw;
        }
      }

      throw new TimeoutException($"Failed to get a read handle to {path} within {timeoutMs}ms.");
    }

  }
}
