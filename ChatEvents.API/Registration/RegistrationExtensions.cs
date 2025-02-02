using ChatEvents.API.Handlers;
using ChatEvents.ConfigurationOptions;
using ChatEvents.Infrastructure.Registration;

namespace ChatEvents.API.Registration;

public static class RegistrationExtensions
{
    public static void RegisterConfigurationAndServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.ConfigurationKey));
        services.RegisterServices();
    }
}