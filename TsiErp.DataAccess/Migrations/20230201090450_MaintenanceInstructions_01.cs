using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class MaintenanceInstructions_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaintenanceInstructions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    InstructionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PeriodID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PeriodTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PlannedMaintenanceTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Note_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceInstructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceInstructions_MaintenancePeriods_PeriodID",
                        column: x => x.PeriodID,
                        principalTable: "MaintenancePeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceInstructions_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MaintenanceInstructionLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InstructionID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    InstructionDescription = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceInstructionLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaintenanceInstructionLines_MaintenanceInstructions_InstructionID",
                        column: x => x.InstructionID,
                        principalTable: "MaintenanceInstructions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaintenanceInstructionLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaintenanceInstructionLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceInstructionLines_InstructionID",
                table: "MaintenanceInstructionLines",
                column: "InstructionID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceInstructionLines_ProductID",
                table: "MaintenanceInstructionLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceInstructionLines_UnitSetID",
                table: "MaintenanceInstructionLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceInstructions_Code",
                table: "MaintenanceInstructions",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceInstructions_PeriodID",
                table: "MaintenanceInstructions",
                column: "PeriodID");

            migrationBuilder.CreateIndex(
                name: "IX_MaintenanceInstructions_StationID",
                table: "MaintenanceInstructions",
                column: "StationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaintenanceInstructionLines");

            migrationBuilder.DropTable(
                name: "MaintenanceInstructions");
        }
    }
}
