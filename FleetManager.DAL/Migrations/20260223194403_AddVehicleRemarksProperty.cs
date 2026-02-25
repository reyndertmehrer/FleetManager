using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetManager.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleRemarksProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "vehicles",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "vehicles");
        }
    }
}
