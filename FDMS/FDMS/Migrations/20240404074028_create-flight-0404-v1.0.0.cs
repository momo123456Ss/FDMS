﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FDMS.Migrations
{
    public partial class createflight0404v100 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    FlightId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FlightId1 = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Route = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PointOfLoading = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PointOfUnLoading = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsConfirm = table.Column<bool>(type: "bit", nullable: true),
                    AccountConfirm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flight", x => new { x.FlightId, x.FlightId1 });
                    table.ForeignKey(
                        name: "FK_Flight_Account_AccountConfirm",
                        column: x => x.AccountConfirm,
                        principalTable: "Account",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_AccountConfirm",
                table: "Flight",
                column: "AccountConfirm");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flight");
        }
    }
}
