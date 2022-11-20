using Microsoft.Extensions.DependencyInjection;

namespace Proxx.Vlad.Tryhub.ConsoleDrawerUi;

public static class DiRegistrations
{
    public static IServiceCollection ConfigureConsoleDrawer(this IServiceCollection services)
    {
        services.AddSingleton<IConsoleDrawer, ConsoleDrawer>();
        
        return services;
    }
}