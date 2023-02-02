using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class UnplannedMaintenance_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnplannedMaintenances",
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
                    PeriodTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Note_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnplannedTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OccuredTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    UnplannedDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "DateTime", nullable: true),
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
                    table.PrimaryKey("PK_UnplannedMaintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnplannedMaintenances_MaintenancePeriods_PeriodID",
                        column: x => x.PeriodID,
                        principalTable: "MaintenancePeriods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UnplannedMaintenances_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UnplannedMaintenanceLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnplannedMaintenanceID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_UnplannedMaintenanceLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnplannedMaintenanceLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UnplannedMaintenanceLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UnplannedMaintenanceLines_UnplannedMaintenances_UnplannedMaintenanceID",
                        column: x => x.UnplannedMaintenanceID,
                        principalTable: "UnplannedMaintenances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnplannedMaintenanceLines_ProductID",
                table: "UnplannedMaintenanceLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_UnplannedMaintenanceLines_UnitSetID",
                table: "UnplannedMaintenanceLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_UnplannedMaintenanceLines_UnplannedMaintenanceID",
                table: "UnplannedMaintenanceLines",
                column: "UnplannedMaintenanceID");

            migrationBuilder.CreateIndex(
                name: "IX_UnplannedMaintenances_PeriodID",
                table: "UnplannedMaintenances",
                column: "PeriodID");

            migrationBuilder.CreateIndex(
                name: "IX_UnplannedMaintenances_RegistrationNo",
                table: "UnplannedMaintenances",
                column: "RegistrationNo");

            migrationBuilder.CreateIndex(
                name: "IX_UnplannedMaintenances_StationID",
                table: "UnplannedMaintenances",
                column: "StationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnplannedMaintenanceLines");

            migrationBuilder.DropTable(
                name: "UnplannedMaintenances");
        }
    }
}
