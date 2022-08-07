using System.Diagnostics;
using System.Text.Json;

namespace mp3_lyrics_service
{
  public class TagManager
  {
    private readonly ILogger<TagManager> _logger;
    public TagManager(ILogger<TagManager> logger)
    {
      _logger = logger;
    }

    public void ManageUpdate(string filePath)
    {
      using (var tfile = GetFileHandle(filePath))
      {
        _logger.LogWarning(tfile.Tag.Lyrics);

        string fileName = filePath.Split('\\').Last().Trim().Replace(".mp3", "");
        Song song = new(fileName, tfile.Tag.Lyrics);
        string jsonString = JsonSerializer.Serialize(song);
        _logger.LogWarning(jsonString);

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
