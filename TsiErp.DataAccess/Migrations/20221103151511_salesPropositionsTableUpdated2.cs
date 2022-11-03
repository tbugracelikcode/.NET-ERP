using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class salesPropositionsTableUpdated2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_Branches_BranchID",
                table: "SalesPropositionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_Warehouses_WarehouseID",
                table: "SalesPropositionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftLines_Shifts_ShiftID",
                table: "ShiftLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositionLines_BranchID",
                table: "SalesPropositionLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositionLines_WarehouseID",
                table: "SalesPropositionLines");

            migrationBuilder.DropColumn(
                name: "BranchID",
                table: "SalesPropositionLines");

            migrationBuilder.DropColumn(
                name: "WarehouseID",
                table: "SalesPropositionLines");

            migrationBuilder.AddColumn<Guid>(
                name: "BranchesId",
                table: "SalesPropositionLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehousesId",
                table: "SalesPropositionLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SalesOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Time_ = table.Column<string>(type: "NVarChar(8)", maxLength: 8, nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialCode = table.Column<string>(type: "NVarChar(201)", maxLength: 201, nullable: true),
                    SalesOrderState = table.Column<int>(type: "Int", nullable: false),
                    LinkedSalesPropositionID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    WarehouseID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatExcludedAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    NetAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WorkOrderCreationDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ShippingAdressID = table.Column<Guid>(type: "UniqueIdentifier", nullable: true),
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
                    table.PrimaryKey("PK_SalesOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrders_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_ShippingAdresses_ShippingAdressID",
                        column: x => x.ShippingAdressID,
                        principalTable: "ShippingAdresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrders_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SalesOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LikedPropositionLineID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineTotalAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineDescription = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    VATrate = table.Column<int>(type: "Int", nullable: false),
                    VATamount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    SalesOrderLineStateEnum = table.Column<int>(type: "Int", nullable: false),
                    WorkOrderCreationDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    SalesPropositionLinesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_SalesOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_SalesOrders_SalesOrderID",
                        column: x => x.SalesOrderID,
                        principalTable: "SalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_SalesPropositionLines_SalesPropositionLinesId",
                        column: x => x.SalesPropositionLinesId,
                        principalTable: "SalesPropositionLines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesOrderLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_BranchesId",
                table: "SalesPropositionLines",
                column: "BranchesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_WarehousesId",
                table: "SalesPropositionLines",
                column: "WarehousesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_PaymentPlanID",
                table: "SalesOrderLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_ProductID",
                table: "SalesOrderLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_SalesOrderID",
                table: "SalesOrderLines",
                column: "SalesOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_SalesPropositionLinesId",
                table: "SalesOrderLines",
                column: "SalesPropositionLinesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_UnitSetID",
                table: "SalesOrderLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_BranchID",
                table: "SalesOrders",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CurrencyID",
                table: "SalesOrders",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CurrentAccountCardID",
                table: "SalesOrders",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_FicheNo",
                table: "SalesOrders",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_PaymentPlanID",
                table: "SalesOrders",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_ShippingAdressID",
                table: "SalesOrders",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_WarehouseID",
                table: "SalesOrders",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_Branches_BranchesId",
                table: "SalesPropositionLines",
                column: "BranchesId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_Warehouses_WarehousesId",
                table: "SalesPropositionLines",
                column: "WarehousesId",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftLines_Shifts_ShiftID",
                table: "ShiftLines",
                column: "ShiftID",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_Branches_BranchesId",
                table: "SalesPropositionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_Warehouses_WarehousesId",
                table: "SalesPropositionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftLines_Shifts_ShiftID",
                table: "ShiftLines");

            migrationBuilder.DropTable(
                name: "SalesOrderLines");

            migrationBuilder.DropTable(
                name: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositionLines_BranchesId",
                table: "SalesPropositionLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositionLines_WarehousesId",
                table: "SalesPropositionLines");

            migrationBuilder.DropColumn(
                name: "BranchesId",
                table: "SalesPropositionLines");

            migrationBuilder.DropColumn(
                name: "WarehousesId",
                table: "SalesPropositionLines");

            migrationBuilder.AddColumn<Guid>(
                name: "BranchID",
                table: "SalesPropositionLines",
                type: "UniqueIdentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseID",
                table: "SalesPropositionLines",
                type: "UniqueIdentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_BranchID",
                table: "SalesPropositionLines",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_WarehouseID",
                table: "SalesPropositionLines",
                column: "WarehouseID");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_Branches_BranchID",
                table: "SalesPropositionLines",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_Warehouses_WarehouseID",
                table: "SalesPropositionLines",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftLines_Shifts_ShiftID",
                table: "ShiftLines",
                column: "ShiftID",
                principalTable: "Shifts",
                principalColumn: "Id");
        }
    }
}
