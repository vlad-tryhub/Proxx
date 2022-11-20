using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Proxx.Vlad.Tryhub;

// use generic host because it provides a lot of features out of the box
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(new Startup().ConfigureServices)
    .ConfigureLogging(builder => builder.ClearProviders()) 
    .Build();

await host.RunAsync();