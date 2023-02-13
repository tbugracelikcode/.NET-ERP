using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class dataConcurrenyadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "WorkOrders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "WorkOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Warehouses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Warehouses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "VsmSchemas",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "VsmSchemas",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "UserGroups",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "UserGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "UnplannedMaintenances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "UnplannedMaintenances",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "UnplannedMaintenanceLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "UnplannedMaintenanceLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "UnitSets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "UnitSets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "TemplateOperations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "TemplateOperations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "TemplateOperationLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "TemplateOperationLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "TechnicalDrawings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "TechnicalDrawings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Stations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Stations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "StationInventories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "StationInventories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "StationGroups",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "StationGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ShippingAdresses",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ShippingAdresses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Shifts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Shifts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ShiftLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ShiftLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "SalesPropositions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "SalesPropositions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "SalesPropositionLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "SalesPropositionLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "SalesPrices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "SalesPrices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "SalesPriceLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "SalesPriceLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "SalesOrders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "SalesOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "SalesOrderLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "SalesOrderLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Routes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Routes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "RouteLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "RouteLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PurchasingUnsuitabilityItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PurchasingUnsuitabilityItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PurchaseUnsuitabilityReports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PurchaseUnsuitabilityReports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PurchaseRequests",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PurchaseRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PurchaseRequestLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PurchaseRequestLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PurchasePrices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PurchasePrices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PurchasePriceLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PurchasePriceLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PurchaseOrders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PurchaseOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PurchaseOrderLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PurchaseOrderLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ProductsOperations",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ProductsOperations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ProductsOperationLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ProductsOperationLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ProductReferanceNumbers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ProductReferanceNumbers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ProductionTrackings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ProductionTrackings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ProductionTrackingHaltLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ProductionTrackingHaltLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ProductionOrders",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ProductionOrders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ProductionOrderChangeItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ProductionOrderChangeItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ProductGroups",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ProductGroups",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PlannedMaintenances",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PlannedMaintenances",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PlannedMaintenanceLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PlannedMaintenanceLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Periods",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Periods",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "PaymentPlans",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "PaymentPlans",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "OperationUnsuitabilityReports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "OperationUnsuitabilityReports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "OperationUnsuitabilityItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "OperationUnsuitabilityItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Menus",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Menus",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "MaintenancePeriods",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "MaintenancePeriods",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "MaintenanceInstructions",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "MaintenanceInstructions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "MaintenanceInstructionLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "MaintenanceInstructionLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "HaltReasons",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "HaltReasons",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "GrandTotalStockMovements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "GrandTotalStockMovements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Forecasts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Forecasts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ForecastLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ForecastLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "FinalControlUnsuitabilityReports",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "FinalControlUnsuitabilityReports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "FinalControlUnsuitabilityItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "FinalControlUnsuitabilityItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ExchangeRates",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ExchangeRates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "EquipmentRecords",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "EquipmentRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Employees",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Departments",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Departments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "CustomerComplaintItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "CustomerComplaintItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "CurrentAccountCards",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "CurrentAccountCards",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Currencies",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Currencies",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ContractUnsuitabilityItems",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ContractUnsuitabilityItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ContractProductionTrackings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ContractProductionTrackings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "CalibrationVerifications",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "CalibrationVerifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "CalibrationRecords",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "CalibrationRecords",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Calendars",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Calendars",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "CalendarLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "CalendarLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "CalendarDays",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "CalendarDays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "ByDateStockMovements",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "ByDateStockMovements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "Branches",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "Branches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "BillsofMaterials",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "BillsofMaterials",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DataOpenStatus",
                table: "BillsofMaterialLines",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DataOpenStatusUserId",
                table: "BillsofMaterialLines",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "VsmSchemas");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "VsmSchemas");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "UnplannedMaintenances");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "UnplannedMaintenances");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "UnplannedMaintenanceLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "UnplannedMaintenanceLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "UnitSets");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "UnitSets");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "TemplateOperations");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "TemplateOperations");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "TemplateOperationLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "TemplateOperationLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "TechnicalDrawings");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "TechnicalDrawings");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "StationInventories");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "StationInventories");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "StationGroups");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "StationGroups");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ShippingAdresses");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ShippingAdresses");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Shifts");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ShiftLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ShiftLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "SalesPropositions");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "SalesPropositions");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "SalesPropositionLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "SalesPropositionLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "SalesPrices");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "SalesPrices");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "SalesPriceLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "SalesPriceLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "SalesOrderLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "SalesOrderLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "RouteLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "RouteLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PurchasingUnsuitabilityItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PurchasingUnsuitabilityItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PurchaseUnsuitabilityReports");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PurchaseUnsuitabilityReports");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PurchaseRequestLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PurchaseRequestLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PurchasePrices");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PurchasePrices");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PurchasePriceLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PurchasePriceLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PurchaseOrderLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PurchaseOrderLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ProductsOperations");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ProductsOperations");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ProductsOperationLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ProductsOperationLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ProductReferanceNumbers");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ProductReferanceNumbers");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ProductionTrackings");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ProductionTrackings");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ProductionTrackingHaltLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ProductionTrackingHaltLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ProductionOrders");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ProductionOrders");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ProductionOrderChangeItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ProductionOrderChangeItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ProductGroups");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ProductGroups");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PlannedMaintenances");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PlannedMaintenances");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PlannedMaintenanceLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PlannedMaintenanceLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Periods");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "PaymentPlans");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "PaymentPlans");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "OperationUnsuitabilityItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "OperationUnsuitabilityItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "MaintenancePeriods");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "MaintenancePeriods");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "MaintenanceInstructions");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "MaintenanceInstructions");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "MaintenanceInstructionLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "MaintenanceInstructionLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "HaltReasons");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "HaltReasons");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "GrandTotalStockMovements");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "GrandTotalStockMovements");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Forecasts");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Forecasts");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ForecastLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ForecastLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "FinalControlUnsuitabilityReports");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "FinalControlUnsuitabilityReports");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "FinalControlUnsuitabilityItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "FinalControlUnsuitabilityItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ExchangeRates");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "EquipmentRecords");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "EquipmentRecords");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "CustomerComplaintItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "CustomerComplaintItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "CurrentAccountCards");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "CurrentAccountCards");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ContractUnsuitabilityItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ContractUnsuitabilityItems");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ContractProductionTrackings");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ContractProductionTrackings");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "CalibrationVerifications");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "CalibrationVerifications");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "CalibrationRecords");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "CalibrationRecords");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "CalendarLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "CalendarLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "ByDateStockMovements");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "ByDateStockMovements");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "BillsofMaterials");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "BillsofMaterials");

            migrationBuilder.DropColumn(
                name: "DataOpenStatus",
                table: "BillsofMaterialLines");

            migrationBuilder.DropColumn(
                name: "DataOpenStatusUserId",
                table: "BillsofMaterialLines");
        }
    }
}
