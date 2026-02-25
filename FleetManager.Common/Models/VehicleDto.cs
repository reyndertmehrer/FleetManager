using FleetManager.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace FleetManager.Common.Models;

/// <summary>
/// Represents a data transfer object that encapsulates the essential information about a vehicle.
/// </summary>
public sealed class VehicleDto
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(20)]
    public required string LicensePlate { get; set; }

    [Required(AllowEmptyStrings = false)]
    [MaxLength(20)]
    public required string Color { get; set; }

    public required int Year { get; set; }

    public required VehicleStatus Status { get; set; }

    [MaxLength(100)]
    public string? LeasedTo { get; set; }

    [MaxLength(500)]
    public string? Remarks { get; set; }
}
