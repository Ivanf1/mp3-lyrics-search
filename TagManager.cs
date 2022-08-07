using System.Diagnostics;

namespace mp3_lyrics_service
{
  public class TagManager
  {
    private readonly ILogger<TagManager> _logger;
    public TagManager(ILogger<TagManager> logger)
    {
      _logger = logger;
    }

    public void ManageUpdate(string fileName)
    {
      using (var tfile = GetFileHandle(fileName))
      {
        _logger.LogWarning(tfile.Tag.Lyrics);
        tfile.Dispose();
      }
    }

    // https://stackoverflow.com/a/37154588/13363519
    private TagLib.File GetFileHandle(string path, int timeoutMs = 2000)
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
