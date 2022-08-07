namespace mp3_lyrics_service
{
  public class Worker : BackgroundService
  {
    private readonly FolderWatcher _folderWatcher;
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger, FolderWatcher watcher)
    {
      _logger = logger;
      _folderWatcher = watcher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);
      while (!stoppingToken.IsCancellationRequested)
      {
        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}