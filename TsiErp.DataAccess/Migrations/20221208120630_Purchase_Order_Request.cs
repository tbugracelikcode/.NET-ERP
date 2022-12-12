using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class Purchase_Order_Request : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseRequestsId",
                table: "SalesPropositionLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PurchaseRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_ = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PropositionRevisionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RevisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevisionTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchaseRequestState = table.Column<int>(type: "int", nullable: false),
                    LinkedPurchaseRequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatExcludedAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ValidityDate_ = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingAdressID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductionOrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BranchesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WarehousesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrenciesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentAccountCardsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShippingAdressesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductionOrdersId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Branches_BranchesId",
                        column: x => x.BranchesId,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Currencies_CurrenciesId",
                        column: x => x.CurrenciesId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_CurrentAccountCards_CurrentAccountCardsId",
                        column: x => x.CurrentAccountCardsId,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_ProductionOrders_ProductionOrdersId",
                        column: x => x.ProductionOrdersId,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_ShippingAdresses_ShippingAdressesId",
                        column: x => x.ShippingAdressesId,
                        principalTable: "ShippingAdresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Warehouses_WarehousesId",
                        column: x => x.WarehousesId,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FicheNo = table.Column<string>(type: "NVarChar(17)", maxLength: 17, nullable: false),
                    Date_ = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Time_ = table.Column<string>(type: "NVarChar(8)", maxLength: 8, nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Description_ = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialCode = table.Column<string>(type: "NVarChar(201)", maxLength: 201, nullable: true),
                    PurchaseOrderState = table.Column<int>(type: "Int", nullable: false),
                    LinkedPurchaseRequestID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrencyID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    BranchID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    WarehouseID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    CurrentAccountCardID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatExcludedAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalVatAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    TotalDiscountAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    NetAmount = table.Column<decimal>(type: "Decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    WorkOrderCreationDate = table.Column<DateTime>(type: "DateTime", nullable: true),
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
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Branches_BranchID",
                        column: x => x.BranchID,
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Currencies_CurrencyID",
                        column: x => x.CurrencyID,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_CurrentAccountCards_CurrentAccountCardID",
                        column: x => x.CurrentAccountCardID,
                        principalTable: "CurrentAccountCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PurchaseRequests_LinkedPurchaseRequestID",
                        column: x => x.LinkedPurchaseRequestID,
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_ShippingAdresses_ShippingAdressID",
                        column: x => x.ShippingAdressID,
                        principalTable: "ShippingAdresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Warehouses_WarehouseID",
                        column: x => x.WarehouseID,
                        principalTable: "Warehouses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRequestLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseRequestID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNr = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineTotalAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    LineDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATrate = table.Column<int>(type: "int", nullable: false),
                    VATamount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PaymentPlanID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductionOrderID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseRequestLineState = table.Column<int>(type: "int", nullable: false),
                    OrderConversionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProductsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitSetsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentPlansId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseRequestsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductionOrdersId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequestLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRequestLines_PaymentPlans_PaymentPlansId",
                        column: x => x.PaymentPlansId,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequestLines_ProductionOrders_ProductionOrdersId",
                        column: x => x.ProductionOrdersId,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequestLines_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequestLines_PurchaseRequests_PurchaseRequestsId",
                        column: x => x.PurchaseRequestsId,
                        principalTable: "PurchaseRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseRequestLines_UnitSets_UnitSetsId",
                        column: x => x.UnitSetsId,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LineNr = table.Column<int>(type: "Int", nullable: false),
                    ProductID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    UnitSetID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    ProductionOrderID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
                    LikedPurchaseRequestLineID = table.Column<Guid>(type: "UniqueIdentifier", nullable: false),
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
                    PurchaseOrderLineStateEnum = table.Column<int>(type: "Int", nullable: false),
                    WorkOrderCreationDate = table.Column<DateTime>(type: "DateTime", nullable: true),
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
                    table.PrimaryKey("PK_PurchaseOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_PaymentPlans_PaymentPlanID",
                        column: x => x.PaymentPlanID,
                        principalTable: "PaymentPlans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_ProductionOrders_ProductionOrderID",
                        column: x => x.ProductionOrderID,
                        principalTable: "ProductionOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_PurchaseOrders_PurchaseOrderID",
                        column: x => x.PurchaseOrderID,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderLines_UnitSets_UnitSetID",
                        column: x => x.UnitSetID,
                        principalTable: "UnitSets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_PurchaseRequestsId",
                table: "SalesPropositionLines",
                column: "PurchaseRequestsId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_PaymentPlanID",
                table: "PurchaseOrderLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_ProductID",
                table: "PurchaseOrderLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_ProductionOrderID",
                table: "PurchaseOrderLines",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_PurchaseOrderID",
                table: "PurchaseOrderLines",
                column: "PurchaseOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_UnitSetID",
                table: "PurchaseOrderLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_BranchID",
                table: "PurchaseOrders",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CurrencyID",
                table: "PurchaseOrders",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CurrentAccountCardID",
                table: "PurchaseOrders",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_FicheNo",
                table: "PurchaseOrders",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_LinkedPurchaseRequestID",
                table: "PurchaseOrders",
                column: "LinkedPurchaseRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PaymentPlanID",
                table: "PurchaseOrders",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ProductionOrderID",
                table: "PurchaseOrders",
                column: "ProductionOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ShippingAdressID",
                table: "PurchaseOrders",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_WarehouseID",
                table: "PurchaseOrders",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_PaymentPlansId",
                table: "PurchaseRequestLines",
                column: "PaymentPlansId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_ProductionOrdersId",
                table: "PurchaseRequestLines",
                column: "ProductionOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_ProductsId",
                table: "PurchaseRequestLines",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_PurchaseRequestsId",
                table: "PurchaseRequestLines",
                column: "PurchaseRequestsId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_UnitSetsId",
                table: "PurchaseRequestLines",
                column: "UnitSetsId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_BranchesId",
                table: "PurchaseRequests",
                column: "BranchesId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_CurrenciesId",
                table: "PurchaseRequests",
                column: "CurrenciesId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_CurrentAccountCardsId",
                table: "PurchaseRequests",
                column: "CurrentAccountCardsId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_PaymentPlanID",
                table: "PurchaseRequests",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_ProductionOrdersId",
                table: "PurchaseRequests",
                column: "ProductionOrdersId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_ShippingAdressesId",
                table: "PurchaseRequests",
                column: "ShippingAdressesId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_WarehousesId",
                table: "PurchaseRequests",
                column: "WarehousesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_PurchaseRequests_PurchaseRequestsId",
                table: "SalesPropositionLines",
                column: "PurchaseRequestsId",
                principalTable: "PurchaseRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_PurchaseRequests_PurchaseRequestsId",
                table: "SalesPropositionLines");

            migrationBuilder.DropTable(
                name: "PurchaseOrderLines");

            migrationBuilder.DropTable(
                name: "PurchaseRequestLines");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositionLines_PurchaseRequestsId",
                table: "SalesPropositionLines");

            migrationBuilder.DropColumn(
                name: "PurchaseRequestsId",
                table: "SalesPropositionLines");
        }
    }
}
