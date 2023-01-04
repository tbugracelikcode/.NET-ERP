using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Prod_Tracking01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AdjustmentTime",
                table: "ProductionTrackings",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "ProductionTrackings",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsManagement",
                table: "HaltReasons",
                type: "Bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ContractProductionTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProducedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OperationTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OperationStartDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    OperationEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HaltTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    IsFinished = table.Column<bool>(type: "Bit", nullable: false),
                    PlannedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    StationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShiftCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ShiftID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_ContractProductionTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractProductionTrackings_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContractProductionTrackings_Shifts_ShiftID",
                        column: x => x.ShiftID,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContractProductionTrackings_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ContractProductionTrackings_WorkOrders_WorkOrderID",
                        column: x => x.WorkOrderID,
                        principalTable: "WorkOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionTrackingHaltLines_ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines",
                column: "ContractProductionTrackingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractProductionTrackings_EmployeeID",
                table: "ContractProductionTrackings",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_ContractProductionTrackings_ShiftID",
                table: "ContractProductionTrackings",
                column: "ShiftID");

            migrationBuilder.CreateIndex(
                name: "IX_ContractProductionTrackings_StationID",
                table: "ContractProductionTrackings",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_ContractProductionTrackings_WorkOrderID",
                table: "ContractProductionTrackings",
                column: "WorkOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionTrackingHaltLines_ContractProductionTrackings_ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines",
                column: "ContractProductionTrackingsId",
                principalTable: "ContractProductionTrackings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionTrackingHaltLines_ContractProductionTrackings_ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines");

            migrationBuilder.DropTable(
                name: "ContractProductionTrackings");

            migrationBuilder.DropIndex(
                name: "IX_ProductionTrackingHaltLines_ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines");

            migrationBuilder.DropColumn(
                name: "AdjustmentTime",
                table: "ProductionTrackings");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "ProductionTrackings");

            migrationBuilder.DropColumn(
                name: "ContractProductionTrackingsId",
                table: "ProductionTrackingHaltLines");

            migrationBuilder.DropColumn(
                name: "IsManagement",
                table: "HaltReasons");
        }
    }
}
