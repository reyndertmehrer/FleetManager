using System.ComponentModel.DataAnnotations;

namespace FleetManager.Web.Models.Request;

public class LeaseVehicleRequest
{
    [Required(AllowEmptyStrings = false)]
    public required string LeasedTo { get; set; }
}
