using FleetManager.Common.Models;
namespace FleetManager.Services.Interfaces;

public interface IVehicleValidationService
{
    ValidationResult HasValidData(VehicleDto vehicle);
}
