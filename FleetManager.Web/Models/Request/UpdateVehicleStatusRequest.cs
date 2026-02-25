using FleetManager.Common.Enums;

namespace FleetManager.Web.Models.Request;

public class UpdateVehicleStatusRequest
{
    public VehicleStatus Status { get; set; }
}
