using FleetManager.Common.Enums;
using FleetManager.Common.Models;
using FleetManager.Services.Implementations;
using FleetManager.Services.Interfaces;
using Shouldly;

namespace FleetManager.Tests.UnitTests;

public class VehicleValidationServiceTests
{
    private readonly IVehicleValidationService _sut;

    public VehicleValidationServiceTests()
    {
        _sut = new VehicleValidationService();
    }

    [Fact]
    public void HasValidData_WhenVehicleTooOld_ShouldReturnInvalidResult()
    {
        // Arrange
        var vehicle = new VehicleDto // TODO: create a Faker for this
        {
            Id = 1,
            LicensePlate = "123",
            LeasedTo = "John Doe",
            Color = "Red",
            Year = 1800, // Invalid year
            Status = VehicleStatus.Available
        };

        // Act
        var result = _sut.HasValidData(vehicle);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.ValidationMessage.ShouldBe("Year must be greater than or equal to 1886.");
    }
}
