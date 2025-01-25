using Carter;
using FluentValidation;
using FTPSearch.API.Application.Services;
using FTPSearch.API.Infrastructure.Configurations;
using FTPSearch.API.Infrastructure.Data.Context;
using FTPSearch.API.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace FTPSearch.API.Infrastructure;

public static partial class InfrastructureServiceRegistrations
{
    private static IServiceCollection AddFTPSearchDbContext(this IServiceCollection services, 
        IConfiguration configuration)
        => services.AddDbContext<FTPSearchDbContext>(options =>
    {
        options.UseNpgsql(configuration.GetConnectionString("DatabaseConnection"));
    });
    
    private static IServiceCollection AddConfigurations(this IServiceCollection services, 
        IConfiguration configuration)
        => services.Configure<FtpConfiguration>(configuration.GetSection("FTPConfiguration"));

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddScoped<IFtpService, FtpService>();

    private static IServiceCollection AddLibraries(this IServiceCollection services)
        => services.AddSwagger()
            .AddMediatRLibrary()
            .AddValidations()
            .AddCarterLibrary();

    private static IServiceCollection AddSwagger(this IServiceCollection services)
        => services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "FTPSearch API", Version = "v1" });
        }).AddEndpointsApiExplorer();

    private static IServiceCollection AddMediatRLibrary(this IServiceCollection services)
        => services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

    private static IServiceCollection AddValidations(this IServiceCollection services)
        => services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    private static IServiceCollection AddCarterLibrary(this IServiceCollection services)
        => services.AddCarter();

    public static void ConfigureLibraries(this WebApplication app)
    {
        ConfigureCarter(app);
        ConfigureSwagger(app);
    }

    private static void ConfigureSwagger(WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;
        app.UseSwagger();
        app.UseSwaggerUI(options => { options.DisplayRequestDuration(); });
    }

    private static void ConfigureCarter(WebApplication app)
    {
        app.MapCarter();
    }
}