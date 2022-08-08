using System.Runtime.Caching;

namespace mp3_lyrics_service
{
  public class FolderWatcher
  {
    private readonly ILogger<FolderWatcher> _logger;
    private readonly FolderWatcherOptions _options;
    private readonly TagManager _tagManager;
    private readonly FileSystemWatcher _watcher;

    /*
     * OnChanged raises more than once per file change. We use a cache so 
     * we can perform the desired operation only once even if the event gets
     * raised more than once.
     * https://failingfast.io/a-robust-solution-for-filesystemwatcher-firing-events-multiple-times/
     */
    private readonly MemoryCache _memCache;
    private readonly CacheItemPolicy _cacheItemPolicy;
    private const int CacheTimeMilliseconds = 100;

    public FolderWatcher(ILogger<FolderWatcher> logger, TagManager tagManager, FolderWatcherOptions options)
    {
      _logger = logger;
      _tagManager = tagManager;
      _options = options;

      _memCache = MemoryCache.Default;
      _cacheItemPolicy = new CacheItemPolicy()
      {
        RemovedCallback = OnRemovedFromCache
      };

      _watcher = new FileSystemWatcher(_options.FolderPath)
      {
        NotifyFilter = NotifyFilters.LastWrite
      };

      _watcher.Changed += OnChanged;

      _watcher.Filter = "*.mp3";
      _watcher.IncludeSubdirectories = true;
      _watcher.EnableRaisingEvents = true;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
      if (e.ChangeType != WatcherChangeTypes.Changed)
      {
        return;
      }
      _cacheItemPolicy.AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(CacheTimeMilliseconds);

      // Only add if it is not there already (swallow others)
      _memCache.AddOrGetExisting(e.Name, e, _cacheItemPolicy);
    }

    // Handle cache item expiring
    private void OnRemovedFromCache(CacheEntryRemovedArguments args)
    {
      if (args.RemovedReason != CacheEntryRemovedReason.Expired) return;

      // Now actually handle file event
      var e = (FileSystemEventArgs)args.CacheItem.Value;
      _logger.LogWarning($"Changed: {e.FullPath}");
      _tagManager.ManageUpdate(e.FullPath);
    }

  }

}
