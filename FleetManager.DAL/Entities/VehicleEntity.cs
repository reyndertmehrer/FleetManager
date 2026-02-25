using FleetManager.Common.Enums;

namespace FleetManager.DAL.Entities;

public class VehicleEntity
{
    public required int Id { get; set; }
    public required string LicensePlate { get; set; }
    public required string Color { get; set; } = null!;
    public required int Year { get; set; }
    public required VehicleStatus Status { get; set; }
    public string? LeasedTo { get; set; }
    public string? Remarks { get; set; }
}
