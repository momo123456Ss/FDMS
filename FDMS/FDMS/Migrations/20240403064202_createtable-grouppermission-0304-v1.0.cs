using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS.Migrations
{
    public partial class createtablegrouppermission0304v10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupPermission",
                columns: table => new
                {
                    GroupPermissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalMembers = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Creator = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPermission", x => x.GroupPermissionId);
                    table.ForeignKey(
                        name: "FK_GroupPermission_Account_Creator",
                        column: x => x.Creator,
                        principalTable: "Account",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Account_GroupPermission",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupPermissionId = table.Column<int>(type: "int", nullable: false),
                    AccountEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account_GroupPermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_GroupPermission_Account_AccountEmail",
                        column: x => x.AccountEmail,
                        principalTable: "Account",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Account_GroupPermission_GroupPermission_GroupPermissionId",
                        column: x => x.GroupPermissionId,
                        principalTable: "GroupPermission",
                        principalColumn: "GroupPermissionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_GroupPermission_AccountEmail",
                table: "Account_GroupPermission",
                column: "AccountEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Account_GroupPermission_GroupPermissionId",
                table: "Account_GroupPermission",
                column: "GroupPermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermission_Creator",
                table: "GroupPermission",
                column: "Creator");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account_GroupPermission");

            migrationBuilder.DropTable(
                name: "GroupPermission");
        }
    }
}
