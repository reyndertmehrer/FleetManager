using FleetManager.Common.Enums;
using FleetManager.Common.Models;
using FleetManager.Services.Interfaces;

namespace FleetManager.Services.Implementations;

public sealed class VehicleValidationService() : IVehicleValidationService
{
    private const int MinimumYear = 1886; // The year the first automobile was invented. TODO: move to const class

    public ValidationResult HasValidData(VehicleDto vehicle)
    {
        if (vehicle.Year < MinimumYear)
        {
            return GetInvalidResult($"{nameof(vehicle.Year)} must be greater than or equal to {MinimumYear}.");
        }

        if (vehicle.LeasedTo is not null && vehicle.LeasedTo.Length == 0)
        {
            return GetInvalidResult($"When provided, {nameof(vehicle.LeasedTo)} cannot be an empty string.");
        }

        var canParseStatus = Enum.TryParse<VehicleStatus>(vehicle.Status.ToString(), out var statusResult);
        if (!canParseStatus || !Enum.IsDefined(statusResult))
        {
            return GetInvalidResult($"Invalid {nameof(vehicle.Status)} of vehicle.");
        }

        if (vehicle.Status is VehicleStatus.Leased && string.IsNullOrWhiteSpace(vehicle.LeasedTo))
        {
            return GetInvalidResult($"When {nameof(vehicle.Status)} is {VehicleStatus.Leased}, {nameof(vehicle.LeasedTo)} must be provided and cannot be empty.");
        }

        if (vehicle.Status is not VehicleStatus.Leased && !string.IsNullOrEmpty(vehicle.LeasedTo))
        {
            return GetInvalidResult($"When {nameof(vehicle.LeasedTo)} is provided, {nameof(vehicle.Status)} must be {VehicleStatus.Leased}.");
        }

        return GetValidResult();
    }

    private static ValidationResult GetInvalidResult(string message) => new(false, message);

    private static ValidationResult GetValidResult() => new(true);
}
