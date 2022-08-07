namespace mp3_lyrics_service
{
  public class Worker : BackgroundService
  {
    private readonly FolderWatcher folderWatcher;
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger, FolderWatcher watcher)
    {
      _logger = logger;
      folderWatcher = watcher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}