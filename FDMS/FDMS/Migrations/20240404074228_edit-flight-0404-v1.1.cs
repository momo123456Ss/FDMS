using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS.Migrations
{
    public partial class editflight0404v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Flight",
                table: "Flight");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Flight");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flight",
                table: "Flight",
                column: "FlightId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Flight",
                table: "Flight");

            migrationBuilder.AddColumn<string>(
                name: "FlightId",
                table: "Flight",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flight",
                table: "Flight",
                columns: new[] { "FlightId", "FlightId1" });
        }
    }
}
