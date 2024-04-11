using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS.Migrations
{
    public partial class edittabledocgroup090411 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightDocument_GroupPermissions_FlightDocument_FlightDocumentId",
                table: "FlightDocument_GroupPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightDocument_GroupPermissions_GroupPermission_GroupPermissionId",
                table: "FlightDocument_GroupPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlightDocument_GroupPermissions",
                table: "FlightDocument_GroupPermissions");

            migrationBuilder.RenameTable(
                name: "FlightDocument_GroupPermissions",
                newName: "FlightDocument_GroupPermission");

            migrationBuilder.RenameIndex(
                name: "IX_FlightDocument_GroupPermissions_GroupPermissionId",
                table: "FlightDocument_GroupPermission",
                newName: "IX_FlightDocument_GroupPermission_GroupPermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_FlightDocument_GroupPermissions_FlightDocumentId",
                table: "FlightDocument_GroupPermission",
                newName: "IX_FlightDocument_GroupPermission_FlightDocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightDocument_GroupPermission",
                table: "FlightDocument_GroupPermission",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightDocument_GroupPermission_FlightDocument_FlightDocumentId",
                table: "FlightDocument_GroupPermission",
                column: "FlightDocumentId",
                principalTable: "FlightDocument",
                principalColumn: "FlightDocumentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightDocument_GroupPermission_GroupPermission_GroupPermissionId",
                table: "FlightDocument_GroupPermission",
                column: "GroupPermissionId",
                principalTable: "GroupPermission",
                principalColumn: "GroupPermissionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlightDocument_GroupPermission_FlightDocument_FlightDocumentId",
                table: "FlightDocument_GroupPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_FlightDocument_GroupPermission_GroupPermission_GroupPermissionId",
                table: "FlightDocument_GroupPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FlightDocument_GroupPermission",
                table: "FlightDocument_GroupPermission");

            migrationBuilder.RenameTable(
                name: "FlightDocument_GroupPermission",
                newName: "FlightDocument_GroupPermissions");

            migrationBuilder.RenameIndex(
                name: "IX_FlightDocument_GroupPermission_GroupPermissionId",
                table: "FlightDocument_GroupPermissions",
                newName: "IX_FlightDocument_GroupPermissions_GroupPermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_FlightDocument_GroupPermission_FlightDocumentId",
                table: "FlightDocument_GroupPermissions",
                newName: "IX_FlightDocument_GroupPermissions_FlightDocumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlightDocument_GroupPermissions",
                table: "FlightDocument_GroupPermissions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FlightDocument_GroupPermissions_FlightDocument_FlightDocumentId",
                table: "FlightDocument_GroupPermissions",
                column: "FlightDocumentId",
                principalTable: "FlightDocument",
                principalColumn: "FlightDocumentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FlightDocument_GroupPermissions_GroupPermission_GroupPermissionId",
                table: "FlightDocument_GroupPermissions",
                column: "GroupPermissionId",
                principalTable: "GroupPermission",
                principalColumn: "GroupPermissionId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
