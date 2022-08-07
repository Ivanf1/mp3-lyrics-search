using mp3_lyrics_service;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
      options.ServiceName = ".NET lyrics service";
    })
    .ConfigureServices(services =>
    {
      services.AddSingleton<FolderWatcher>();
      services.AddHostedService<Worker>();
    })
    .ConfigureLogging((context, logging) =>
    {
      // See: https://github.com/dotnet/runtime/issues/47303
      logging.AddConfiguration(
          context.Configuration.GetSection("Logging"));
    })
    .Build();

await host.RunAsync();
