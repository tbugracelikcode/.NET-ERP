using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Work_Orders_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Products_ProductsId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ProductsId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "Routes");

            migrationBuilder.AlterColumn<string>(
                name: "ProductionPoolDescription",
                table: "RouteLines",
                type: "NVarChar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<decimal>(
                name: "OperationTime",
                table: "RouteLines",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuName = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: false),
                    ParentMenuId = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Cancel_ = table.Column<bool>(type: "Bit", nullable: false),
                    ProductionOrderState = table.Column<int>(type: "Int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PlannedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ProducedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    CustomerOrderNo = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    OrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    OrderLineID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    FinishedProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LinkedProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    BOMID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductTreeID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductTreeLineID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PropositionID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PropositionLineID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LinkedProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_ProductionOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionOrders_BillsofMaterials_BOMID",
                        column: x => x.BOMID,
                        principalTable: "BillsofMaterials",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_CurrentAccountCards_CurrentAccountID",
                        column: x => x.CurrentAccountID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Products_FinishedProductID",
                        column: x => x.FinishedProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_SalesOrderLines_OrderLineID",
                        column: x => x.OrderLineID,
                        principalTable: "SalesOrderLines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_SalesOrders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "SalesOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_SalesPropositionLines_PropositionLineID",
                        column: x => x.PropositionLineID,
                        principalTable: "SalesPropositionLines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_SalesPropositions_PropositionID",
                        column: x => x.PropositionID,
                        principalTable: "SalesPropositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductionOrders_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    WorkOrderNo = table.Column<string>(type: "NVarChar(200)", maxLength: 200, nullable: true),
                    IsCancel = table.Column<bool>(type: "Bit", nullable: false),
                    WorkOrderState = table.Column<int>(type: "Int", nullable: false),
                    AdjustmentAndControlTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OperationTime = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OccuredStartDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    OccuredFinishDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PlannedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ProducedQuantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    LinkedWorkOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
                    ProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PropositionID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    RouteID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductsOperationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    StationGroupID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    table.PrimaryKey("PK_WorkOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkOrders_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_ProductsOperations_ProductsOperationID",
                        column: x => x.ProductsOperationID,
                        principalTable: "ProductsOperations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_Routes_RouteID",
                        column: x => x.RouteID,
                        principalTable: "Routes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_SalesPropositions_PropositionID",
                        column: x => x.PropositionID,
                        principalTable: "SalesPropositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_StationGroups_StationGroupID",
                        column: x => x.StationGroupID,
                        principalTable: "StationGroups",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkOrders_Stations_StationID",
                        column: x => x.StationID,
                        principalTable: "Stations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ProductID",
                table: "Routes",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_BOMID",
                table: "ProductionOrders",
                column: "BOMID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_CurrentAccountID",
                table: "ProductionOrders",
                column: "CurrentAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_FicheNo",
                table: "ProductionOrders",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_FinishedProductID",
                table: "ProductionOrders",
                column: "FinishedProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_OrderID",
                table: "ProductionOrders",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_OrderLineID",
                table: "ProductionOrders",
                column: "OrderLineID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_PropositionID",
                table: "ProductionOrders",
                column: "PropositionID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_PropositionLineID",
                table: "ProductionOrders",
                column: "PropositionLineID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_RouteID",
                table: "ProductionOrders",
                column: "RouteID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionOrders_UnitSetID",
                table: "ProductionOrders",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_Code",
                table: "WorkOrders",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_CurrentAccountCardID",
                table: "WorkOrders",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductID",
                table: "WorkOrders",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductionOrderID",
                table: "WorkOrders",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_ProductsOperationID",
                table: "WorkOrders",
                column: "ProductsOperationID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_PropositionID",
                table: "WorkOrders",
                column: "PropositionID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_RouteID",
                table: "WorkOrders",
                column: "RouteID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_StationGroupID",
                table: "WorkOrders",
                column: "StationGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkOrders_StationID",
                table: "WorkOrders",
                column: "StationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Products_ProductID",
                table: "Routes",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Products_ProductID",
                table: "Routes");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "WorkOrders");

            migrationBuilder.DropTable(
                name: "ProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ProductID",
                table: "Routes");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductionPoolDescription",
                table: "RouteLines",
                type: "NVarChar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVarChar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OperationTime",
                table: "RouteLines",
                type: "Int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ProductsId",
                table: "Routes",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Products_ProductsId",
                table: "Routes",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
