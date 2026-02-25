using FleetManager.Common.Enums;
using FleetManager.Common.Models;
using FleetManager.Services.Interfaces;
using FleetManager.Web.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace FleetManager.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiclesController(IVehicleValidationService vehicleValidationService, IVehicleService vehicleService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleDto>>> Get([FromQuery] string? leasedTo, [FromQuery] VehicleStatus? status)
    {
        var vehicles = await vehicleService.GetAsync(leasedTo, status);
        if (vehicles.Count == 0) return NotFound("No vehicles found that match the given filter.");

        return Ok(vehicles);
    }

    [HttpPost]
    public async Task<IActionResult> Create(VehicleDto vehicle)
    {
        var validationResult = vehicleValidationService.HasValidData(vehicle);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ValidationMessage);
        }

        var result = await vehicleService.AddVehicleAsync(vehicle);
        if (!result.Success)
        {
            return StatusCode((int)result.StatusCode, result.Message);
        }

        return CreatedAtAction(nameof(Get), new { licensePlate = vehicle.LicensePlate }, result.Data);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateVehicleStatus(int id, [FromBody] UpdateVehicleStatusRequest request)
    {
        var result = await vehicleService.UpdateVehicleStatusAsync(id, request.Status);

        return StatusCode((int)result.StatusCode, result.Message);
    }

    [HttpPost("{id:int}/lease")]
    public async Task<IActionResult> LeaseVehicle(int id, [FromBody] LeaseVehicleRequest request)
    {
        var result = await vehicleService.LeaseVehicleAsync(id, request.LeasedTo);

        return StatusCode((int)result.StatusCode, result.Message);
    }

    [HttpPost("populate")]
    public async Task<IActionResult> PopulateVehicles()
    {
        var succeeded = await vehicleService.PopulateVehiclesAsync();
        if (!succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while populating vehicles.");
        }

        return Ok();
    }
}
