using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS.Migrations
{
    public partial class createflightaccount0404v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flight_Account",
                columns: table => new
                {
                    Flight_AccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightId = table.Column<int>(type: "int", nullable: false),
                    AccountEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight_Account", x => x.Flight_AccountId);
                    table.ForeignKey(
                        name: "FK_Flight_Account_Account_AccountEmail",
                        column: x => x.AccountEmail,
                        principalTable: "Account",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Flight_Account_Flight_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flight",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Account_AccountEmail",
                table: "Flight_Account",
                column: "AccountEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Account_FlightId",
                table: "Flight_Account",
                column: "FlightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flight_Account");
        }
    }
}
