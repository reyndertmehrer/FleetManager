using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexForLicensePlate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_vehicles_LicensePlate",
                table: "vehicles",
                column: "LicensePlate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_vehicles_LicensePlate",
                table: "vehicles");
        }
    }
}
