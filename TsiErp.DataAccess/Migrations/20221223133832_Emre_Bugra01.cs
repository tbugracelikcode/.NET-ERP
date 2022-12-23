using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Emre_Bugra01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HaltReasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "NVarChar(50)", maxLength: 50, nullable: false),
                    IsPlanned = table.Column<bool>(type: "Bit", nullable: false),
                    IsMachine = table.Column<bool>(type: "Bit", nullable: false),
                    IsOperator = table.Column<bool>(type: "Bit", nullable: false),
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
                    table.PrimaryKey("PK_HaltReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionTrackings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProducedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OperationTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OperationStartDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    OperationEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HaltTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
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
                    table.PrimaryKey("PK_ProductionTrackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionTrackings_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionTrackings_Shifts_ShiftID",
                        column: x => x.ShiftID,
                        principalTable: "Shifts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionTrackings_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionTrackings_WorkOrders_WorkOrderID",
                        column: x => x.WorkOrderID,
                        principalTable: "WorkOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductionTrackingHaltLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionTrackingID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    HaltID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    HaltCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HaltName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HaltTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
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
                    table.PrimaryKey("PK_ProductionTrackingHaltLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionTrackingHaltLines_HaltReasons_HaltID",
                        column: x => x.HaltID,
                        principalTable: "HaltReasons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionTrackingHaltLines_ProductionTrackings_ProductionTrackingID",
                        column: x => x.ProductionTrackingID,
                        principalTable: "ProductionTrackings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionTrackingHaltLines_HaltID",
                table: "ProductionTrackingHaltLines",
                column: "HaltID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionTrackingHaltLines_ProductionTrackingID",
                table: "ProductionTrackingHaltLines",
                column: "ProductionTrackingID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionTrackings_EmployeeID",
                table: "ProductionTrackings",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionTrackings_ShiftID",
                table: "ProductionTrackings",
                column: "ShiftID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionTrackings_StationID",
                table: "ProductionTrackings",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionTrackings_WorkOrderID",
                table: "ProductionTrackings",
                column: "WorkOrderID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionTrackingHaltLines");

            migrationBuilder.DropTable(
                name: "HaltReasons");

            migrationBuilder.DropTable(
                name: "ProductionTrackings");
        }
    }
}
