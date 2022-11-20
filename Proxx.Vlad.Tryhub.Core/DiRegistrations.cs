using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Proxx.Vlad.Tryhub.Core;

public static class DiRegistrations
{
    public static IServiceCollection ConfigureGameDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // singleton scope should be totally sufficient for our case 
        // but we could go with scoped lifetime + scope factory
        
        services.AddSingleton<IBoard, Board>();
        services.AddSingleton<IAdjacentHolesCalculator, AdjacentHolesCalculator>();
        services.AddSingleton<IBlackHolesGenerator, BlackHolesGenerator>();
        services.AddSingleton<IBoardValidator, BoardValidator>();
        services.Configure<GameConfiguration>(configuration.GetSection(nameof(GameConfiguration)));
        
        return services;
    }
}