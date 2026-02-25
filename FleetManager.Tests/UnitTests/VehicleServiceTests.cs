using FleetManager.Common.Enums;
using FleetManager.Common.Models;
using FleetManager.DAL;
using FleetManager.DAL.Entities;
using FleetManager.Services.Implementations;
using FleetManager.Services.Mappers;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Shouldly;

namespace FleetManager.Tests.UnitTests;

public class VehicleServiceTests
{
    private readonly Mock<ILogger<VehicleService>> _loggerMock = new();
    private readonly Mock<AppDbContext> _dbContextMock = new();

    private readonly VehicleService _sut;

    public VehicleServiceTests()
    {
        _sut = new VehicleService(_dbContextMock.Object, _loggerMock.Object);
    }

    [Theory]
    [InlineData(VehicleStatus.Sold)]
    [InlineData(VehicleStatus.InMaintenance)]
    [InlineData(VehicleStatus.Leased)]
    [InlineData(VehicleStatus.Ordered)]
    public async Task LeaseVehicleAsync_WhenStatusNotAvailable_ShouldReturnFailedResult(VehicleStatus vehicleStatus)
    {
        // Arrange
        const int vehicleId = 1;
        const string leasedTo = "John Doe";

        var vehicle = new VehicleDto // TODO: create a Faker for this
        {
            Id = vehicleId,
            LicensePlate = "123",
            LeasedTo = leasedTo,
            Color = "Red",
            Year = 1800, // Invalid year
            Status = vehicleStatus
        };

        var entity = vehicle.ToEntity();

        IList<VehicleEntity> existingVehicles = [entity];
        _dbContextMock.Setup(db => db.Vehicles).ReturnsDbSet(existingVehicles);

        // Act
        var result = await _sut.LeaseVehicleAsync(vehicleId, leasedTo);

        // Assert
        result.Success.ShouldBeFalse();
        result.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
        result.Message.ShouldBe($"Cannot lease vehicle {vehicleId}; vehicle is not available.");
    }
}
