using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS.Migrations
{
    public partial class createtableFlightdocumentgroup050410 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightDocument_GroupPermissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightDocumentId = table.Column<int>(type: "int", nullable: false),
                    GroupPermissionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightDocument_GroupPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlightDocument_GroupPermissions_FlightDocument_FlightDocumentId",
                        column: x => x.FlightDocumentId,
                        principalTable: "FlightDocument",
                        principalColumn: "FlightDocumentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightDocument_GroupPermissions_GroupPermission_GroupPermissionId",
                        column: x => x.GroupPermissionId,
                        principalTable: "GroupPermission",
                        principalColumn: "GroupPermissionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightDocument_GroupPermissions_FlightDocumentId",
                table: "FlightDocument_GroupPermissions",
                column: "FlightDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightDocument_GroupPermissions_GroupPermissionId",
                table: "FlightDocument_GroupPermissions",
                column: "GroupPermissionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightDocument_GroupPermissions");
        }
    }
}
