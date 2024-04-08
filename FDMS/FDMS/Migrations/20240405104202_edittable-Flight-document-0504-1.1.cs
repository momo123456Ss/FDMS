using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS.Migrations
{
    public partial class edittableFlightdocument050411 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FlightDocumentIdFK",
                table: "FlightDocument",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FlightDocument_FlightDocumentIdFK",
                table: "FlightDocument",
                column: "FlightDocumentIdFK");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightDocument_FlightDocument_FlightDocumentIdFK",
                table: "FlightDocument",
                column: "FlightDocumentIdFK",
                principalTable: "FlightDocument",
                principalColumn: "FlightDocumentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightDocument_FlightDocument_FlightDocumentIdFK",
                table: "FlightDocument");

            migrationBuilder.DropIndex(
                name: "IX_FlightDocument_FlightDocumentIdFK",
                table: "FlightDocument");

            migrationBuilder.DropColumn(
                name: "FlightDocumentIdFK",
                table: "FlightDocument");
        }
    }
}
