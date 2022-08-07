namespace mp3_lyrics_service
{
  public class Worker : BackgroundService
  {
    private readonly JokeService jokeService;
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger, JokeService jokeService)
    {
      _logger = logger;
      this.jokeService = jokeService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        string joke = jokeService.GetJoke();
        _logger.LogWarning(joke);
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        await Task.Delay(1000, stoppingToken);
      }
    }
  }
}