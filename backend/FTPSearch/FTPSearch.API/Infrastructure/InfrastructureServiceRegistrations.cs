namespace FTPSearch.API.Infrastructure;

public static partial class InfrastructureServiceRegistrations
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
        => services.AddFTPSearchDbContext(configuration)
            .AddConfigurations(configuration)
            .AddServices()
            .AddLibraries();
}