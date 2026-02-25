using FleetManager.Common.Enums;
using FleetManager.Common.Models;

namespace FleetManager.Services.Interfaces;

public interface IVehicleService
{
    Task<Result<VehicleDto>> AddVehicleAsync(VehicleDto vehicle);
    Task<bool> PopulateVehiclesAsync();
    Task<IReadOnlyCollection<VehicleDto>> GetAsync(string? leasedTo, VehicleStatus? status);
    Task<Result<VehicleDto>> LeaseVehicleAsync(int vehicleId, string leasedTo);
    Task<Result<VehicleDto>> UpdateVehicleStatusAsync(int vehicleId, VehicleStatus status);

}
