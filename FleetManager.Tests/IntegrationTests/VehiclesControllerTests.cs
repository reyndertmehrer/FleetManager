using FleetManager.Common.Enums;
using FleetManager.Common.Models;
using FleetManager.Web.Models.Request;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace FleetManager.Tests.IntegrationTests;

public class VehiclesControllerTests : IAsyncLifetime
{
    private FleetManagerWebApplicationFactory _factory = null!;
    private HttpClient _client = null!;

    ValueTask IAsyncLifetime.InitializeAsync()
    {
        _factory = new FleetManagerWebApplicationFactory();
        _client = _factory.CreateClient();

        return ValueTask.CompletedTask;
    }

    [Fact]
    public async Task Create_WithValidVehicleDto_ReturnsCreatedAtAction()
    {
        // Arrange
        var vehicleDto = new VehicleDto
        {
            LicensePlate = "XA1234",
            Color = "Red",
            Year = 2023,
            Status = VehicleStatus.Ordered,
            LeasedTo = null
        };

        // Act
        var response = await _client.PostAsJsonAsync("/vehicles", vehicleDto, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var vehicle = await response.Content.ReadFromJsonAsync<VehicleDto>(cancellationToken: TestContext.Current.CancellationToken);
        vehicle.ShouldNotBeNull();
        vehicle.Id.ShouldBeGreaterThan(0);
        vehicle.LicensePlate.ShouldBe(vehicleDto.LicensePlate);
    }

    [Fact]
    public async Task Create_WithInvalidVehicleDto_ReturnsBadRequest()
    {
        // Arrange 
        var invalidVehicleDto = new VehicleDto
        {
            LicensePlate = "", // Empty license plate
            Color = "Red",
            Year = 2023,
            Status = VehicleStatus.Ordered,
            LeasedTo = null
        };

        // Act
        var response = await _client.PostAsJsonAsync("/vehicles", invalidVehicleDto, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var errorMessage = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        errorMessage.ShouldContain("The LicensePlate field is required.");
    }

    [Fact]
    public async Task UpdateVehicleStatus_WithValidId_ReturnsOk()
    {
        // Arrange 
        await PopulateVehicles();

        var updateRequest = new UpdateVehicleStatusRequest
        {
            Status = VehicleStatus.Leased
        };

        // Act
        var response = await _client.PatchAsJsonAsync("/vehicles/1", updateRequest, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    private async Task PopulateVehicles()
    {
        var response = await _client.PostAsync("/vehicles/populate", null);
        response.EnsureSuccessStatusCode();
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        // TODO: run teardown to delete test data
        _client?.Dispose();
        _factory?.Dispose();

        return ValueTask.CompletedTask;
    }
}
