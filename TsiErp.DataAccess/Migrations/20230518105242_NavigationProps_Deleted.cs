using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TsiErp.DataAccess.Migrations
{
    public partial class NavigationProps_Deleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillsofMaterialLines_BillsofMaterials_BoMID",
                table: "BillsofMaterialLines");

            migrationBuilder.DropForeignKey(
                name: "FK_BillsofMaterialLines_Products_ProductID",
                table: "BillsofMaterialLines");

            migrationBuilder.DropForeignKey(
                name: "FK_BillsofMaterialLines_UnitSets_UnitSetID",
                table: "BillsofMaterialLines");

            migrationBuilder.DropForeignKey(
                name: "FK_BillsofMaterials_Products_FinishedProductID",
                table: "BillsofMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_ByDateStockMovements_Branches_BranchID",
                table: "ByDateStockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_ByDateStockMovements_Products_ProductID",
                table: "ByDateStockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_ByDateStockMovements_Warehouses_WarehouseID",
                table: "ByDateStockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarDays_Calendars_CalendarID",
                table: "CalendarDays");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarLines_Calendars_CalendarID",
                table: "CalendarLines");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarLines_Shifts_ShiftID",
                table: "CalendarLines");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarLines_Stations_StationID",
                table: "CalendarLines");

            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationRecords_EquipmentRecords_EquipmentID",
                table: "CalibrationRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CalibrationVerifications_EquipmentRecords_EquipmentID",
                table: "CalibrationVerifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractProductionTrackings_CurrentAccountCards_CurrentAccountCardID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractProductionTrackings_Employees_EmployeeID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractProductionTrackings_Products_ProductID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractProductionTrackings_Shifts_ShiftID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractProductionTrackings_Stations_StationID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ContractProductionTrackings_WorkOrders_WorkOrderID",
                table: "ContractProductionTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrentAccountCards_Currencies_CurrencyID",
                table: "CurrentAccountCards");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_EquipmentRecords_Departments_Department",
                table: "EquipmentRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyID",
                table: "ExchangeRates");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalControlUnsuitabilityReports_Employees_EmployeeID",
                table: "FinalControlUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalControlUnsuitabilityReports_Products_ProductID",
                table: "FinalControlUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ForecastLines_Forecasts_ForecastID",
                table: "ForecastLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ForecastLines_Products_ProductID",
                table: "ForecastLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_Branches_BranchID",
                table: "Forecasts");

            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_CurrentAccountCards_CurrentAccountCardID",
                table: "Forecasts");

            migrationBuilder.DropForeignKey(
                name: "FK_Forecasts_Periods_PeriodID",
                table: "Forecasts");

            migrationBuilder.DropForeignKey(
                name: "FK_GrandTotalStockMovements_Branches_BranchID",
                table: "GrandTotalStockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_GrandTotalStockMovements_Products_ProductID",
                table: "GrandTotalStockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_GrandTotalStockMovements_Warehouses_WarehouseID",
                table: "GrandTotalStockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceInstructionLines_MaintenanceInstructions_InstructionID",
                table: "MaintenanceInstructionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceInstructionLines_Products_ProductID",
                table: "MaintenanceInstructionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceInstructionLines_UnitSets_UnitSetID",
                table: "MaintenanceInstructionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceInstructions_MaintenancePeriods_PeriodID",
                table: "MaintenanceInstructions");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceInstructions_Stations_StationID",
                table: "MaintenanceInstructions");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationUnsuitabilityReports_Employees_EmployeeID",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationUnsuitabilityReports_ProductionOrders_ProductionOrderID",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationUnsuitabilityReports_Products_ProductID",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationUnsuitabilityReports_ProductsOperations_OperationID",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationUnsuitabilityReports_StationGroups_StationGroupID",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationUnsuitabilityReports_Stations_StationID",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_OperationUnsuitabilityReports_WorkOrders_WorkOrderID",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_Periods_Branches_BranchID",
                table: "Periods");

            migrationBuilder.DropForeignKey(
                name: "FK_PlannedMaintenanceLines_PlannedMaintenances_PlannedMaintenanceID",
                table: "PlannedMaintenanceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PlannedMaintenanceLines_Products_ProductID",
                table: "PlannedMaintenanceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PlannedMaintenanceLines_UnitSets_UnitSetID",
                table: "PlannedMaintenanceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PlannedMaintenances_MaintenancePeriods_PeriodID",
                table: "PlannedMaintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_PlannedMaintenances_Stations_StationID",
                table: "PlannedMaintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_BillsofMaterials_BOMID",
                table: "ProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_CurrentAccountCards_CurrentAccountID",
                table: "ProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_Products_FinishedProductID",
                table: "ProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_Routes_RouteID",
                table: "ProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_SalesOrderLines_OrderLineID",
                table: "ProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_SalesOrders_OrderID",
                table: "ProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_SalesPropositionLines_PropositionLineID",
                table: "ProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_SalesPropositions_PropositionID",
                table: "ProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionOrders_UnitSets_UnitSetID",
                table: "ProductionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionTrackingHaltLines_HaltReasons_HaltID",
                table: "ProductionTrackingHaltLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionTrackingHaltLines_ProductionTrackings_ProductionTrackingID",
                table: "ProductionTrackingHaltLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionTrackings_Employees_EmployeeID",
                table: "ProductionTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionTrackings_Shifts_ShiftID",
                table: "ProductionTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionTrackings_Stations_StationID",
                table: "ProductionTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionTrackings_WorkOrders_WorkOrderID",
                table: "ProductionTrackings");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReferanceNumbers_CurrentAccountCards_CurrentAccountCardID",
                table: "ProductReferanceNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductReferanceNumbers_Products_ProductID",
                table: "ProductReferanceNumbers");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductGroups_ProductGrpID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_UnitSets_UnitSetID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOperationLines_ProductsOperations_ProductsOperationID",
                table: "ProductsOperationLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOperationLines_Stations_StationID",
                table: "ProductsOperationLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOperations_Products_ProductID",
                table: "ProductsOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLines_PaymentPlans_PaymentPlanID",
                table: "PurchaseOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLines_Products_ProductID",
                table: "PurchaseOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLines_PurchaseOrders_PurchaseOrderID",
                table: "PurchaseOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderLines_UnitSets_UnitSetID",
                table: "PurchaseOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Branches_BranchID",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Currencies_CurrencyID",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_CurrentAccountCards_CurrentAccountCardID",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_PaymentPlans_PaymentPlanID",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_ShippingAdresses_ShippingAdressID",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Warehouses_WarehouseID",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePriceLines_Currencies_CurrencyID",
                table: "PurchasePriceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePriceLines_Products_ProductID",
                table: "PurchasePriceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePriceLines_PurchasePrices_PurchasePriceID",
                table: "PurchasePriceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePrices_Branches_BranchID",
                table: "PurchasePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePrices_Currencies_CurrencyID",
                table: "PurchasePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePrices_CurrentAccountCards_CurrentAccountCardID",
                table: "PurchasePrices");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasePrices_Warehouses_WarehouseID",
                table: "PurchasePrices");

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

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseUnsuitabilityReports_CurrentAccountCards_CurrentAccountCardID",
                table: "PurchaseUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseUnsuitabilityReports_Products_ProductID",
                table: "PurchaseUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseUnsuitabilityReports_PurchaseOrders_OrderID",
                table: "PurchaseUnsuitabilityReports");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_Products_ProductID",
                table: "RouteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_ProductsOperations_ProductsOperationID",
                table: "RouteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteLines_Routes_RouteID",
                table: "RouteLines");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Products_ProductID",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderLines_PaymentPlans_PaymentPlanID",
                table: "SalesOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderLines_Products_ProductID",
                table: "SalesOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderLines_SalesOrders_SalesOrderID",
                table: "SalesOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderLines_SalesPropositionLines_SalesPropositionLinesId",
                table: "SalesOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderLines_UnitSets_UnitSetID",
                table: "SalesOrderLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_Branches_BranchID",
                table: "SalesOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_Currencies_CurrencyID",
                table: "SalesOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_CurrentAccountCards_CurrentAccountCardID",
                table: "SalesOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_PaymentPlans_PaymentPlanID",
                table: "SalesOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_ShippingAdresses_ShippingAdressID",
                table: "SalesOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_Warehouses_WarehouseID",
                table: "SalesOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPriceLines_Currencies_CurrencyID",
                table: "SalesPriceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPriceLines_Products_ProductID",
                table: "SalesPriceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPriceLines_SalesPrices_SalesPriceID",
                table: "SalesPriceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPrices_Branches_BranchID",
                table: "SalesPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPrices_Currencies_CurrencyID",
                table: "SalesPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPrices_CurrentAccountCards_CurrentAccountCardID",
                table: "SalesPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPrices_Warehouses_WarehouseID",
                table: "SalesPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_PaymentPlans_PaymentPlanID",
                table: "SalesPropositionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_Products_ProductID",
                table: "SalesPropositionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_PurchaseRequests_PurchaseRequestsId",
                table: "SalesPropositionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_SalesPropositions_SalesPropositionID",
                table: "SalesPropositionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositionLines_UnitSets_UnitSetID",
                table: "SalesPropositionLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositions_Branches_BranchID",
                table: "SalesPropositions");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositions_Currencies_CurrencyID",
                table: "SalesPropositions");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositions_CurrentAccountCards_CurrentAccountCardID",
                table: "SalesPropositions");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositions_PaymentPlans_PaymentPlanID",
                table: "SalesPropositions");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositions_ShippingAdresses_ShippingAdressID",
                table: "SalesPropositions");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesPropositions_Warehouses_WarehouseID",
                table: "SalesPropositions");

            migrationBuilder.DropForeignKey(
                name: "FK_ShiftLines_Shifts_ShiftID",
                table: "ShiftLines");

            migrationBuilder.DropForeignKey(
                name: "FK_ShippingAdresses_CurrentAccountCards_CustomerCardID",
                table: "ShippingAdresses");

            migrationBuilder.DropForeignKey(
                name: "FK_StationInventories_Stations_StationID",
                table: "StationInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_StationGroups_GroupID",
                table: "Stations");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalDrawings_Products_ProductID",
                table: "TechnicalDrawings");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateOperationLines_Stations_StationID",
                table: "TemplateOperationLines");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateOperationLines_TemplateOperations_TemplateOperationID",
                table: "TemplateOperationLines");

            migrationBuilder.DropForeignKey(
                name: "FK_UnplannedMaintenanceLines_Products_ProductID",
                table: "UnplannedMaintenanceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_UnplannedMaintenanceLines_UnitSets_UnitSetID",
                table: "UnplannedMaintenanceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_UnplannedMaintenanceLines_UnplannedMaintenances_UnplannedMaintenanceID",
                table: "UnplannedMaintenanceLines");

            migrationBuilder.DropForeignKey(
                name: "FK_UnplannedMaintenances_MaintenancePeriods_PeriodID",
                table: "UnplannedMaintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_UnplannedMaintenances_Stations_StationID",
                table: "UnplannedMaintenances");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserGroups_GroupID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_CurrentAccountCards_CurrentAccountCardID",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_ProductionOrders_ProductionOrderID",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Products_ProductID",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_ProductsOperations_ProductsOperationID",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Routes_RouteID",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_SalesPropositions_PropositionID",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_StationGroups_StationGroupID",
                table: "WorkOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkOrders_Stations_StationID",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_PropositionID",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_RouteID",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_StationGroupID",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_WorkOrders_StationID",
                table: "WorkOrders");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_TemplateOperationLines_StationID",
                table: "TemplateOperationLines");

            migrationBuilder.DropIndex(
                name: "IX_Stations_GroupID",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_ShippingAdresses_CustomerCardID",
                table: "ShippingAdresses");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositions_CurrencyID",
                table: "SalesPropositions");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositions_PaymentPlanID",
                table: "SalesPropositions");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositions_ShippingAdressID",
                table: "SalesPropositions");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositionLines_PaymentPlanID",
                table: "SalesPropositionLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositionLines_PurchaseRequestsId",
                table: "SalesPropositionLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesPropositionLines_UnitSetID",
                table: "SalesPropositionLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_CurrencyID",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_PaymentPlanID",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_ShippingAdressID",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrderLines_PaymentPlanID",
                table: "SalesOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrderLines_SalesPropositionLinesId",
                table: "SalesOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrderLines_UnitSetID",
                table: "SalesOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_Routes_ProductID",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_CurrencyID",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_PaymentPlanID",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequests_ShippingAdressID",
                table: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_PaymentPlanID",
                table: "PurchaseRequestLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseRequestLines_UnitSetID",
                table: "PurchaseRequestLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_CurrencyID",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_PaymentPlanID",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ShippingAdressID",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderLines_PaymentPlanID",
                table: "PurchaseOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderLines_UnitSetID",
                table: "PurchaseOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_ProductsOperations_ProductID",
                table: "ProductsOperations");

            migrationBuilder.DropIndex(
                name: "IX_ProductsOperationLines_StationID",
                table: "ProductsOperationLines");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductGrpID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_UnitSetID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductionOrders_OrderLineID",
                table: "ProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_ProductionOrders_PropositionID",
                table: "ProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_ProductionOrders_PropositionLineID",
                table: "ProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_ProductionOrders_RouteID",
                table: "ProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_ProductionOrders_UnitSetID",
                table: "ProductionOrders");

            migrationBuilder.DropIndex(
                name: "IX_Periods_BranchID",
                table: "Periods");

            migrationBuilder.DropIndex(
                name: "IX_OperationUnsuitabilityReports_EmployeeID",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropIndex(
                name: "IX_OperationUnsuitabilityReports_StationGroupID",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropIndex(
                name: "IX_OperationUnsuitabilityReports_StationID",
                table: "OperationUnsuitabilityReports");

            migrationBuilder.DropIndex(
                name: "IX_EquipmentRecords_Department",
                table: "EquipmentRecords");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_CurrentAccountCards_CurrencyID",
                table: "CurrentAccountCards");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationVerifications_EquipmentID",
                table: "CalibrationVerifications");

            migrationBuilder.DropIndex(
                name: "IX_CalibrationRecords_EquipmentID",
                table: "CalibrationRecords");

            migrationBuilder.DropIndex(
                name: "IX_BillsofMaterials_FinishedProductID",
                table: "BillsofMaterials");

            migrationBuilder.DropColumn(
                name: "PurchaseRequestsId",
                table: "SalesPropositionLines");

            migrationBuilder.DropColumn(
                name: "SalesPropositionLinesId",
                table: "SalesOrderLines");

            migrationBuilder.DropColumn(
                name: "ShippingAdressID",
                table: "PurchaseRequests");

            migrationBuilder.DropColumn(
                name: "RouteID",
                table: "BillsofMaterials");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWorkTime",
                table: "Shifts",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalBreakTime",
                table: "Shifts",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "Overtime",
                table: "Shifts",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetWorkTime",
                table: "Shifts",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "Coefficient",
                table: "ShiftLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "OperationTime",
                table: "RouteLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalVatExcludedAmount",
                table: "PurchaseRequests",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalVatAmount",
                table: "PurchaseRequests",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDiscountAmount",
                table: "PurchaseRequests",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "NetAmount",
                table: "PurchaseRequests",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "GrossAmount",
                table: "PurchaseRequests",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "PurchaseRequests",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDays",
                table: "Calendars",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "OfficialHolidayDays",
                table: "Calendars",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailableDays",
                table: "Calendars",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "ShiftTime",
                table: "CalendarLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "ShiftOverTime",
                table: "CalendarLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedHaltTimes",
                table: "CalendarLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailableTime",
                table: "CalendarLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AddColumn<Guid>(
                name: "CalendarsId",
                table: "CalendarLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Size",
                table: "BillsofMaterialLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "BillsofMaterialLines",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarLines_CalendarsId",
                table: "CalendarLines",
                column: "CalendarsId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarLines_Calendars_CalendarsId",
                table: "CalendarLines",
                column: "CalendarsId",
                principalTable: "Calendars",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalendarLines_Calendars_CalendarsId",
                table: "CalendarLines");

            migrationBuilder.DropIndex(
                name: "IX_CalendarLines_CalendarsId",
                table: "CalendarLines");

            migrationBuilder.DropColumn(
                name: "CalendarsId",
                table: "CalendarLines");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalWorkTime",
                table: "Shifts",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalBreakTime",
                table: "Shifts",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Overtime",
                table: "Shifts",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "NetWorkTime",
                table: "Shifts",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Coefficient",
                table: "ShiftLines",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AddColumn<Guid>(
                name: "PurchaseRequestsId",
                table: "SalesPropositionLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesPropositionLinesId",
                table: "SalesOrderLines",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "OperationTime",
                table: "RouteLines",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalVatExcludedAmount",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalVatAmount",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDiscountAmount",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "NetAmount",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "GrossAmount",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "PurchaseRequests",
                type: "Decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal");

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingAdressID",
                table: "PurchaseRequests",
                type: "UniqueIdentifier",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalDays",
                table: "Calendars",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "OfficialHolidayDays",
                table: "Calendars",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailableDays",
                table: "Calendars",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "ShiftTime",
                table: "CalendarLines",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "ShiftOverTime",
                table: "CalendarLines",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "PlannedHaltTimes",
                table: "CalendarLines",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "AvailableTime",
                table: "CalendarLines",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AddColumn<Guid>(
                name: "RouteID",
                table: "BillsofMaterials",
                type: "UniqueIdentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<decimal>(
                name: "Size",
                table: "BillsofMaterialLines",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "BillsofMaterialLines",
                type: "Decimal",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "Decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

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

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupID",
                table: "Users",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateOperationLines_StationID",
                table: "TemplateOperationLines",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_Stations_GroupID",
                table: "Stations",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingAdresses_CustomerCardID",
                table: "ShippingAdresses",
                column: "CustomerCardID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_CurrencyID",
                table: "SalesPropositions",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_PaymentPlanID",
                table: "SalesPropositions",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositions_ShippingAdressID",
                table: "SalesPropositions",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_PaymentPlanID",
                table: "SalesPropositionLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_PurchaseRequestsId",
                table: "SalesPropositionLines",
                column: "PurchaseRequestsId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesPropositionLines_UnitSetID",
                table: "SalesPropositionLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CurrencyID",
                table: "SalesOrders",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_PaymentPlanID",
                table: "SalesOrders",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_ShippingAdressID",
                table: "SalesOrders",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_PaymentPlanID",
                table: "SalesOrderLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_SalesPropositionLinesId",
                table: "SalesOrderLines",
                column: "SalesPropositionLinesId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrderLines_UnitSetID",
                table: "SalesOrderLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_ProductID",
                table: "Routes",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_CurrencyID",
                table: "PurchaseRequests",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_PaymentPlanID",
                table: "PurchaseRequests",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_ShippingAdressID",
                table: "PurchaseRequests",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_PaymentPlanID",
                table: "PurchaseRequestLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequestLines_UnitSetID",
                table: "PurchaseRequestLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CurrencyID",
                table: "PurchaseOrders",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PaymentPlanID",
                table: "PurchaseOrders",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ShippingAdressID",
                table: "PurchaseOrders",
                column: "ShippingAdressID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_PaymentPlanID",
                table: "PurchaseOrderLines",
                column: "PaymentPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderLines_UnitSetID",
                table: "PurchaseOrderLines",
                column: "UnitSetID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperations_ProductID",
                table: "ProductsOperations",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsOperationLines_StationID",
                table: "ProductsOperationLines",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductGrpID",
                table: "Products",
                column: "ProductGrpID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UnitSetID",
                table: "Products",
                column: "UnitSetID");

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
                name: "IX_Periods_BranchID",
                table: "Periods",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_EmployeeID",
                table: "OperationUnsuitabilityReports",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_StationGroupID",
                table: "OperationUnsuitabilityReports",
                column: "StationGroupID");

            migrationBuilder.CreateIndex(
                name: "IX_OperationUnsuitabilityReports_StationID",
                table: "OperationUnsuitabilityReports",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentRecords_Department",
                table: "EquipmentRecords",
                column: "Department");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentID",
                table: "Employees",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_CurrentAccountCards_CurrencyID",
                table: "CurrentAccountCards",
                column: "CurrencyID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationVerifications_EquipmentID",
                table: "CalibrationVerifications",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationRecords_EquipmentID",
                table: "CalibrationRecords",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_BillsofMaterials_FinishedProductID",
                table: "BillsofMaterials",
                column: "FinishedProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_BillsofMaterialLines_BillsofMaterials_BoMID",
                table: "BillsofMaterialLines",
                column: "BoMID",
                principalTable: "BillsofMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillsofMaterialLines_Products_ProductID",
                table: "BillsofMaterialLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BillsofMaterialLines_UnitSets_UnitSetID",
                table: "BillsofMaterialLines",
                column: "UnitSetID",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BillsofMaterials_Products_FinishedProductID",
                table: "BillsofMaterials",
                column: "FinishedProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ByDateStockMovements_Branches_BranchID",
                table: "ByDateStockMovements",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ByDateStockMovements_Products_ProductID",
                table: "ByDateStockMovements",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ByDateStockMovements_Warehouses_WarehouseID",
                table: "ByDateStockMovements",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarDays_Calendars_CalendarID",
                table: "CalendarDays",
                column: "CalendarID",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarLines_Calendars_CalendarID",
                table: "CalendarLines",
                column: "CalendarID",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarLines_Shifts_ShiftID",
                table: "CalendarLines",
                column: "ShiftID",
                principalTable: "Shifts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarLines_Stations_StationID",
                table: "CalendarLines",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationRecords_EquipmentRecords_EquipmentID",
                table: "CalibrationRecords",
                column: "EquipmentID",
                principalTable: "EquipmentRecords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CalibrationVerifications_EquipmentRecords_EquipmentID",
                table: "CalibrationVerifications",
                column: "EquipmentID",
                principalTable: "EquipmentRecords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractProductionTrackings_CurrentAccountCards_CurrentAccountCardID",
                table: "ContractProductionTrackings",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractProductionTrackings_Employees_EmployeeID",
                table: "ContractProductionTrackings",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractProductionTrackings_Products_ProductID",
                table: "ContractProductionTrackings",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractProductionTrackings_Shifts_ShiftID",
                table: "ContractProductionTrackings",
                column: "ShiftID",
                principalTable: "Shifts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractProductionTrackings_Stations_StationID",
                table: "ContractProductionTrackings",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractProductionTrackings_WorkOrders_WorkOrderID",
                table: "ContractProductionTrackings",
                column: "WorkOrderID",
                principalTable: "WorkOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrentAccountCards_Currencies_CurrencyID",
                table: "CurrentAccountCards",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentID",
                table: "Employees",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EquipmentRecords_Departments_Department",
                table: "EquipmentRecords",
                column: "Department",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRates_Currencies_CurrencyID",
                table: "ExchangeRates",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalControlUnsuitabilityReports_Employees_EmployeeID",
                table: "FinalControlUnsuitabilityReports",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalControlUnsuitabilityReports_Products_ProductID",
                table: "FinalControlUnsuitabilityReports",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForecastLines_Forecasts_ForecastID",
                table: "ForecastLines",
                column: "ForecastID",
                principalTable: "Forecasts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForecastLines_Products_ProductID",
                table: "ForecastLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Branches_BranchID",
                table: "Forecasts",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_CurrentAccountCards_CurrentAccountCardID",
                table: "Forecasts",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Forecasts_Periods_PeriodID",
                table: "Forecasts",
                column: "PeriodID",
                principalTable: "Periods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrandTotalStockMovements_Branches_BranchID",
                table: "GrandTotalStockMovements",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrandTotalStockMovements_Products_ProductID",
                table: "GrandTotalStockMovements",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GrandTotalStockMovements_Warehouses_WarehouseID",
                table: "GrandTotalStockMovements",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceInstructionLines_MaintenanceInstructions_InstructionID",
                table: "MaintenanceInstructionLines",
                column: "InstructionID",
                principalTable: "MaintenanceInstructions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceInstructionLines_Products_ProductID",
                table: "MaintenanceInstructionLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceInstructionLines_UnitSets_UnitSetID",
                table: "MaintenanceInstructionLines",
                column: "UnitSetID",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceInstructions_MaintenancePeriods_PeriodID",
                table: "MaintenanceInstructions",
                column: "PeriodID",
                principalTable: "MaintenancePeriods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceInstructions_Stations_StationID",
                table: "MaintenanceInstructions",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationUnsuitabilityReports_Employees_EmployeeID",
                table: "OperationUnsuitabilityReports",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationUnsuitabilityReports_ProductionOrders_ProductionOrderID",
                table: "OperationUnsuitabilityReports",
                column: "ProductionOrderID",
                principalTable: "ProductionOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationUnsuitabilityReports_Products_ProductID",
                table: "OperationUnsuitabilityReports",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationUnsuitabilityReports_ProductsOperations_OperationID",
                table: "OperationUnsuitabilityReports",
                column: "OperationID",
                principalTable: "ProductsOperations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationUnsuitabilityReports_StationGroups_StationGroupID",
                table: "OperationUnsuitabilityReports",
                column: "StationGroupID",
                principalTable: "StationGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationUnsuitabilityReports_Stations_StationID",
                table: "OperationUnsuitabilityReports",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationUnsuitabilityReports_WorkOrders_WorkOrderID",
                table: "OperationUnsuitabilityReports",
                column: "WorkOrderID",
                principalTable: "WorkOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Periods_Branches_BranchID",
                table: "Periods",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedMaintenanceLines_PlannedMaintenances_PlannedMaintenanceID",
                table: "PlannedMaintenanceLines",
                column: "PlannedMaintenanceID",
                principalTable: "PlannedMaintenances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedMaintenanceLines_Products_ProductID",
                table: "PlannedMaintenanceLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedMaintenanceLines_UnitSets_UnitSetID",
                table: "PlannedMaintenanceLines",
                column: "UnitSetID",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedMaintenances_MaintenancePeriods_PeriodID",
                table: "PlannedMaintenances",
                column: "PeriodID",
                principalTable: "MaintenancePeriods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedMaintenances_Stations_StationID",
                table: "PlannedMaintenances",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_BillsofMaterials_BOMID",
                table: "ProductionOrders",
                column: "BOMID",
                principalTable: "BillsofMaterials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_CurrentAccountCards_CurrentAccountID",
                table: "ProductionOrders",
                column: "CurrentAccountID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_Products_FinishedProductID",
                table: "ProductionOrders",
                column: "FinishedProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_Routes_RouteID",
                table: "ProductionOrders",
                column: "RouteID",
                principalTable: "Routes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_SalesOrderLines_OrderLineID",
                table: "ProductionOrders",
                column: "OrderLineID",
                principalTable: "SalesOrderLines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_SalesOrders_OrderID",
                table: "ProductionOrders",
                column: "OrderID",
                principalTable: "SalesOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_SalesPropositionLines_PropositionLineID",
                table: "ProductionOrders",
                column: "PropositionLineID",
                principalTable: "SalesPropositionLines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_SalesPropositions_PropositionID",
                table: "ProductionOrders",
                column: "PropositionID",
                principalTable: "SalesPropositions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionOrders_UnitSets_UnitSetID",
                table: "ProductionOrders",
                column: "UnitSetID",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionTrackingHaltLines_HaltReasons_HaltID",
                table: "ProductionTrackingHaltLines",
                column: "HaltID",
                principalTable: "HaltReasons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionTrackingHaltLines_ProductionTrackings_ProductionTrackingID",
                table: "ProductionTrackingHaltLines",
                column: "ProductionTrackingID",
                principalTable: "ProductionTrackings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionTrackings_Employees_EmployeeID",
                table: "ProductionTrackings",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionTrackings_Shifts_ShiftID",
                table: "ProductionTrackings",
                column: "ShiftID",
                principalTable: "Shifts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionTrackings_Stations_StationID",
                table: "ProductionTrackings",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionTrackings_WorkOrders_WorkOrderID",
                table: "ProductionTrackings",
                column: "WorkOrderID",
                principalTable: "WorkOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReferanceNumbers_CurrentAccountCards_CurrentAccountCardID",
                table: "ProductReferanceNumbers",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductReferanceNumbers_Products_ProductID",
                table: "ProductReferanceNumbers",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductGroups_ProductGrpID",
                table: "Products",
                column: "ProductGrpID",
                principalTable: "ProductGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_UnitSets_UnitSetID",
                table: "Products",
                column: "UnitSetID",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOperationLines_ProductsOperations_ProductsOperationID",
                table: "ProductsOperationLines",
                column: "ProductsOperationID",
                principalTable: "ProductsOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOperationLines_Stations_StationID",
                table: "ProductsOperationLines",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOperations_Products_ProductID",
                table: "ProductsOperations",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLines_PaymentPlans_PaymentPlanID",
                table: "PurchaseOrderLines",
                column: "PaymentPlanID",
                principalTable: "PaymentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLines_Products_ProductID",
                table: "PurchaseOrderLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLines_PurchaseOrders_PurchaseOrderID",
                table: "PurchaseOrderLines",
                column: "PurchaseOrderID",
                principalTable: "PurchaseOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderLines_UnitSets_UnitSetID",
                table: "PurchaseOrderLines",
                column: "UnitSetID",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Branches_BranchID",
                table: "PurchaseOrders",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Currencies_CurrencyID",
                table: "PurchaseOrders",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_CurrentAccountCards_CurrentAccountCardID",
                table: "PurchaseOrders",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_PaymentPlans_PaymentPlanID",
                table: "PurchaseOrders",
                column: "PaymentPlanID",
                principalTable: "PaymentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_ShippingAdresses_ShippingAdressID",
                table: "PurchaseOrders",
                column: "ShippingAdressID",
                principalTable: "ShippingAdresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Warehouses_WarehouseID",
                table: "PurchaseOrders",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePriceLines_Currencies_CurrencyID",
                table: "PurchasePriceLines",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePriceLines_Products_ProductID",
                table: "PurchasePriceLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePriceLines_PurchasePrices_PurchasePriceID",
                table: "PurchasePriceLines",
                column: "PurchasePriceID",
                principalTable: "PurchasePrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePrices_Branches_BranchID",
                table: "PurchasePrices",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePrices_Currencies_CurrencyID",
                table: "PurchasePrices",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePrices_CurrentAccountCards_CurrentAccountCardID",
                table: "PurchasePrices",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasePrices_Warehouses_WarehouseID",
                table: "PurchasePrices",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseUnsuitabilityReports_CurrentAccountCards_CurrentAccountCardID",
                table: "PurchaseUnsuitabilityReports",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseUnsuitabilityReports_Products_ProductID",
                table: "PurchaseUnsuitabilityReports",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseUnsuitabilityReports_PurchaseOrders_OrderID",
                table: "PurchaseUnsuitabilityReports",
                column: "OrderID",
                principalTable: "PurchaseOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_Products_ProductID",
                table: "RouteLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_ProductsOperations_ProductsOperationID",
                table: "RouteLines",
                column: "ProductsOperationID",
                principalTable: "ProductsOperations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteLines_Routes_RouteID",
                table: "RouteLines",
                column: "RouteID",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Products_ProductID",
                table: "Routes",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderLines_PaymentPlans_PaymentPlanID",
                table: "SalesOrderLines",
                column: "PaymentPlanID",
                principalTable: "PaymentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderLines_Products_ProductID",
                table: "SalesOrderLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderLines_SalesOrders_SalesOrderID",
                table: "SalesOrderLines",
                column: "SalesOrderID",
                principalTable: "SalesOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderLines_SalesPropositionLines_SalesPropositionLinesId",
                table: "SalesOrderLines",
                column: "SalesPropositionLinesId",
                principalTable: "SalesPropositionLines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderLines_UnitSets_UnitSetID",
                table: "SalesOrderLines",
                column: "UnitSetID",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_Branches_BranchID",
                table: "SalesOrders",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_Currencies_CurrencyID",
                table: "SalesOrders",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_CurrentAccountCards_CurrentAccountCardID",
                table: "SalesOrders",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_PaymentPlans_PaymentPlanID",
                table: "SalesOrders",
                column: "PaymentPlanID",
                principalTable: "PaymentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_ShippingAdresses_ShippingAdressID",
                table: "SalesOrders",
                column: "ShippingAdressID",
                principalTable: "ShippingAdresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_Warehouses_WarehouseID",
                table: "SalesOrders",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPriceLines_Currencies_CurrencyID",
                table: "SalesPriceLines",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPriceLines_Products_ProductID",
                table: "SalesPriceLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPriceLines_SalesPrices_SalesPriceID",
                table: "SalesPriceLines",
                column: "SalesPriceID",
                principalTable: "SalesPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPrices_Branches_BranchID",
                table: "SalesPrices",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPrices_Currencies_CurrencyID",
                table: "SalesPrices",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPrices_CurrentAccountCards_CurrentAccountCardID",
                table: "SalesPrices",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPrices_Warehouses_WarehouseID",
                table: "SalesPrices",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_PaymentPlans_PaymentPlanID",
                table: "SalesPropositionLines",
                column: "PaymentPlanID",
                principalTable: "PaymentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_Products_ProductID",
                table: "SalesPropositionLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_PurchaseRequests_PurchaseRequestsId",
                table: "SalesPropositionLines",
                column: "PurchaseRequestsId",
                principalTable: "PurchaseRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_SalesPropositions_SalesPropositionID",
                table: "SalesPropositionLines",
                column: "SalesPropositionID",
                principalTable: "SalesPropositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositionLines_UnitSets_UnitSetID",
                table: "SalesPropositionLines",
                column: "UnitSetID",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositions_Branches_BranchID",
                table: "SalesPropositions",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositions_Currencies_CurrencyID",
                table: "SalesPropositions",
                column: "CurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositions_CurrentAccountCards_CurrentAccountCardID",
                table: "SalesPropositions",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositions_PaymentPlans_PaymentPlanID",
                table: "SalesPropositions",
                column: "PaymentPlanID",
                principalTable: "PaymentPlans",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositions_ShippingAdresses_ShippingAdressID",
                table: "SalesPropositions",
                column: "ShippingAdressID",
                principalTable: "ShippingAdresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesPropositions_Warehouses_WarehouseID",
                table: "SalesPropositions",
                column: "WarehouseID",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShiftLines_Shifts_ShiftID",
                table: "ShiftLines",
                column: "ShiftID",
                principalTable: "Shifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShippingAdresses_CurrentAccountCards_CustomerCardID",
                table: "ShippingAdresses",
                column: "CustomerCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StationInventories_Stations_StationID",
                table: "StationInventories",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_StationGroups_GroupID",
                table: "Stations",
                column: "GroupID",
                principalTable: "StationGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalDrawings_Products_ProductID",
                table: "TechnicalDrawings",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateOperationLines_Stations_StationID",
                table: "TemplateOperationLines",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateOperationLines_TemplateOperations_TemplateOperationID",
                table: "TemplateOperationLines",
                column: "TemplateOperationID",
                principalTable: "TemplateOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnplannedMaintenanceLines_Products_ProductID",
                table: "UnplannedMaintenanceLines",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnplannedMaintenanceLines_UnitSets_UnitSetID",
                table: "UnplannedMaintenanceLines",
                column: "UnitSetID",
                principalTable: "UnitSets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnplannedMaintenanceLines_UnplannedMaintenances_UnplannedMaintenanceID",
                table: "UnplannedMaintenanceLines",
                column: "UnplannedMaintenanceID",
                principalTable: "UnplannedMaintenances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UnplannedMaintenances_MaintenancePeriods_PeriodID",
                table: "UnplannedMaintenances",
                column: "PeriodID",
                principalTable: "MaintenancePeriods",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UnplannedMaintenances_Stations_StationID",
                table: "UnplannedMaintenances",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserGroups_GroupID",
                table: "Users",
                column: "GroupID",
                principalTable: "UserGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_CurrentAccountCards_CurrentAccountCardID",
                table: "WorkOrders",
                column: "CurrentAccountCardID",
                principalTable: "CurrentAccountCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_ProductionOrders_ProductionOrderID",
                table: "WorkOrders",
                column: "ProductionOrderID",
                principalTable: "ProductionOrders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Products_ProductID",
                table: "WorkOrders",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_ProductsOperations_ProductsOperationID",
                table: "WorkOrders",
                column: "ProductsOperationID",
                principalTable: "ProductsOperations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Routes_RouteID",
                table: "WorkOrders",
                column: "RouteID",
                principalTable: "Routes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_SalesPropositions_PropositionID",
                table: "WorkOrders",
                column: "PropositionID",
                principalTable: "SalesPropositions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_StationGroups_StationGroupID",
                table: "WorkOrders",
                column: "StationGroupID",
                principalTable: "StationGroups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkOrders_Stations_StationID",
                table: "WorkOrders",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id");
        }
    }
}
