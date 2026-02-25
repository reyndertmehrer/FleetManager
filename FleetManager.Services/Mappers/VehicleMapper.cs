using FleetManager.Common.Models;
using FleetManager.DAL.Entities;

namespace FleetManager.Services.Mappers;

public static class VehicleMapper
{
    public static VehicleDto ToDto(this VehicleEntity entity)
    {
        return new VehicleDto
        {
            Id = entity.Id,
            LicensePlate = entity.LicensePlate,
            Color = entity.Color,
            Year = entity.Year,
            Status = entity.Status,
            LeasedTo = entity.LeasedTo,
            Remarks = entity.Remarks
        };
    }

    public static VehicleEntity ToEntity(this VehicleDto dto)
    {
        return new VehicleEntity
        {
            Id = dto.Id,
            LicensePlate = dto.LicensePlate,
            Color = dto.Color,
            Year = dto.Year,
            Status = dto.Status,
            LeasedTo = dto.LeasedTo,
            Remarks = dto.Remarks
        };
    }
}
