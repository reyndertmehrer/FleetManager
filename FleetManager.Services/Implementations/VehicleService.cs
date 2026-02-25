using FleetManager.Common.Enums;
using FleetManager.Common.Models;
using FleetManager.DAL;
using FleetManager.DAL.Entities;
using FleetManager.Services.Interfaces;
using FleetManager.Services.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace FleetManager.Services.Implementations;

public class VehicleService(AppDbContext dbContext, ILogger<VehicleService> logger) : IVehicleService
{
    private const int PopulationThreshold = 100;

    public async Task<Result<VehicleDto>> AddVehicleAsync(VehicleDto vehicle)
    {
        try
        {
            var entity = vehicle.ToEntity();

            await dbContext.Vehicles.AddAsync(entity);
            await dbContext.SaveChangesAsync();

            return GetSuccessResult(HttpStatusCode.Created, entity.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error occurred trying to add vehicle with id {LicensePlate}: {ExceptionMessage}", vehicle.LicensePlate, ex.Message);
        }

        return GetFailedResult<VehicleDto>(HttpStatusCode.InternalServerError, "An error occurred trying to add the vehicle.");
    }

    public async Task<Result<VehicleDto>> UpdateVehicleStatusAsync(int vehicleId, VehicleStatus status)
    {
        var entity = await dbContext.Vehicles.SingleOrDefaultAsync(v => v.Id == vehicleId);
        if (entity is null)
        {
            return GetFailedResult<VehicleDto>(HttpStatusCode.NotFound, $"Vehicle not found.");
        }

        if (entity.Status is VehicleStatus.Sold)
        {
            return GetFailedResult<VehicleDto>(HttpStatusCode.BadRequest, $"Cannot update status of vehicle {vehicleId}; vehicle already sold.");
        }

        if (entity.Status == status)
        {
            return GetSuccessResult(HttpStatusCode.NoContent, entity.ToDto());
        }

        try
        {
            entity.Status = status;

            await dbContext.SaveChangesAsync();

            return GetSuccessResult(HttpStatusCode.NoContent, entity.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error occurred trying to update status of vehicle with id {VehicleId}: {ExceptionMessage}", vehicleId, ex.Message);
        }

        return GetFailedResult<VehicleDto>(HttpStatusCode.InternalServerError, "An error occurred trying to update the status of the vehicle.");
    }

    public async Task<IReadOnlyCollection<VehicleDto>> GetAsync(string? leasedTo, VehicleStatus? status)
    {
        // TODO: add index on leasedto and status for better performance
        var query = dbContext.Vehicles.AsQueryable();
        if (!string.IsNullOrEmpty(leasedTo))
        {
            query = query.Where(v => v.LeasedTo == leasedTo);
        }

        if (status.HasValue)
        {
            query = query.Where(v => v.Status == status.Value);
        }

        var entities = await query.ToArrayAsync();
        if (entities.Length == 0) return [];

        return entities.Select(e => e.ToDto()).ToArray();
    }

    public async Task<Result<VehicleDto>> LeaseVehicleAsync(int vehicleId, string leasedTo)
    {
        var entity = await dbContext.Vehicles.SingleOrDefaultAsync(v => v.Id == vehicleId);
        if (entity is null)
        {
            return GetFailedResult<VehicleDto>(HttpStatusCode.NotFound, $"Vehicle not found.");
        }

        if (entity.Status is not VehicleStatus.Available)
        {
            return GetFailedResult<VehicleDto>(HttpStatusCode.BadRequest, $"Cannot lease vehicle {vehicleId}; vehicle is not available.");
        }

        try
        {
            entity.Status = VehicleStatus.Leased;
            entity.LeasedTo = leasedTo;
            await dbContext.SaveChangesAsync();

            return GetSuccessResult(HttpStatusCode.NoContent, entity.ToDto());
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error occurred trying to lease vehicle with id {VehicleId}: {ExceptionMessage}", vehicleId, ex.Message);
        }

        return GetFailedResult<VehicleDto>(HttpStatusCode.InternalServerError, "An error occurred trying to lease the vehicle.");
    }

    public async Task<bool> PopulateVehiclesAsync()
    {
        try
        {
            var vehicleCount = await dbContext.Vehicles.CountAsync();
            if (vehicleCount >= PopulationThreshold) return true;

            var vehiclesToAdd = new VehicleEntity[PopulationThreshold];
            for (int i = 0; i < PopulationThreshold; i++)
            {
                vehiclesToAdd[i] = GetDummyVehicleEntity(i);
            }

            await dbContext.Vehicles.AddRangeAsync(vehiclesToAdd);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error occurred trying to populate vehicles: {ExceptionMessage}", ex.Message);
        }

        return true;
    }

    private static VehicleEntity GetDummyVehicleEntity(int index)
    {
        return new VehicleEntity
        {
            Id = 0,
            LicensePlate = $"Populated-{index}",
            Color = "Red",
            Year = 2020,
            Status = Common.Enums.VehicleStatus.Available,
            LeasedTo = null,
            Remarks = "This vehicle was added during database population."
        };
    }

    private static Result<T> GetFailedResult<T>(HttpStatusCode statusCode, string? message = null) where T : class?
    {
        return new Result<T>
        {
            Success = false,
            Message = message,
            StatusCode = statusCode,
            Data = null
        };
    }

    private static Result<T> GetSuccessResult<T>(HttpStatusCode statusCode, T? data, string? message = null) where T : class?
    {
        return new Result<T>
        {
            Success = true,
            Message = message,
            StatusCode = statusCode,
            Data = data
        };
    }
}
