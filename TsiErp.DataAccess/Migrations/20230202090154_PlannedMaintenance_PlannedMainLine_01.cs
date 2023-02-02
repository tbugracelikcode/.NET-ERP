using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class PlannedMaintenance_PlannedMainLine_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlannedMaintenances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegistrationNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PeriodID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Status = table.Column<int>(type: "Int", nullable: false),
                    Caregiver = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberofCaregivers = table.Column<int>(type: "Int", nullable: false),
                    RemainingTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Note_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlannedTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OccuredTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
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
                    table.PrimaryKey("PK_PlannedMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannedMaintenances_MaintenancePeriods_PeriodID",
                        column: x => x.PeriodID,
                        principalTable: "MaintenancePeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlannedMaintenances_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlannedMaintenanceLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlannedMaintenanceID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    InstructionDescription = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    MaintenanceNote = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
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
                    table.PrimaryKey("PK_PlannedMaintenanceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlannedMaintenanceLines_PlannedMaintenances_PlannedMaintenanceID",
                        column: x => x.PlannedMaintenanceID,
                        principalTable: "PlannedMaintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlannedMaintenanceLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlannedMaintenanceLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlannedMaintenanceLines_PlannedMaintenanceID",
                table: "PlannedMaintenanceLines",
                column: "PlannedMaintenanceID");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedMaintenanceLines_ProductID",
                table: "PlannedMaintenanceLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedMaintenanceLines_UnitSetID",
                table: "PlannedMaintenanceLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedMaintenances_PeriodID",
                table: "PlannedMaintenances",
                column: "PeriodID");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedMaintenances_RegistrationNo",
                table: "PlannedMaintenances",
                column: "RegistrationNo");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedMaintenances_StationID",
                table: "PlannedMaintenances",
                column: "StationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlannedMaintenanceLines");

            migrationBuilder.DropTable(
                name: "PlannedMaintenances");
        }
    }
}
