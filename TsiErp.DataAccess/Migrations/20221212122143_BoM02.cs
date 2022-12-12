using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class BoM02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestLines_PaymentPlans_PaymentPlansId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestLines_Products_ProductsId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestLines_PurchaseRequests_PurchaseRequestsId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestLines_UnitSets_UnitSetsId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_Branches_BranchesId",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_Currencies_CurrenciesId",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_CurrentAccountCards_CurrentAccountCardsId",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_PaymentPlans_PaymentPlanID",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_ShippingAdresses_ShippingAdressesId",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_Warehouses_WarehousesId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_BranchesId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_CurrenciesId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_CurrentAccountCardsId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_ShippingAdressesId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_WarehousesId",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_PaymentPlansId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_ProductsId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_PurchaseRequestsId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_UnitSetsId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropColumn(
                name: "BranchesId",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "CurrenciesId",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "CurrentAccountCardsId",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "ShippingAdressesId",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "WarehousesId",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "PaymentPlansId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropColumn(
                name: "ProductsId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropColumn(
                name: "PurchaseRequestsId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropColumn(
                name: "UnitSetsId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropColumn(
                name: "QuantityFormula",
                table: "BillsofMaterialLines");

            migrationBuilder.AlterColumn<Guid>(
                name: "WarehouseID",
                table: "PurchaseRequests",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidityDate_",
                table: "PurchaseRequests",
                type: "DateTime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalVatExcludedAmount",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalVatAmount",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDiscountAmount",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<string>(
                name: "Time_",
                table: "PurchaseRequests",
                type: "NVarChar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SpecialCode",
                table: "PurchaseRequests",
                type: "NVarChar(201)",
                maxLength: 201,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ShippingAdressID",
                table: "PurchaseRequests",
                type: "UniqueIdentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RevisionTime",
                table: "PurchaseRequests",
                type: "NVarChar(8)",
                maxLength: 8,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RevisionDate",
                table: "PurchaseRequests",
                type: "DateTime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseRequestState",
                table: "PurchaseRequests",
                type: "Int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PropositionRevisionNo",
                table: "PurchaseRequests",
                type: "NVarChar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductionOrderID",
                table: "PurchaseRequests",
                type: "UniqueIdentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentPlanID",
                table: "PurchaseRequests",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetAmount",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<Guid>(
                name: "LinkedPurchaseRequestID",
                table: "PurchaseRequests",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrossAmount",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<string>(
                name: "FicheNo",
                table: "PurchaseRequests",
                type: "NVarChar(17)",
                maxLength: 17,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_",
                table: "PurchaseRequests",
                type: "DateTime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "PurchaseRequests",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrencyID",
                table: "PurchaseRequests",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "PurchaseRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BranchID",
                table: "PurchaseRequests",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "VATrate",
                table: "PurchaseRequestLines",
                type: "Int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "VATamount",
                table: "PurchaseRequestLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitSetID",
                table: "PurchaseRequestLines",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "PurchaseRequestLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "PurchaseRequestLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseRequestLineState",
                table: "PurchaseRequestLines",
                type: "Int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseRequestID",
                table: "PurchaseRequestLines",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductionOrderID",
                table: "PurchaseRequestLines",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductID",
                table: "PurchaseRequestLines",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentPlanID",
                table: "PurchaseRequestLines",
                type: "UniqueIdentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderConversionDate",
                table: "PurchaseRequestLines",
                type: "DateTime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LineTotalAmount",
                table: "PurchaseRequestLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<int>(
                name: "LineNr",
                table: "PurchaseRequestLines",
                type: "Int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "LineDescription",
                table: "PurchaseRequestLines",
                type: "nvarchar(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LineAmount",
                table: "PurchaseRequestLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "PurchaseRequestLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                table: "PurchaseRequestLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "PurchaseRequestLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "PurchaseRequestLines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_BranchID",
                table: "PurchaseRequests",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_CurrencyID",
                table: "PurchaseRequests",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_CurrentAccountCardID",
                table: "PurchaseRequests",
                column: "CurrentAccountCardID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_FicheNo",
                table: "PurchaseRequests",
                column: "FicheNo");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_ShippingAdressID",
                table: "PurchaseRequests",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_WarehouseID",
                table: "PurchaseRequests",
                column: "WarehouseID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_PaymentPlanID",
                table: "PurchaseRequestLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_ProductID",
                table: "PurchaseRequestLines",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_PurchaseRequestID",
                table: "PurchaseRequestLines",
                column: "PurchaseRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_UnitSetID",
                table: "PurchaseRequestLines",
                column: "UnitSetID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestLines_PaymentPlans_PaymentPlanID",
                table: "PurchaseRequestLines",
                column: "PaymentPlanID",
                principalTable: "PaymentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestLines_Products_ProductID",
                table: "PurchaseRequestLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestLines_PurchaseRequests_PurchaseRequestID",
                table: "PurchaseRequestLines",
                column: "PurchaseRequestID",
                principalTable: "PurchaseRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestLines_UnitSets_UnitSetID",
                table: "PurchaseRequestLines",
                column: "UnitSetID",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_Branches_BranchID",
                table: "PurchaseRequests",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_Currencies_CurrencyID",
                table: "PurchaseRequests",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_CurrentAccountCards_CurrentAccountCardID",
                table: "PurchaseRequests",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_PaymentPlans_PaymentPlanID",
                table: "PurchaseRequests",
                column: "PaymentPlanID",
                principalTable: "PaymentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_ShippingAdresses_ShippingAdressID",
                table: "PurchaseRequests",
                column: "ShippingAdressID",
                principalTable: "ShippingAdresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_Warehouses_WarehouseID",
                table: "PurchaseRequests",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestLines_PaymentPlans_PaymentPlanID",
                table: "PurchaseRequestLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestLines_Products_ProductID",
                table: "PurchaseRequestLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestLines_PurchaseRequests_PurchaseRequestID",
                table: "PurchaseRequestLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequestLines_UnitSets_UnitSetID",
                table: "PurchaseRequestLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_Branches_BranchID",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_Currencies_CurrencyID",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_CurrentAccountCards_CurrentAccountCardID",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_PaymentPlans_PaymentPlanID",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_ShippingAdresses_ShippingAdressID",
                table: "PurchaseRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRequests_Warehouses_WarehouseID",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_BranchID",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_CurrencyID",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_CurrentAccountCardID",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_FicheNo",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_ShippingAdressID",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_WarehouseID",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_PaymentPlanID",
                table: "PurchaseRequestLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_ProductID",
                table: "PurchaseRequestLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_PurchaseRequestID",
                table: "PurchaseRequestLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_UnitSetID",
                table: "PurchaseRequestLines");

            migrationBuilder.AlterColumn<Guid>(
                name: "WarehouseID",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidityDate_",
                table: "PurchaseRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DateTime");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalVatExcludedAmount",
                table: "PurchaseRequests",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalVatAmount",
                table: "PurchaseRequests",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDiscountAmount",
                table: "PurchaseRequests",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<string>(
                name: "Time_",
                table: "PurchaseRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SpecialCode",
                table: "PurchaseRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(201)",
                oldMaxLength: 201,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ShippingAdressID",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RevisionTime",
                table: "PurchaseRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(8)",
                oldMaxLength: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RevisionDate",
                table: "PurchaseRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DateTime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseRequestState",
                table: "PurchaseRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.AlterColumn<string>(
                name: "PropositionRevisionNo",
                table: "PurchaseRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductionOrderID",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentPlanID",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetAmount",
                table: "PurchaseRequests",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<Guid>(
                name: "LinkedPurchaseRequestID",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrossAmount",
                table: "PurchaseRequests",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<string>(
                name: "FicheNo",
                table: "PurchaseRequests",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVarChar(17)",
                oldMaxLength: 17);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "PurchaseRequests",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date_",
                table: "PurchaseRequests",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DateTime");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrentAccountCardID",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrencyID",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "PurchaseRequests",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "BranchID",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "BranchesId",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrenciesId",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CurrentAccountCardsId",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingAdressesId",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehousesId",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VATrate",
                table: "PurchaseRequestLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.AlterColumn<decimal>(
                name: "VATamount",
                table: "PurchaseRequestLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitSetID",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "PurchaseRequestLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "PurchaseRequestLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseRequestLineState",
                table: "PurchaseRequestLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.AlterColumn<Guid>(
                name: "PurchaseRequestID",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductionOrderID",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProductID",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PaymentPlanID",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UniqueIdentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderConversionDate",
                table: "PurchaseRequestLines",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DateTime",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LineTotalAmount",
                table: "PurchaseRequestLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<int>(
                name: "LineNr",
                table: "PurchaseRequestLines",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "Int");

            migrationBuilder.AlterColumn<string>(
                name: "LineDescription",
                table: "PurchaseRequestLines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(MAX)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "LineAmount",
                table: "PurchaseRequestLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "PurchaseRequestLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                table: "PurchaseRequestLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "PurchaseRequestLines",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationTime",
                table: "PurchaseRequestLines",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentPlansId",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductsId",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseRequestsId",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UnitSetsId",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuantityFormula",
                table: "BillsofMaterialLines",
                type: "NVarChar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

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
                name: "IX_PurchaseRequests_ShippingAdressesId",
                table: "PurchaseRequests",
                column: "ShippingAdressesId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_WarehousesId",
                table: "PurchaseRequests",
                column: "WarehousesId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_PaymentPlansId",
                table: "PurchaseRequestLines",
                column: "PaymentPlansId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestLines_PaymentPlans_PaymentPlansId",
                table: "PurchaseRequestLines",
                column: "PaymentPlansId",
                principalTable: "PaymentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestLines_Products_ProductsId",
                table: "PurchaseRequestLines",
                column: "ProductsId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestLines_PurchaseRequests_PurchaseRequestsId",
                table: "PurchaseRequestLines",
                column: "PurchaseRequestsId",
                principalTable: "PurchaseRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequestLines_UnitSets_UnitSetsId",
                table: "PurchaseRequestLines",
                column: "UnitSetsId",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_Branches_BranchesId",
                table: "PurchaseRequests",
                column: "BranchesId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_Currencies_CurrenciesId",
                table: "PurchaseRequests",
                column: "CurrenciesId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_CurrentAccountCards_CurrentAccountCardsId",
                table: "PurchaseRequests",
                column: "CurrentAccountCardsId",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_PaymentPlans_PaymentPlanID",
                table: "PurchaseRequests",
                column: "PaymentPlanID",
                principalTable: "PaymentPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_ShippingAdresses_ShippingAdressesId",
                table: "PurchaseRequests",
                column: "ShippingAdressesId",
                principalTable: "ShippingAdresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRequests_Warehouses_WarehousesId",
                table: "PurchaseRequests",
                column: "WarehousesId",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }
    }
}
