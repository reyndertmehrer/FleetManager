using FleetManager.Services.Implementations;
using FleetManager.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FleetManager.Services;

public static class RegistrationExtensions
{
    public static void RegisterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IVehicleValidationService, VehicleValidationService>();
        serviceCollection.AddScoped<IVehicleService, VehicleService>();
    }
}
