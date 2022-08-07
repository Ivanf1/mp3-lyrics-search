namespace mp3_lyrics_service
{
  public class FolderWatcher
  {
    private readonly ILogger<FolderWatcher> _logger;
    private readonly FileSystemWatcher watcher;

    public FolderWatcher(ILogger<FolderWatcher> logger)
    {
      _logger = logger;
      watcher = new FileSystemWatcher(@"D:\Musica\Mp3TagCsharp")
      {
        NotifyFilter = NotifyFilters.Attributes
                          | NotifyFilters.CreationTime
                          | NotifyFilters.DirectoryName
                          | NotifyFilters.FileName
                          | NotifyFilters.LastAccess
                          | NotifyFilters.LastWrite
                          | NotifyFilters.Security
                          | NotifyFilters.Size
      };

      watcher.Changed += OnChanged;
      watcher.Created += OnCreated;
      watcher.Deleted += OnDeleted;
      watcher.Renamed += OnRenamed;
      watcher.Error += OnError;

      watcher.Filter = "*.txt";
      watcher.IncludeSubdirectories = true;
      watcher.EnableRaisingEvents = true;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
      if (e.ChangeType != WatcherChangeTypes.Changed)
      {
        return;
      }
      _logger.LogWarning($"Changed: {e.FullPath}");
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
      string value = $"Created: {e.FullPath}";
      _logger.LogWarning(value);
    }

    private void OnDeleted(object sender, FileSystemEventArgs e) =>
        _logger.LogWarning($"Deleted: {e.FullPath}");

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
      _logger.LogWarning($"Renamed:");
      _logger.LogWarning($"    Old: {e.OldFullPath}");
      _logger.LogWarning($"    New: {e.FullPath}");
    }

    private void OnError(object sender, ErrorEventArgs e) =>
        PrintException(e.GetException());

    private void PrintException(Exception? ex)
    {
      if (ex != null)
      {
        _logger.LogWarning($"Message: {ex.Message}");
        _logger.LogWarning("Stacktrace:");
        _logger.LogWarning(ex.StackTrace);
        PrintException(ex.InnerException);
      }
    }

  }

}
