using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Proxx.Vlad.Tryhub.ConsoleDrawerUi;
using Proxx.Vlad.Tryhub.Core;

namespace Proxx.Vlad.Tryhub;

public class Startup
{
    public void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddHostedService<GameRunner>();

        services.ConfigureGameDependencies(hostContext.Configuration);
        services.ConfigureConsoleDrawer();
    }
}