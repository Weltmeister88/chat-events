using ChatEvents.Infrastructure.Database;
using ChatEvents.Infrastructure.Repositories;
using ChatEvents.Infrastructure.Services;
using ChatEvents.Repositories;
using ChatEvents.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ChatEvents.Infrastructure.Registration;

public static class RegistrationExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddSingleton<IDbInitializer, DbInitializer>();
        services.AddScoped<IChatRoomsRepository, ChatRoomsRepository>();
        services.AddScoped<IChatRoomsService, ChatRoomsService>();
        services.AddScoped<IChatEventsRepository, ChatEventsRepository>();
        services.AddScoped<IChatEventsService, ChatEventsService>();
    }
}