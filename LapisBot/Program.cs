using LapisBot.ServiceConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
  .ConfigureAppConfiguration((hostingContext, config) =>
  {
      config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
  })
  .ConfigureServices((hostingContext, services) =>
  {
      var configuration = hostingContext.Configuration;

      services.AddDiscordServices();
      services.AddLoggingServices(configuration);
      services.ConfigureDiscordConfiguration(configuration);
  })
  .Build();

await host.RunAsync();
