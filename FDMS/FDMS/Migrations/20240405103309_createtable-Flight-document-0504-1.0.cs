using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS.Migrations
{
    public partial class createtableFlightdocument050410 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightDocument",
                columns: table => new
                {
                    FlightDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<int>(type: "int", nullable: false),
                    VersionPatch = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FileSize = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FileViewUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightDocument", x => x.FlightDocumentId);
                    table.ForeignKey(
                        name: "FK_FlightDocument_Account_Creator",
                        column: x => x.Creator,
                        principalTable: "Account",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlightDocument_DocumentType_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalTable: "DocumentType",
                        principalColumn: "DocumentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlightDocument_Flight_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flight",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlightDocument_Creator",
                table: "FlightDocument",
                column: "Creator");

            migrationBuilder.CreateIndex(
                name: "IX_FlightDocument_DocumentTypeId",
                table: "FlightDocument",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FlightDocument_FlightId",
                table: "FlightDocument",
                column: "FlightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightDocument");
        }
    }
}
