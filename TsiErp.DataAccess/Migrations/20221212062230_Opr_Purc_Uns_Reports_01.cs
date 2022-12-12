using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Opr_Purc_Uns_Reports_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperationUnsuitabilityReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUnsuitabilityWorkOrder = table.Column<bool>(type: "Bit", nullable: false),
                    ControlFormDeclaration = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    IsScrap = table.Column<bool>(type: "Bit", nullable: false),
                    IsCorrection = table.Column<bool>(type: "Bit", nullable: false),
                    IsToBeUsedAs = table.Column<bool>(type: "Bit", nullable: false),
                    WorkOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationGroupID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    EmployeeID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    OperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_OperationUnsuitabilityReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_ProductsOperations_OperationID",
                        column: x => x.OperationID,
                        principalTable: "ProductsOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_StationGroups_StationGroupID",
                        column: x => x.StationGroupID,
                        principalTable: "StationGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationUnsuitabilityReports_WorkOrders_WorkOrderID",
                        column: x => x.WorkOrderID,
                        principalTable: "WorkOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseUnsuitabilityReports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    PartyNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    UnsuitableAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUnsuitabilityWorkOrder = table.Column<bool>(type: "Bit", nullable: false),
                    IsReject = table.Column<bool>(type: "Bit", nullable: false),
                    IsCorrection = table.Column<bool>(type: "Bit", nullable: false),
                    IsToBeUsedAs = table.Column<bool>(type: "Bit", nullable: false),
                    IsContactSupplier = table.Column<bool>(type: "Bit", nullable: false),
                    OrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_PurchaseUnsuitabilityReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseUnsuitabilityReports_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseUnsuitabilityReports_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseUnsuitabilityReports_PurchaseOrders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_EmployeeID",
                table: "OperationUnsuitabilityReports",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_FicheNo",
                table: "OperationUnsuitabilityReports",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_OperationID",
                table: "OperationUnsuitabilityReports",
                column: "OperationID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_ProductID",
                table: "OperationUnsuitabilityReports",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_ProductionOrderID",
                table: "OperationUnsuitabilityReports",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_StationGroupID",
                table: "OperationUnsuitabilityReports",
                column: "StationGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_StationID",
                table: "OperationUnsuitabilityReports",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_WorkOrderID",
                table: "OperationUnsuitabilityReports",
                column: "WorkOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnsuitabilityReports_CurrentAccountCardID",
                table: "PurchaseUnsuitabilityReports",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnsuitabilityReports_FicheNo",
                table: "PurchaseUnsuitabilityReports",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnsuitabilityReports_OrderID",
                table: "PurchaseUnsuitabilityReports",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnsuitabilityReports_ProductID",
                table: "PurchaseUnsuitabilityReports",
                column: "ProductID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationUnsuitabilityReports");

            migrationBuilder.DropTable(
                name: "PurchaseUnsuitabilityReports");
        }
    }
}
