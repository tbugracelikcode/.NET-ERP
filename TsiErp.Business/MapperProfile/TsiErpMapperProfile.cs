using AutoMapper;
using TsiErp.Entities.Entities.CostManagement.CostPeriod;
using TsiErp.Entities.Entities.CostManagement.CostPeriod.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPR;
using TsiErp.Entities.Entities.CostManagement.CPR.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRManufacturingCostLine;
using TsiErp.Entities.Entities.CostManagement.CPRManufacturingCostLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRMaterialCostLine;
using TsiErp.Entities.Entities.CostManagement.CPRMaterialCostLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.CPRSetupCostLine;
using TsiErp.Entities.Entities.CostManagement.CPRSetupCostLine.Dtos;
using TsiErp.Entities.Entities.CostManagement.StandartStationCostRecord;
using TsiErp.Entities.Entities.CostManagement.StandartStationCostRecord.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount;
using TsiErp.Entities.Entities.FinanceManagement.BankAccount.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan;
using TsiErp.Entities.Entities.FinanceManagement.CashFlowPlan.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan;
using TsiErp.Entities.Entities.FinanceManagement.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Currency.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ExchangeRate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.FinanceManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.GeneralParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.GeneralParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MaintenanceManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MaintenanceManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.NotificationTemplate.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Period.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PlanningManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ProductionManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.PurchaseManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.QualityControlParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.QualityControlParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Shift.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShiftLine;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShiftLine.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShippingManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.ShippingManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.StockManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.LeanProduction.GeneralOEE;
using TsiErp.Entities.Entities.LeanProduction.GeneralOEE.Dtos;
using TsiErp.Entities.Entities.LeanProduction.OEEDetail;
using TsiErp.Entities.Entities.LeanProduction.OEEDetail.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeOperation;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeOperation.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoring.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeScoringLine.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.GeneralSkillRecordPriority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalary.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalaryLine;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StartingSalaryLine.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancy;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancy.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyHistory;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyHistory.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyLine;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancyLine.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRP.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRPLine;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceMRPLine.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenancePeriod.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.MaintenanceManagement.PlannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenance;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenance.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.MaintenanceManagement.UnplannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.Other.ByDateStockMovement;
using TsiErp.Entities.Entities.Other.ByDateStockMovement.Dtos;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement;
using TsiErp.Entities.Entities.Other.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.Other.Notification;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.Calendar;
using TsiErp.Entities.Entities.PlanningManagement.Calendar.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRP;
using TsiErp.Entities.Entities.PlanningManagement.MRP.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPII;
using TsiErp.Entities.Entities.PlanningManagement.MRPII.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPIILine;
using TsiErp.Entities.Entities.PlanningManagement.MRPIILine.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine;
using TsiErp.Entities.Entities.PlanningManagement.MRPLine.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanning.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine;
using TsiErp.Entities.Entities.PlanningManagement.ShipmentPlanningLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheAmountEntryLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.OperationQuantityInformation;
using TsiErp.Entities.Entities.ProductionManagement.OperationQuantityInformation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine;
using TsiErp.Entities.Entities.ProductionManagement.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.Route;
using TsiErp.Entities.Entities.ProductionManagement.Route.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine;
using TsiErp.Entities.Entities.ProductionManagement.RouteLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine;
using TsiErp.Entities.Entities.ProductionManagement.TemplateOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoice;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoice.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoiceLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseInvoiceLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePrice;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePrice.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchasePriceLine.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine;
using TsiErp.Entities.Entities.PurchaseManagement.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.CalibrationRecord;
using TsiErp.Entities.Entities.QualityControl.CalibrationRecord.Dtos;
using TsiErp.Entities.Entities.QualityControl.CalibrationVerification;
using TsiErp.Entities.Entities.QualityControl.CalibrationVerification.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractOperationPicture;
using TsiErp.Entities.Entities.QualityControl.ContractOperationPicture.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlCondition;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.Entities.QualityControl.CustomerComplaintReport;
using TsiErp.Entities.Entities.QualityControl.CustomerComplaintReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord.Dtos;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalSPCLine;
using TsiErp.Entities.Entities.QualityControl.OperationalSPCLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationPicture;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.PFMEA;
using TsiErp.Entities.Entities.QualityControl.PFMEA.Dtos;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport;
using TsiErp.Entities.Entities.QualityControl.ProductionOrderChangeReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine;
using TsiErp.Entities.Entities.QualityControl.PurchaseOrdersAwaitingApprovalLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.Report8D;
using TsiErp.Entities.Entities.QualityControl.Report8D.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;
using TsiErp.Entities.Entities.SalesManagement.Forecast;
using TsiErp.Entities.Entities.SalesManagement.Forecast.Dtos;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecord.Dtos;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine;
using TsiErp.Entities.Entities.SalesManagement.OrderAcceptanceRecordLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoice;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoice.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoiceLine;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoiceLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPrice;
using TsiErp.Entities.Entities.SalesManagement.SalesPrice.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPriceLine;
using TsiErp.Entities.Entities.SalesManagement.SalesPriceLine.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition;
using TsiErp.Entities.Entities.SalesManagement.SalesProposition.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine;
using TsiErp.Entities.Entities.SalesManagement.SalesPropositionLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche;
using TsiErp.Entities.Entities.ShippingManagement.PackageFiche.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine;
using TsiErp.Entities.Entities.ShippingManagement.PackageFicheLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingList;
using TsiErp.Entities.Entities.ShippingManagement.PackingList.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletCubageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine;
using TsiErp.Entities.Entities.ShippingManagement.PackingListPalletPackageLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductCost;
using TsiErp.Entities.Entities.StockManagement.ProductCost.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductProperty;
using TsiErp.Entities.Entities.StockManagement.ProductProperty.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddress;
using TsiErp.Entities.Entities.StockManagement.StockAddress.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockAddressLine;
using TsiErp.Entities.Entities.StockManagement.StockAddressLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockColumn;
using TsiErp.Entities.Entities.StockManagement.StockColumn.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockNumber;
using TsiErp.Entities.Entities.StockManagement.StockNumber.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockSection;
using TsiErp.Entities.Entities.StockManagement.StockSection.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockShelf;
using TsiErp.Entities.Entities.StockManagement.StockShelf.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Entities.TestManagement.City;
using TsiErp.Entities.Entities.TestManagement.City.Dtos;
using TsiErp.Entities.Entities.TestManagement.Continent;
using TsiErp.Entities.Entities.TestManagement.Continent.Dtos;
using TsiErp.Entities.Entities.TestManagement.ContinentLine;
using TsiErp.Entities.Entities.TestManagement.ContinentLine.Dtos;
using TsiErp.Entities.Entities.TestManagement.District;
using TsiErp.Entities.Entities.TestManagement.District.Dtos;
using TsiErp.Entities.Entities.TestManagement.Sector;
using TsiErp.Entities.Entities.TestManagement.Sector.Dtos;
using TsiErp.Entities.Entities.TestManagement.SectorLine;
using TsiErp.Entities.Entities.TestManagement.SectorLine.Dtos;


namespace TsiErp.Business.MapperProfile
{
    public class TsiErpMapperProfile : Profile
    {
        public TsiErpMapperProfile()
        {
            CreateMap<UnitSets, SelectUnitSetsDto>();
            CreateMap<UnitSets, ListUnitSetsDto>();
            CreateMap<CreateUnitSetsDto, UnitSets>();
            CreateMap<SelectUnitSetsDto, CreateUnitSetsDto>();
            CreateMap<UpdateUnitSetsDto, UnitSets>();
            CreateMap<SelectUnitSetsDto, UpdateUnitSetsDto>();

            CreateMap<GeneralOEEs, SelectGeneralOEEsDto>();
            CreateMap<GeneralOEEs, ListGeneralOEEsDto>();
            CreateMap<CreateGeneralOEEsDto, GeneralOEEs>();
            CreateMap<SelectGeneralOEEsDto, CreateGeneralOEEsDto>();
            CreateMap<UpdateGeneralOEEsDto, GeneralOEEs>();
            CreateMap<SelectGeneralOEEsDto, UpdateGeneralOEEsDto>();

            CreateMap<OEEDetails, SelectOEEDetailsDto>();
            CreateMap<OEEDetails, ListOEEDetailsDto>();
            CreateMap<CreateOEEDetailsDto, OEEDetails>();
            CreateMap<SelectOEEDetailsDto, CreateOEEDetailsDto>();
            CreateMap<UpdateOEEDetailsDto, OEEDetails>();
            CreateMap<SelectOEEDetailsDto, UpdateOEEDetailsDto>();


            CreateMap<StandartStationCostRecords, SelectStandartStationCostRecordsDto>();
            CreateMap<StandartStationCostRecords, ListStandartStationCostRecordsDto>();
            CreateMap<CreateStandartStationCostRecordsDto, StandartStationCostRecords>();
            CreateMap<SelectStandartStationCostRecordsDto, CreateStandartStationCostRecordsDto>();
            CreateMap<UpdateStandartStationCostRecordsDto, StandartStationCostRecords>();
            CreateMap<SelectStandartStationCostRecordsDto, UpdateStandartStationCostRecordsDto>();


            CreateMap<CostPeriods, SelectCostPeriodsDto>();
            CreateMap<CostPeriods, ListCostPeriodsDto>();
            CreateMap<CreateCostPeriodsDto, CostPeriods>();
            CreateMap<SelectCostPeriodsDto, CreateCostPeriodsDto>();
            CreateMap<UpdateCostPeriodsDto, CostPeriods>();
            CreateMap<SelectCostPeriodsDto, UpdateCostPeriodsDto>();

            CreateMap<StockColumns, SelectStockColumnsDto>();
            CreateMap<StockColumns, ListStockColumnsDto>();
            CreateMap<CreateStockColumnsDto, StockColumns>();
            CreateMap<SelectStockColumnsDto, CreateStockColumnsDto>();
            CreateMap<UpdateStockColumnsDto, StockColumns>();
            CreateMap<SelectStockColumnsDto, UpdateStockColumnsDto>();

            CreateMap<StockNumbers, SelectStockNumbersDto>();
            CreateMap<StockNumbers, ListStockNumbersDto>();
            CreateMap<CreateStockNumbersDto, StockNumbers>();
            CreateMap<SelectStockNumbersDto, CreateStockNumbersDto>();
            CreateMap<UpdateStockNumbersDto, StockNumbers>();
            CreateMap<SelectStockNumbersDto, UpdateStockNumbersDto>();

            CreateMap<ProductReceiptTransactions, SelectProductReceiptTransactionsDto>();
            CreateMap<ProductReceiptTransactions, ListProductReceiptTransactionsDto>();
            CreateMap<CreateProductReceiptTransactionsDto, ProductReceiptTransactions>();
            CreateMap<SelectProductReceiptTransactionsDto, CreateProductReceiptTransactionsDto>();
            CreateMap<UpdateProductReceiptTransactionsDto, ProductReceiptTransactions>();
            CreateMap<SelectProductReceiptTransactionsDto, UpdateProductReceiptTransactionsDto>();

            CreateMap<StockSections, SelectStockSectionsDto>();
            CreateMap<StockSections, ListStockSectionsDto>();
            CreateMap<CreateStockSectionsDto, StockSections>();
            CreateMap<SelectStockSectionsDto, CreateStockSectionsDto>();
            CreateMap<UpdateStockSectionsDto, StockSections>();
            CreateMap<SelectStockSectionsDto, UpdateStockSectionsDto>();

            CreateMap<StockShelfs, SelectStockShelfsDto>();
            CreateMap<StockShelfs, ListStockShelfsDto>();
            CreateMap<CreateStockShelfsDto, StockShelfs>();
            CreateMap<SelectStockShelfsDto, CreateStockShelfsDto>();
            CreateMap<UpdateStockShelfsDto, StockShelfs>();
            CreateMap<SelectStockShelfsDto, UpdateStockShelfsDto>();

            CreateMap<ProductCosts, SelectProductCostsDto>();
            CreateMap<ProductCosts, ListProductCostsDto>();
            CreateMap<CreateProductCostsDto, ProductCosts>();
            CreateMap<SelectProductCostsDto, CreateProductCostsDto>();
            CreateMap<UpdateProductCostsDto, ProductCosts>();
            CreateMap<SelectProductCostsDto, UpdateProductCostsDto>();

            CreateMap<EmployeeSeniorities, SelectEmployeeSenioritiesDto>();
            CreateMap<EmployeeSeniorities, ListEmployeeSenioritiesDto>();
            CreateMap<CreateEmployeeSenioritiesDto, EmployeeSeniorities>();
            CreateMap<SelectEmployeeSenioritiesDto, CreateEmployeeSenioritiesDto>();
            CreateMap<UpdateEmployeeSenioritiesDto, EmployeeSeniorities>();
            CreateMap<SelectEmployeeSenioritiesDto, UpdateEmployeeSenioritiesDto>();

            CreateMap<EducationLevelScores, SelectEducationLevelScoresDto>();
            CreateMap<EducationLevelScores, ListEducationLevelScoresDto>();
            CreateMap<CreateEducationLevelScoresDto, EducationLevelScores>();
            CreateMap<SelectEducationLevelScoresDto, CreateEducationLevelScoresDto>();
            CreateMap<UpdateEducationLevelScoresDto, EducationLevelScores>();
            CreateMap<SelectEducationLevelScoresDto, UpdateEducationLevelScoresDto>();

            CreateMap<TaskScorings, SelectTaskScoringsDto>();
            CreateMap<TaskScorings, ListTaskScoringsDto>();
            CreateMap<CreateTaskScoringsDto, TaskScorings>();
            CreateMap<SelectTaskScoringsDto, CreateTaskScoringsDto>();
            CreateMap<UpdateTaskScoringsDto, TaskScorings>();
            CreateMap<SelectTaskScoringsDto, UpdateTaskScoringsDto>();

            CreateMap<GeneralSkillRecordPriorities, SelectGeneralSkillRecordPrioritiesDto>();
            CreateMap<GeneralSkillRecordPriorities, ListGeneralSkillRecordPrioritiesDto>();
            CreateMap<CreateGeneralSkillRecordPrioritiesDto, GeneralSkillRecordPriorities>();
            CreateMap<SelectGeneralSkillRecordPrioritiesDto, CreateGeneralSkillRecordPrioritiesDto>();
            CreateMap<UpdateGeneralSkillRecordPrioritiesDto, GeneralSkillRecordPriorities>();
            CreateMap<SelectGeneralSkillRecordPrioritiesDto, UpdateGeneralSkillRecordPrioritiesDto>();

            CreateMap<EmployeeGeneralSkillRecords, SelectEmployeeGeneralSkillRecordsDto>();
            CreateMap<EmployeeGeneralSkillRecords, ListEmployeeGeneralSkillRecordsDto>();
            CreateMap<CreateEmployeeGeneralSkillRecordsDto, EmployeeGeneralSkillRecords>();
            CreateMap<SelectEmployeeGeneralSkillRecordsDto, CreateEmployeeGeneralSkillRecordsDto>();
            CreateMap<UpdateEmployeeGeneralSkillRecordsDto, EmployeeGeneralSkillRecords>();
            CreateMap<SelectEmployeeGeneralSkillRecordsDto, UpdateEmployeeGeneralSkillRecordsDto>();

            CreateMap<EmployeeAnnualSeniorityDifferences, SelectEmployeeAnnualSeniorityDifferencesDto>();
            CreateMap<EmployeeAnnualSeniorityDifferences, ListEmployeeAnnualSeniorityDifferencesDto>();
            CreateMap<CreateEmployeeAnnualSeniorityDifferencesDto, EmployeeAnnualSeniorityDifferences>();
            CreateMap<SelectEmployeeAnnualSeniorityDifferencesDto, CreateEmployeeAnnualSeniorityDifferencesDto>();
            CreateMap<UpdateEmployeeAnnualSeniorityDifferencesDto, EmployeeAnnualSeniorityDifferences>();
            CreateMap<SelectEmployeeAnnualSeniorityDifferencesDto, UpdateEmployeeAnnualSeniorityDifferencesDto>();

            CreateMap<CustomerComplaintReports, SelectCustomerComplaintReportsDto>();
            CreateMap<CustomerComplaintReports, ListCustomerComplaintReportsDto>();
            CreateMap<CreateCustomerComplaintReportsDto, CustomerComplaintReports>();
            CreateMap<SelectCustomerComplaintReportsDto, CreateCustomerComplaintReportsDto>();
            CreateMap<UpdateCustomerComplaintReportsDto, CustomerComplaintReports>();
            CreateMap<SelectCustomerComplaintReportsDto, UpdateCustomerComplaintReportsDto>();

            CreateMap<ProductionOrderChangeReports, SelectProductionOrderChangeReportsDto>();
            CreateMap<ProductionOrderChangeReports, ListProductionOrderChangeReportsDto>();
            CreateMap<CreateProductionOrderChangeReportsDto, ProductionOrderChangeReports>();
            CreateMap<SelectProductionOrderChangeReportsDto, CreateProductionOrderChangeReportsDto>();
            CreateMap<UpdateProductionOrderChangeReportsDto, ProductionOrderChangeReports>();
            CreateMap<SelectProductionOrderChangeReportsDto, UpdateProductionOrderChangeReportsDto>();

            CreateMap<Report8Ds, SelectReport8DsDto>();
            CreateMap<Report8Ds, ListReport8DsDto>();
            CreateMap<CreateReport8DsDto, Report8Ds>();
            CreateMap<SelectReport8DsDto, CreateReport8DsDto>();
            CreateMap<UpdateReport8DsDto, Report8Ds>();
            CreateMap<SelectReport8DsDto, UpdateReport8DsDto>();


            CreateMap<CashFlowPlans, SelectCashFlowPlansDto>();
            CreateMap<CashFlowPlans, ListCashFlowPlansDto>();
            CreateMap<CreateCashFlowPlansDto, CashFlowPlans>();
            CreateMap<SelectCashFlowPlansDto, CreateCashFlowPlansDto>();
            CreateMap<UpdateCashFlowPlansDto, CashFlowPlans>();
            CreateMap<SelectCashFlowPlansDto, UpdateCashFlowPlansDto>();


            CreateMap<MaintenancePeriods, SelectMaintenancePeriodsDto>();
            CreateMap<MaintenancePeriods, ListMaintenancePeriodsDto>();
            CreateMap<CreateMaintenancePeriodsDto, MaintenancePeriods>();
            CreateMap<SelectMaintenancePeriodsDto, CreateMaintenancePeriodsDto>();
            CreateMap<UpdateMaintenancePeriodsDto, MaintenancePeriods>();
            CreateMap<SelectMaintenancePeriodsDto, UpdateMaintenancePeriodsDto>();

            CreateMap<Branches, SelectBranchesDto>();
            CreateMap<Branches, ListBranchesDto>();
            CreateMap<CreateBranchesDto, Branches>();
            CreateMap<SelectBranchesDto, CreateBranchesDto>();
            CreateMap<UpdateBranchesDto, Branches>();
            CreateMap<SelectBranchesDto, UpdateBranchesDto>();
            CreateMap<Branches, UpdateBranchesDto>();

            CreateMap<StationInventories, SelectStationInventoriesDto>();
            CreateMap<StationInventories, ListStationInventoriesDto>();
            CreateMap<CreateStationInventoriesDto, StationInventories>();
            CreateMap<SelectStationInventoriesDto, CreateStationInventoriesDto>();
            CreateMap<UpdateStationInventoriesDto, StationInventories>();
            CreateMap<SelectStationInventoriesDto, UpdateStationInventoriesDto>();


            CreateMap<ProductReferanceNumbers, SelectProductReferanceNumbersDto>();
            CreateMap<ProductReferanceNumbers, ListProductReferanceNumbersDto>();
            CreateMap<CreateProductReferanceNumbersDto, ProductReferanceNumbers>();
            CreateMap<SelectProductReferanceNumbersDto, CreateProductReferanceNumbersDto>();
            CreateMap<UpdateProductReferanceNumbersDto, ProductReferanceNumbers>();
            CreateMap<SelectProductReferanceNumbersDto, UpdateProductReferanceNumbersDto>();

            CreateMap<TechnicalDrawings, SelectTechnicalDrawingsDto>();
            CreateMap<TechnicalDrawings, ListTechnicalDrawingsDto>();
            CreateMap<CreateTechnicalDrawingsDto, TechnicalDrawings>();
            CreateMap<SelectTechnicalDrawingsDto, CreateTechnicalDrawingsDto>();
            CreateMap<UpdateTechnicalDrawingsDto, TechnicalDrawings>();
            CreateMap<SelectTechnicalDrawingsDto, UpdateTechnicalDrawingsDto>();

            CreateMap<CalibrationRecords, SelectCalibrationRecordsDto>();
            CreateMap<CalibrationRecords, ListCalibrationRecordsDto>();
            CreateMap<CreateCalibrationRecordsDto, CalibrationRecords>();
            CreateMap<SelectCalibrationRecordsDto, CreateCalibrationRecordsDto>();
            CreateMap<UpdateCalibrationRecordsDto, CalibrationRecords>();
            CreateMap<SelectCalibrationRecordsDto, UpdateCalibrationRecordsDto>();

            CreateMap<CalibrationVerifications, SelectCalibrationVerificationsDto>();
            CreateMap<CalibrationVerifications, ListCalibrationVerificationsDto>();
            CreateMap<CreateCalibrationVerificationsDto, CalibrationVerifications>();
            CreateMap<SelectCalibrationVerificationsDto, CreateCalibrationVerificationsDto>();
            CreateMap<UpdateCalibrationVerificationsDto, CalibrationVerifications>();
            CreateMap<SelectCalibrationVerificationsDto, UpdateCalibrationVerificationsDto>();

            CreateMap<Currencies, SelectCurrenciesDto>();
            CreateMap<Currencies, ListCurrenciesDto>();
            CreateMap<CreateCurrenciesDto, Currencies>();
            CreateMap<SelectCurrenciesDto, CreateCurrenciesDto>();
            CreateMap<UpdateCurrenciesDto, Currencies>();
            CreateMap<SelectCurrenciesDto, UpdateCurrenciesDto>();

            CreateMap<StationOccupancyHistories, SelectStationOccupancyHistoriesDto>();
            CreateMap<StationOccupancyHistories, ListStationOccupancyHistoriesDto>();
            CreateMap<CreateStationOccupancyHistoriesDto, StationOccupancyHistories>();
            CreateMap<SelectStationOccupancyHistoriesDto, CreateStationOccupancyHistoriesDto>();
            CreateMap<UpdateStationOccupancyHistoriesDto, StationOccupancyHistories>();
            CreateMap<SelectStationOccupancyHistoriesDto, UpdateStationOccupancyHistoriesDto>();

            CreateMap<StationOccupancies, SelectStationOccupanciesDto>();
            CreateMap<StationOccupancies, ListStationOccupanciesDto>();
            CreateMap<CreateStationOccupanciesDto, StationOccupancies>();
            CreateMap<SelectStationOccupanciesDto, CreateStationOccupanciesDto>();
            CreateMap<UpdateStationOccupanciesDto, StationOccupancies>();
            CreateMap<SelectStationOccupanciesDto, UpdateStationOccupanciesDto>();


            CreateMap<StationOccupancyLines, SelectStationOccupancyLinesDto>();
            CreateMap<StationOccupancyLines, ListStationOccupancyLinesDto>();
            CreateMap<CreateStationOccupancyLinesDto, StationOccupancyLines>();
            CreateMap<SelectStationOccupancyLinesDto, CreateStationOccupancyLinesDto>();
            CreateMap<UpdateStationOccupancyLinesDto, StationOccupancyLines>();
            CreateMap<SelectStationOccupancyLinesDto, UpdateStationOccupancyLinesDto>();
            CreateMap<SelectStationOccupancyLinesDto, StationOccupancyLines>();

            CreateMap<CurrentAccountCards, SelectCurrentAccountCardsDto>();
            CreateMap<CurrentAccountCards, ListCurrentAccountCardsDto>();
            CreateMap<CreateCurrentAccountCardsDto, CurrentAccountCards>();
            CreateMap<SelectCurrentAccountCardsDto, CreateCurrentAccountCardsDto>();
            CreateMap<UpdateCurrentAccountCardsDto, CurrentAccountCards>();
            CreateMap<SelectCurrentAccountCardsDto, UpdateCurrentAccountCardsDto>();

            CreateMap<Departments, SelectDepartmentsDto>();
            CreateMap<Departments, ListDepartmentsDto>();
            CreateMap<CreateDepartmentsDto, Departments>();
            CreateMap<SelectDepartmentsDto, CreateDepartmentsDto>();
            CreateMap<UpdateDepartmentsDto, Departments>();
            CreateMap<SelectDepartmentsDto, UpdateDepartmentsDto>();

            CreateMap<BankAccounts, SelectBankAccountsDto>();
            CreateMap<BankAccounts, ListBankAccountsDto>();
            CreateMap<CreateBankAccountsDto, BankAccounts>();
            CreateMap<SelectBankAccountsDto, CreateBankAccountsDto>();
            CreateMap<UpdateBankAccountsDto, BankAccounts>();
            CreateMap<SelectBankAccountsDto, UpdateBankAccountsDto>();

            CreateMap<Employees, SelectEmployeesDto>();
            CreateMap<Employees, ListEmployeesDto>();
            CreateMap<CreateEmployeesDto, Employees>();
            CreateMap<SelectEmployeesDto, CreateEmployeesDto>();
            CreateMap<UpdateEmployeesDto, Employees>();
            CreateMap<SelectEmployeesDto, UpdateEmployeesDto>();

            CreateMap<EquipmentRecords, SelectEquipmentRecordsDto>();
            CreateMap<EquipmentRecords, ListEquipmentRecordsDto>();
            CreateMap<CreateEquipmentRecordsDto, EquipmentRecords>();
            CreateMap<SelectEquipmentRecordsDto, CreateEquipmentRecordsDto>();
            CreateMap<UpdateEquipmentRecordsDto, EquipmentRecords>();
            CreateMap<SelectEquipmentRecordsDto, UpdateEquipmentRecordsDto>();

            CreateMap<ExchangeRates, SelectExchangeRatesDto>();
            CreateMap<ExchangeRates, ListExchangeRatesDto>();
            CreateMap<CreateExchangeRatesDto, ExchangeRates>();
            CreateMap<SelectExchangeRatesDto, CreateExchangeRatesDto>();
            CreateMap<UpdateExchangeRatesDto, ExchangeRates>();
            CreateMap<SelectExchangeRatesDto, UpdateExchangeRatesDto>();

            CreateMap<PaymentPlans, SelectPaymentPlansDto>();
            CreateMap<PaymentPlans, ListPaymentPlansDto>();
            CreateMap<CreatePaymentPlansDto, PaymentPlans>();
            CreateMap<SelectPaymentPlansDto, CreatePaymentPlansDto>();
            CreateMap<UpdatePaymentPlansDto, PaymentPlans>();
            CreateMap<SelectPaymentPlansDto, UpdatePaymentPlansDto>();

            CreateMap<Periods, SelectPeriodsDto>();
            CreateMap<Periods, ListPeriodsDto>();
            CreateMap<CreatePeriodsDto, Periods>();
            CreateMap<SelectPeriodsDto, CreatePeriodsDto>();
            CreateMap<UpdatePeriodsDto, Periods>();
            CreateMap<SelectPeriodsDto, UpdatePeriodsDto>();

            CreateMap<Products, SelectProductsDto>();
            CreateMap<Products, ListProductsDto>();
            CreateMap<CreateProductsDto, Products>();
            CreateMap<SelectProductsDto, CreateProductsDto>();
            CreateMap<SelectProductsDto, ListProductsDto>();
            CreateMap<UpdateProductsDto, Products>();
            CreateMap<SelectProductsDto, UpdateProductsDto>();

            CreateMap<Stations, SelectStationsDto>();
            CreateMap<Stations, ListStationsDto>();
            CreateMap<CreateStationsDto, Stations>();
            CreateMap<SelectStationsDto, CreateStationsDto>();
            CreateMap<UpdateStationsDto, Stations>();
            CreateMap<SelectStationsDto, UpdateStationsDto>();

            CreateMap<StationGroups, SelectStationGroupsDto>();
            CreateMap<StationGroups, ListStationGroupsDto>();
            CreateMap<CreateStationGroupsDto, StationGroups>();
            CreateMap<SelectStationGroupsDto, CreateStationGroupsDto>();
            CreateMap<UpdateStationGroupsDto, StationGroups>();
            CreateMap<SelectStationGroupsDto, UpdateStationGroupsDto>();


            CreateMap<SalesPropositions, SelectSalesPropositionsDto>();
            CreateMap<SalesPropositions, ListSalesPropositionsDto>();
            CreateMap<UpdateSalesPropositionsDto, SalesPropositions>();
            CreateMap<CreateSalesPropositionsDto, SalesPropositions>();
            CreateMap<SelectSalesPropositionsDto, CreateSalesPropositionsDto>();
            CreateMap<SelectSalesPropositionsDto, UpdateSalesPropositionsDto>();


            CreateMap<SalesPropositionLines, SelectSalesPropositionLinesDto>();
            CreateMap<SalesPropositionLines, ListSalesPropositionLinesDto>();
            CreateMap<CreateSalesPropositionLinesDto, SalesPropositionLines>();
            CreateMap<SelectSalesPropositionLinesDto, CreateSalesPropositionLinesDto>();
            CreateMap<UpdateSalesPropositionLinesDto, SalesPropositionLines>();
            CreateMap<SelectSalesPropositionLinesDto, UpdateSalesPropositionLinesDto>();
            CreateMap<SelectSalesPropositionLinesDto, SalesPropositionLines>();



            CreateMap<SalesOrders, SelectSalesOrderDto>();
            CreateMap<SalesOrders, ListSalesOrderDto>();
            CreateMap<UpdateSalesOrderDto, SalesOrders>();
            CreateMap<CreateSalesOrderDto, SalesOrders>();
            CreateMap<SelectSalesOrderDto, CreateSalesOrderDto>();
            CreateMap<SelectSalesOrderDto, UpdateSalesOrderDto>();



            CreateMap<SalesOrderLines, SelectSalesOrderLinesDto>();
            CreateMap<SalesOrderLines, ListSalesOrderLinesDto>();
            CreateMap<CreateSalesOrderLinesDto, SalesOrderLines>();
            CreateMap<SelectSalesOrderLinesDto, CreateSalesOrderLinesDto>();
            CreateMap<UpdateSalesOrderLinesDto, SalesOrderLines>();
            CreateMap<SelectSalesOrderLinesDto, UpdateSalesOrderLinesDto>();
            CreateMap<SelectSalesOrderLinesDto, SalesOrderLines>();


            CreateMap<SalesInvoices, SelectSalesInvoiceDto>();
            CreateMap<SalesInvoices, ListSalesInvoiceDto>();
            CreateMap<UpdateSalesInvoiceDto, SalesInvoices>();
            CreateMap<CreateSalesInvoiceDto, SalesInvoices>();
            CreateMap<SelectSalesInvoiceDto, CreateSalesInvoiceDto>();
            CreateMap<SelectSalesInvoiceDto, UpdateSalesInvoiceDto>();



            CreateMap<SalesInvoiceLines, SelectSalesInvoiceLinesDto>();
            CreateMap<SalesInvoiceLines, ListSalesInvoiceLinesDto>();
            CreateMap<CreateSalesInvoiceLinesDto, SalesInvoiceLines>();
            CreateMap<SelectSalesInvoiceLinesDto, CreateSalesInvoiceLinesDto>();
            CreateMap<UpdateSalesInvoiceLinesDto, SalesInvoiceLines>();
            CreateMap<SelectSalesInvoiceLinesDto, UpdateSalesInvoiceLinesDto>();
            CreateMap<SelectSalesInvoiceLinesDto, SalesInvoiceLines>();



            CreateMap<ProductGroups, SelectProductGroupsDto>();
            CreateMap<ProductGroups, ListProductGroupsDto>();
            CreateMap<CreateProductGroupsDto, ProductGroups>();
            CreateMap<SelectProductGroupsDto, CreateProductGroupsDto>();
            CreateMap<UpdateProductGroupsDto, ProductGroups>();
            CreateMap<SelectProductGroupsDto, UpdateProductGroupsDto>();

            CreateMap<ShippingAdresses, SelectShippingAdressesDto>();
            CreateMap<ShippingAdresses, ListShippingAdressesDto>();
            CreateMap<CreateShippingAdressesDto, ShippingAdresses>();
            CreateMap<SelectShippingAdressesDto, CreateShippingAdressesDto>();
            CreateMap<UpdateShippingAdressesDto, ShippingAdresses>();
            CreateMap<SelectShippingAdressesDto, UpdateShippingAdressesDto>();

            CreateMap<Warehouses, SelectWarehousesDto>();
            CreateMap<Warehouses, ListWarehousesDto>();
            CreateMap<CreateWarehousesDto, Warehouses>();
            CreateMap<SelectWarehousesDto, CreateWarehousesDto>();
            CreateMap<UpdateWarehousesDto, Warehouses>();
            CreateMap<SelectWarehousesDto, UpdateWarehousesDto>();


            CreateMap<Routes, SelectRoutesDto>();
            CreateMap<Routes, ListRoutesDto>();
            CreateMap<CreateRoutesDto, Routes>();
            CreateMap<SelectRoutesDto, CreateRoutesDto>();
            CreateMap<UpdateRoutesDto, Routes>();
            CreateMap<SelectRoutesDto, UpdateRoutesDto>();


            CreateMap<RouteLines, SelectRouteLinesDto>();
            CreateMap<RouteLines, ListRouteLinesDto>();
            CreateMap<CreateRouteLinesDto, RouteLines>();
            CreateMap<SelectRouteLinesDto, RouteLines>();
            CreateMap<SelectRouteLinesDto, CreateRouteLinesDto>();
            CreateMap<UpdateRouteLinesDto, RouteLines>();
            CreateMap<SelectRouteLinesDto, UpdateRouteLinesDto>();


            CreateMap<Shifts, SelectShiftsDto>();
            CreateMap<Shifts, ListShiftsDto>();
            CreateMap<UpdateShiftsDto, Shifts>();
            CreateMap<CreateShiftsDto, Shifts>();
            CreateMap<SelectShiftsDto, CreateShiftsDto>();
            CreateMap<SelectShiftsDto, UpdateShiftsDto>();
            CreateMap<Shifts, UpdateShiftsDto>();


            CreateMap<ShiftLines, SelectShiftLinesDto>();
            CreateMap<ShiftLines, ListShiftLinesDto>();
            CreateMap<CreateShiftLinesDto, ShiftLines>();
            CreateMap<SelectShiftLinesDto, CreateShiftLinesDto>();
            CreateMap<UpdateShiftLinesDto, ShiftLines>();
            CreateMap<SelectShiftLinesDto, UpdateShiftLinesDto>();
            CreateMap<SelectShiftLinesDto, ShiftLines>();


            CreateMap<Calendars, SelectCalendarsDto>();
            CreateMap<Calendars, ListCalendarsDto>();
            CreateMap<CreateCalendarsDto, Calendars>();
            CreateMap<SelectCalendarsDto, CreateCalendarsDto>();
            CreateMap<UpdateCalendarsDto, Calendars>();
            CreateMap<SelectCalendarsDto, UpdateCalendarsDto>();

            CreateMap<CalendarDays, SelectCalendarDaysDto>();
            CreateMap<CalendarDays, ListCalendarDaysDto>();
            CreateMap<CreateCalendarDaysDto, CalendarDays>();
            CreateMap<SelectCalendarDaysDto, CreateCalendarDaysDto>();
            CreateMap<UpdateCalendarDaysDto, CalendarDays>();
            CreateMap<SelectCalendarDaysDto, UpdateCalendarDaysDto>();
            CreateMap<SelectCalendarDaysDto, CalendarDays>();

            CreateMap<CalendarLines, SelectCalendarLinesDto>();
            CreateMap<CalendarLines, ListCalendarLinesDto>();
            CreateMap<CreateCalendarLinesDto, CalendarLines>();
            CreateMap<SelectCalendarLinesDto, CreateCalendarLinesDto>();
            CreateMap<UpdateCalendarLinesDto, CalendarLines>();
            CreateMap<SelectCalendarLinesDto, UpdateCalendarLinesDto>();
            CreateMap<SelectCalendarLinesDto, CalendarLines>();


            CreateMap<TemplateOperations, SelectTemplateOperationsDto>();
            CreateMap<TemplateOperations, ListTemplateOperationsDto>();
            CreateMap<UpdateTemplateOperationsDto, TemplateOperations>();
            CreateMap<CreateTemplateOperationsDto, TemplateOperations>();
            CreateMap<SelectTemplateOperationsDto, CreateTemplateOperationsDto>();
            CreateMap<SelectTemplateOperationsDto, UpdateTemplateOperationsDto>();


            CreateMap<TemplateOperationLines, SelectTemplateOperationLinesDto>();
            CreateMap<TemplateOperationLines, ListTemplateOperationLinesDto>();
            CreateMap<CreateTemplateOperationLinesDto, TemplateOperationLines>();
            CreateMap<SelectTemplateOperationLinesDto, CreateTemplateOperationLinesDto>();
            CreateMap<UpdateTemplateOperationLinesDto, TemplateOperationLines>();
            CreateMap<SelectTemplateOperationLinesDto, UpdateTemplateOperationLinesDto>();
            CreateMap<SelectTemplateOperationLinesDto, TemplateOperationLines>();


            CreateMap<ProductsOperations, SelectProductsOperationsDto>();
            CreateMap<ProductsOperations, ListProductsOperationsDto>();
            CreateMap<UpdateProductsOperationsDto, ProductsOperations>();
            CreateMap<CreateProductsOperationsDto, ProductsOperations>();
            CreateMap<SelectProductsOperationsDto, CreateProductsOperationsDto>();
            CreateMap<SelectProductsOperationsDto, UpdateProductsOperationsDto>();


            CreateMap<ProductsOperationLines, SelectProductsOperationLinesDto>();
            CreateMap<ProductsOperationLines, ListProductsOperationLinesDto>();
            CreateMap<CreateProductsOperationLinesDto, ProductsOperationLines>();
            CreateMap<SelectProductsOperationLinesDto, CreateProductsOperationLinesDto>();
            CreateMap<UpdateProductsOperationLinesDto, ProductsOperationLines>();
            CreateMap<SelectProductsOperationLinesDto, UpdateProductsOperationLinesDto>();
            CreateMap<SelectProductsOperationLinesDto, ProductsOperationLines>();


            CreateMap<BillsofMaterials, SelectBillsofMaterialsDto>();
            CreateMap<BillsofMaterials, ListBillsofMaterialsDto>();
            CreateMap<UpdateBillsofMaterialsDto, BillsofMaterials>();
            CreateMap<CreateBillsofMaterialsDto, BillsofMaterials>();
            CreateMap<SelectBillsofMaterialsDto, CreateBillsofMaterialsDto>();
            CreateMap<SelectBillsofMaterialsDto, UpdateBillsofMaterialsDto>();
            CreateMap<BillsofMaterials, UpdateBillsofMaterialsDto>();


            CreateMap<BillsofMaterialLines, SelectBillsofMaterialLinesDto>();
            CreateMap<BillsofMaterialLines, ListBillsofMaterialLinesDto>();
            CreateMap<CreateBillsofMaterialLinesDto, BillsofMaterialLines>();
            CreateMap<SelectBillsofMaterialLinesDto, CreateBillsofMaterialLinesDto>();
            CreateMap<UpdateBillsofMaterialLinesDto, BillsofMaterialLines>();
            CreateMap<SelectBillsofMaterialLinesDto, UpdateBillsofMaterialLinesDto>();
            CreateMap<SelectBillsofMaterialLinesDto, BillsofMaterialLines>();

            CreateMap<ProductionOrders, SelectProductionOrdersDto>();
            CreateMap<ProductionOrders, ListProductionOrdersDto>();
            CreateMap<CreateProductionOrdersDto, ProductionOrders>();
            CreateMap<SelectProductionOrdersDto, CreateProductionOrdersDto>();
            CreateMap<UpdateProductionOrdersDto, ProductionOrders>();
            CreateMap<SelectProductionOrdersDto, UpdateProductionOrdersDto>();


            CreateMap<WorkOrders, SelectWorkOrdersDto>();
            CreateMap<WorkOrders, ListWorkOrdersDto>();
            CreateMap<CreateWorkOrdersDto, WorkOrders>();
            CreateMap<SelectWorkOrdersDto, CreateWorkOrdersDto>();
            CreateMap<UpdateWorkOrdersDto, WorkOrders>();
            CreateMap<SelectWorkOrdersDto, UpdateWorkOrdersDto>();


            CreateMap<PurchaseOrders, SelectPurchaseOrdersDto>();
            CreateMap<PurchaseOrders, ListPurchaseOrdersDto>();
            CreateMap<UpdatePurchaseOrdersDto, PurchaseOrders>();
            CreateMap<CreatePurchaseOrdersDto, PurchaseOrders>();
            CreateMap<SelectPurchaseOrdersDto, CreatePurchaseOrdersDto>();
            CreateMap<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>();


            CreateMap<PurchaseOrderLines, SelectPurchaseOrderLinesDto>();
            CreateMap<PurchaseOrderLines, ListPurchaseOrderLinesDto>();
            CreateMap<CreatePurchaseOrderLinesDto, PurchaseOrderLines>();
            CreateMap<SelectPurchaseOrderLinesDto, CreatePurchaseOrderLinesDto>();
            CreateMap<UpdatePurchaseOrderLinesDto, PurchaseOrderLines>();
            CreateMap<SelectPurchaseOrderLinesDto, UpdatePurchaseOrderLinesDto>();
            CreateMap<SelectPurchaseOrderLinesDto, PurchaseOrderLines>();


            CreateMap<PurchaseInvoices, SelectPurchaseInvoicesDto>();
            CreateMap<PurchaseInvoices, ListPurchaseInvoicesDto>();
            CreateMap<UpdatePurchaseInvoicesDto, PurchaseInvoices>();
            CreateMap<CreatePurchaseInvoicesDto, PurchaseInvoices>();
            CreateMap<SelectPurchaseInvoicesDto, CreatePurchaseInvoicesDto>();
            CreateMap<SelectPurchaseInvoicesDto, UpdatePurchaseInvoicesDto>();

            CreateMap<PurchaseInvoiceLines, SelectPurchaseInvoiceLinesDto>();
            CreateMap<PurchaseInvoiceLines, ListPurchaseInvoiceLinesDto>();
            CreateMap<CreatePurchaseInvoiceLinesDto, PurchaseInvoiceLines>();
            CreateMap<SelectPurchaseInvoiceLinesDto, CreatePurchaseInvoiceLinesDto>();
            CreateMap<UpdatePurchaseInvoiceLinesDto, PurchaseInvoiceLines>();
            CreateMap<SelectPurchaseInvoiceLinesDto, UpdatePurchaseInvoiceLinesDto>();
            CreateMap<SelectPurchaseInvoiceLinesDto, PurchaseInvoiceLines>();

            CreateMap<PurchaseRequests, SelectPurchaseRequestsDto>();
            CreateMap<PurchaseRequests, ListPurchaseRequestsDto>();
            CreateMap<UpdatePurchaseRequestsDto, PurchaseRequests>();
            CreateMap<CreatePurchaseRequestsDto, PurchaseRequests>();
            CreateMap<SelectPurchaseRequestsDto, CreatePurchaseRequestsDto>();
            CreateMap<SelectPurchaseRequestsDto, UpdatePurchaseRequestsDto>();
            CreateMap<SelectPurchaseRequestsDto, PurchaseRequests>();
            CreateMap<UpdatePurchaseRequestsDto, PurchaseRequests>();
            CreateMap<PurchaseRequests, UpdatePurchaseRequestsDto>();


            CreateMap<PurchaseRequestLines, SelectPurchaseRequestLinesDto>();
            CreateMap<PurchaseRequestLines, ListPurchaseRequestLinesDto>();
            CreateMap<CreatePurchaseRequestLinesDto, PurchaseRequestLines>();
            CreateMap<SelectPurchaseRequestLinesDto, CreatePurchaseRequestLinesDto>();
            CreateMap<UpdatePurchaseRequestLinesDto, PurchaseRequestLines>();
            CreateMap<SelectPurchaseRequestLinesDto, UpdatePurchaseRequestLinesDto>();
            CreateMap<SelectPurchaseRequestLinesDto, PurchaseRequestLines>();


            CreateMap<PurchaseUnsuitabilityReports, SelectPurchaseUnsuitabilityReportsDto>();
            CreateMap<PurchaseUnsuitabilityReports, ListPurchaseUnsuitabilityReportsDto>();
            CreateMap<CreatePurchaseUnsuitabilityReportsDto, PurchaseUnsuitabilityReports>();
            CreateMap<SelectPurchaseUnsuitabilityReportsDto, CreatePurchaseUnsuitabilityReportsDto>();
            CreateMap<UpdatePurchaseUnsuitabilityReportsDto, PurchaseUnsuitabilityReports>();
            CreateMap<SelectPurchaseUnsuitabilityReportsDto, UpdatePurchaseUnsuitabilityReportsDto>();

            CreateMap<OperationUnsuitabilityReports, SelectOperationUnsuitabilityReportsDto>();
            CreateMap<OperationUnsuitabilityReports, ListOperationUnsuitabilityReportsDto>();
            CreateMap<CreateOperationUnsuitabilityReportsDto, OperationUnsuitabilityReports>();
            CreateMap<SelectOperationUnsuitabilityReportsDto, CreateOperationUnsuitabilityReportsDto>();
            CreateMap<UpdateOperationUnsuitabilityReportsDto, OperationUnsuitabilityReports>();
            CreateMap<SelectOperationUnsuitabilityReportsDto, UpdateOperationUnsuitabilityReportsDto>();

            CreateMap<ContractUnsuitabilityReports, SelectContractUnsuitabilityReportsDto>();
            CreateMap<ContractUnsuitabilityReports, ListContractUnsuitabilityReportsDto>();
            CreateMap<CreateContractUnsuitabilityReportsDto, ContractUnsuitabilityReports>();
            CreateMap<SelectContractUnsuitabilityReportsDto, CreateContractUnsuitabilityReportsDto>();
            CreateMap<UpdateContractUnsuitabilityReportsDto, ContractUnsuitabilityReports>();
            CreateMap<SelectContractUnsuitabilityReportsDto, UpdateContractUnsuitabilityReportsDto>();

            CreateMap<HaltReasons, SelectHaltReasonsDto>();
            CreateMap<HaltReasons, ListHaltReasonsDto>();
            CreateMap<CreateHaltReasonsDto, HaltReasons>();
            CreateMap<SelectHaltReasonsDto, CreateHaltReasonsDto>();
            CreateMap<UpdateHaltReasonsDto, HaltReasons>();
            CreateMap<SelectHaltReasonsDto, UpdateHaltReasonsDto>();
            CreateMap<SelectHaltReasonsDto, HaltReasons>();

            CreateMap<ProductionTrackings, SelectProductionTrackingsDto>();
            CreateMap<CreateProductionTrackingsDto, ProductionTrackings>();
            CreateMap<SelectProductionTrackingsDto, CreateProductionTrackingsDto>();
            CreateMap<UpdateProductionTrackingsDto, ProductionTrackings>();
            CreateMap<SelectProductionTrackingsDto, UpdateProductionTrackingsDto>();
            CreateMap<SelectProductionTrackingsDto, ProductionTrackings>();


            CreateMap<ContractProductionTrackings, SelectContractProductionTrackingsDto>();
            CreateMap<ContractProductionTrackings, ListContractProductionTrackingsDto>();
            CreateMap<CreateContractProductionTrackingsDto, ContractProductionTrackings>();
            CreateMap<SelectContractProductionTrackingsDto, CreateContractProductionTrackingsDto>();
            CreateMap<UpdateContractProductionTrackingsDto, ContractProductionTrackings>();
            CreateMap<SelectContractProductionTrackingsDto, UpdateContractProductionTrackingsDto>();
            CreateMap<SelectContractProductionTrackingsDto, ContractProductionTrackings>();

            CreateMap<Forecasts, SelectForecastsDto>();
            CreateMap<Forecasts, ListForecastsDto>();
            CreateMap<UpdateForecastsDto, Forecasts>();
            CreateMap<CreateForecastsDto, Forecasts>();
            CreateMap<SelectForecastsDto, CreateForecastsDto>();
            CreateMap<SelectForecastsDto, UpdateForecastsDto>();


            CreateMap<ForecastLines, SelectForecastLinesDto>();
            CreateMap<ForecastLines, ListForecastLinesDto>();
            CreateMap<CreateForecastLinesDto, ForecastLines>();
            CreateMap<SelectForecastLinesDto, CreateForecastLinesDto>();
            CreateMap<UpdateForecastLinesDto, ForecastLines>();
            CreateMap<SelectForecastLinesDto, UpdateForecastLinesDto>();
            CreateMap<SelectForecastLinesDto, ForecastLines>();


            CreateMap<SalesPrices, SelectSalesPricesDto>();
            CreateMap<SalesPrices, ListSalesPricesDto>();
            CreateMap<UpdateSalesPricesDto, SalesPrices>();
            CreateMap<CreateSalesPricesDto, SalesPrices>();
            CreateMap<SelectSalesPricesDto, CreateSalesPricesDto>();
            CreateMap<SelectSalesPricesDto, UpdateSalesPricesDto>();


            CreateMap<SalesPriceLines, SelectSalesPriceLinesDto>();
            CreateMap<SalesPriceLines, ListSalesPriceLinesDto>();
            CreateMap<CreateSalesPriceLinesDto, SalesPriceLines>();
            CreateMap<SelectSalesPriceLinesDto, CreateSalesPriceLinesDto>();
            CreateMap<UpdateSalesPriceLinesDto, SalesPriceLines>();
            CreateMap<SelectSalesPriceLinesDto, UpdateSalesPriceLinesDto>();
            CreateMap<SelectSalesPriceLinesDto, SalesPriceLines>();


            CreateMap<PurchasePrices, SelectPurchasePricesDto>();
            CreateMap<PurchasePrices, ListPurchasePricesDto>();
            CreateMap<UpdatePurchasePricesDto, PurchasePrices>();
            CreateMap<CreatePurchasePricesDto, PurchasePrices>();
            CreateMap<SelectPurchasePricesDto, CreatePurchasePricesDto>();
            CreateMap<SelectPurchasePricesDto, UpdatePurchasePricesDto>();


            CreateMap<PurchasePriceLines, SelectPurchasePriceLinesDto>();
            CreateMap<PurchasePriceLines, ListPurchasePriceLinesDto>();
            CreateMap<CreatePurchasePriceLinesDto, PurchasePriceLines>();
            CreateMap<SelectPurchasePriceLinesDto, CreatePurchasePriceLinesDto>();
            CreateMap<UpdatePurchasePriceLinesDto, PurchasePriceLines>();
            CreateMap<SelectPurchasePriceLinesDto, UpdatePurchasePriceLinesDto>();
            CreateMap<SelectPurchasePriceLinesDto, PurchasePriceLines>();


            CreateMap<UserGroups, SelectUserGroupsDto>();
            CreateMap<UserGroups, ListUserGroupsDto>();
            CreateMap<CreateUserGroupsDto, UserGroups>();
            CreateMap<SelectUserGroupsDto, CreateUserGroupsDto>();
            CreateMap<UpdateUserGroupsDto, UserGroups>();
            CreateMap<SelectUserGroupsDto, UpdateUserGroupsDto>();


            CreateMap<Users, SelectUsersDto>();
            CreateMap<Users, ListUsersDto>();
            CreateMap<CreateUsersDto, Users>();
            CreateMap<SelectUsersDto, CreateUsersDto>();
            CreateMap<UpdateUsersDto, Users>();
            CreateMap<SelectUsersDto, UpdateUsersDto>();

            CreateMap<UserPermissions, SelectUserPermissionsDto>();
            CreateMap<UserPermissions, ListUserPermissionsDto>();
            CreateMap<CreateUserPermissionsDto, UserPermissions>();
            CreateMap<SelectUserPermissionsDto, CreateUserPermissionsDto>();
            CreateMap<UpdateUserPermissionsDto, UserPermissions>();
            CreateMap<SelectUserPermissionsDto, UpdateUserPermissionsDto>();


            CreateMap<OperationQuantityInformations, SelectOperationQuantityInformationsDto>();
            CreateMap<OperationQuantityInformations, ListOperationQuantityInformationsDto>();
            CreateMap<CreateOperationQuantityInformationsDto, OperationQuantityInformations>();
            CreateMap<SelectOperationQuantityInformationsDto, CreateOperationQuantityInformationsDto>();
            CreateMap<UpdateOperationQuantityInformationsDto, OperationQuantityInformations>();
            CreateMap<SelectOperationQuantityInformationsDto, UpdateOperationQuantityInformationsDto>();


            CreateMap<FinalControlUnsuitabilityReports, SelectFinalControlUnsuitabilityReportsDto>();
            CreateMap<FinalControlUnsuitabilityReports, ListFinalControlUnsuitabilityReportsDto>();
            CreateMap<CreateFinalControlUnsuitabilityReportsDto, FinalControlUnsuitabilityReports>();
            CreateMap<SelectFinalControlUnsuitabilityReportsDto, CreateFinalControlUnsuitabilityReportsDto>();
            CreateMap<UpdateFinalControlUnsuitabilityReportsDto, FinalControlUnsuitabilityReports>();
            CreateMap<SelectFinalControlUnsuitabilityReportsDto, UpdateFinalControlUnsuitabilityReportsDto>();



            CreateMap<MaintenanceInstructions, SelectMaintenanceInstructionsDto>();
            CreateMap<MaintenanceInstructions, ListMaintenanceInstructionsDto>();
            CreateMap<UpdateMaintenanceInstructionsDto, MaintenanceInstructions>();
            CreateMap<CreateMaintenanceInstructionsDto, MaintenanceInstructions>();
            CreateMap<SelectMaintenanceInstructionsDto, CreateMaintenanceInstructionsDto>();
            CreateMap<SelectMaintenanceInstructionsDto, UpdateMaintenanceInstructionsDto>();



            CreateMap<MaintenanceInstructionLines, SelectMaintenanceInstructionLinesDto>();
            CreateMap<MaintenanceInstructionLines, ListMaintenanceInstructionLinesDto>();
            CreateMap<CreateMaintenanceInstructionLinesDto, MaintenanceInstructionLines>();
            CreateMap<SelectMaintenanceInstructionLinesDto, CreateMaintenanceInstructionLinesDto>();
            CreateMap<UpdateMaintenanceInstructionLinesDto, MaintenanceInstructionLines>();
            CreateMap<SelectMaintenanceInstructionLinesDto, UpdateMaintenanceInstructionLinesDto>();
            CreateMap<SelectMaintenanceInstructionLinesDto, MaintenanceInstructionLines>();


            CreateMap<PlannedMaintenances, SelectPlannedMaintenancesDto>();
            CreateMap<PlannedMaintenances, ListPlannedMaintenancesDto>();
            CreateMap<UpdatePlannedMaintenancesDto, PlannedMaintenances>();
            CreateMap<CreatePlannedMaintenancesDto, PlannedMaintenances>();
            CreateMap<SelectPlannedMaintenancesDto, CreatePlannedMaintenancesDto>();
            CreateMap<SelectPlannedMaintenancesDto, UpdatePlannedMaintenancesDto>();



            CreateMap<PlannedMaintenanceLines, SelectPlannedMaintenanceLinesDto>();
            CreateMap<PlannedMaintenanceLines, ListPlannedMaintenanceLinesDto>();
            CreateMap<CreatePlannedMaintenanceLinesDto, PlannedMaintenanceLines>();
            CreateMap<SelectPlannedMaintenanceLinesDto, CreatePlannedMaintenanceLinesDto>();
            CreateMap<UpdatePlannedMaintenanceLinesDto, PlannedMaintenanceLines>();
            CreateMap<SelectPlannedMaintenanceLinesDto, UpdatePlannedMaintenanceLinesDto>();
            CreateMap<SelectPlannedMaintenanceLinesDto, PlannedMaintenanceLines>();


            CreateMap<UnplannedMaintenances, SelectUnplannedMaintenancesDto>();
            CreateMap<UnplannedMaintenances, ListUnplannedMaintenancesDto>();
            CreateMap<UpdateUnplannedMaintenancesDto, UnplannedMaintenances>();
            CreateMap<CreateUnplannedMaintenancesDto, UnplannedMaintenances>();
            CreateMap<SelectUnplannedMaintenancesDto, CreateUnplannedMaintenancesDto>();
            CreateMap<SelectUnplannedMaintenancesDto, UpdateUnplannedMaintenancesDto>();



            CreateMap<UnplannedMaintenanceLines, SelectUnplannedMaintenanceLinesDto>();
            CreateMap<UnplannedMaintenanceLines, ListUnplannedMaintenanceLinesDto>();
            CreateMap<CreateUnplannedMaintenanceLinesDto, UnplannedMaintenanceLines>();
            CreateMap<SelectUnplannedMaintenanceLinesDto, CreateUnplannedMaintenanceLinesDto>();
            CreateMap<UpdateUnplannedMaintenanceLinesDto, UnplannedMaintenanceLines>();
            CreateMap<SelectUnplannedMaintenanceLinesDto, UpdateUnplannedMaintenanceLinesDto>();
            CreateMap<SelectUnplannedMaintenanceLinesDto, UnplannedMaintenanceLines>();

            CreateMap<GrandTotalStockMovements, SelectGrandTotalStockMovementsDto>();
            CreateMap<GrandTotalStockMovements, ListGrandTotalStockMovementsDto>();
            CreateMap<CreateGrandTotalStockMovementsDto, GrandTotalStockMovements>();
            CreateMap<SelectGrandTotalStockMovementsDto, CreateGrandTotalStockMovementsDto>();
            CreateMap<UpdateGrandTotalStockMovementsDto, GrandTotalStockMovements>();
            CreateMap<SelectGrandTotalStockMovementsDto, UpdateGrandTotalStockMovementsDto>();


            CreateMap<ByDateStockMovements, SelectByDateStockMovementsDto>();
            CreateMap<ByDateStockMovements, ListByDateStockMovementsDto>();
            CreateMap<CreateByDateStockMovementsDto, ByDateStockMovements>();
            CreateMap<SelectByDateStockMovementsDto, CreateByDateStockMovementsDto>();
            CreateMap<UpdateByDateStockMovementsDto, ByDateStockMovements>();
            CreateMap<ByDateStockMovements, UpdateByDateStockMovementsDto>();
            CreateMap<SelectByDateStockMovementsDto, UpdateByDateStockMovementsDto>();

            CreateMap<StockFiches, SelectStockFichesDto>();
            CreateMap<StockFiches, ListStockFichesDto>();
            CreateMap<UpdateStockFichesDto, StockFiches>();
            CreateMap<CreateStockFichesDto, StockFiches>();
            CreateMap<SelectStockFichesDto, CreateStockFichesDto>();
            CreateMap<SelectStockFichesDto, UpdateStockFichesDto>();
            CreateMap<StockFiches, UpdateStockFichesDto>();


            CreateMap<StockFicheLines, SelectStockFicheLinesDto>();
            CreateMap<StockFicheLines, ListStockFicheLinesDto>();
            CreateMap<CreateStockFicheLinesDto, StockFicheLines>();
            CreateMap<SelectStockFicheLinesDto, CreateStockFicheLinesDto>();
            CreateMap<UpdateStockFicheLinesDto, StockFicheLines>();
            CreateMap<SelectStockFicheLinesDto, UpdateStockFicheLinesDto>();
            CreateMap<SelectStockFicheLinesDto, StockFicheLines>();



            CreateMap<UnsuitabilityTypesItems, SelectUnsuitabilityTypesItemsDto>();
            CreateMap<UnsuitabilityTypesItems, ListUnsuitabilityTypesItemsDto>();
            CreateMap<CreateUnsuitabilityTypesItemsDto, UnsuitabilityTypesItems>();
            CreateMap<SelectUnsuitabilityTypesItemsDto, CreateUnsuitabilityTypesItemsDto>();
            CreateMap<UpdateUnsuitabilityTypesItemsDto, UnsuitabilityTypesItems>();
            CreateMap<SelectUnsuitabilityTypesItemsDto, UpdateUnsuitabilityTypesItemsDto>();
            CreateMap<UnsuitabilityTypesItems, UpdateUnsuitabilityTypesItemsDto>();

            CreateMap<UnsuitabilityItems, SelectUnsuitabilityItemsDto>();
            CreateMap<UnsuitabilityItems, ListUnsuitabilityItemsDto>();
            CreateMap<CreateUnsuitabilityItemsDto, UnsuitabilityItems>();
            CreateMap<SelectUnsuitabilityItemsDto, CreateUnsuitabilityItemsDto>();
            CreateMap<UpdateUnsuitabilityItemsDto, UnsuitabilityItems>();
            CreateMap<SelectUnsuitabilityItemsDto, UpdateUnsuitabilityItemsDto>();

            CreateMap<FinanceManagementParameters, SelectFinanceManagementParametersDto>();
            CreateMap<FinanceManagementParameters, ListFinanceManagementParametersDto>();
            CreateMap<CreateFinanceManagementParametersDto, FinanceManagementParameters>();
            CreateMap<SelectFinanceManagementParametersDto, CreateFinanceManagementParametersDto>();
            CreateMap<UpdateFinanceManagementParametersDto, FinanceManagementParameters>();
            CreateMap<SelectFinanceManagementParametersDto, UpdateFinanceManagementParametersDto>();
            ;

            CreateMap<NotificationTemplates, SelectNotificationTemplatesDto>();
            CreateMap<NotificationTemplates, ListNotificationTemplatesDto>();
            CreateMap<CreateNotificationTemplatesDto, NotificationTemplates>();
            CreateMap<SelectNotificationTemplatesDto, CreateNotificationTemplatesDto>();
            CreateMap<UpdateNotificationTemplatesDto, NotificationTemplates>();
            CreateMap<SelectNotificationTemplatesDto, UpdateNotificationTemplatesDto>();


            CreateMap<Notifications, SelectNotificationsDto>();
            CreateMap<Notifications, ListNotificationsDto>();
            CreateMap<CreateNotificationsDto, Notifications>();
            CreateMap<SelectNotificationsDto, CreateNotificationsDto>();
            CreateMap<UpdateNotificationsDto, Notifications>();
            CreateMap<SelectNotificationsDto, UpdateNotificationsDto>();

            CreateMap<GeneralParameters, SelectGeneralParametersDto>();
            CreateMap<GeneralParameters, ListGeneralParametersDto>();
            CreateMap<CreateGeneralParametersDto, GeneralParameters>();
            CreateMap<SelectGeneralParametersDto, CreateGeneralParametersDto>();
            CreateMap<UpdateGeneralParametersDto, GeneralParameters>();
            CreateMap<SelectGeneralParametersDto, UpdateGeneralParametersDto>();

            CreateMap<MachineAndWorkforceManagementParameters, SelectMachineAndWorkforceManagementParametersDto>();
            CreateMap<MachineAndWorkforceManagementParameters, ListMachineAndWorkforceManagementParametersDto>();
            CreateMap<CreateMachineAndWorkforceManagementParametersDto, MachineAndWorkforceManagementParameters>();
            CreateMap<SelectMachineAndWorkforceManagementParametersDto, CreateMachineAndWorkforceManagementParametersDto>();
            CreateMap<UpdateMachineAndWorkforceManagementParametersDto, MachineAndWorkforceManagementParameters>();
            CreateMap<SelectMachineAndWorkforceManagementParametersDto, UpdateMachineAndWorkforceManagementParametersDto>();

            CreateMap<MaintenanceManagementParameters, SelectMaintenanceManagementParametersDto>();
            CreateMap<MaintenanceManagementParameters, ListMaintenanceManagementParametersDto>();
            CreateMap<CreateMaintenanceManagementParametersDto, MaintenanceManagementParameters>();
            CreateMap<SelectMaintenanceManagementParametersDto, CreateMaintenanceManagementParametersDto>();
            CreateMap<UpdateMaintenanceManagementParametersDto, MaintenanceManagementParameters>();
            CreateMap<SelectMaintenanceManagementParametersDto, UpdateMaintenanceManagementParametersDto>();

            CreateMap<PlanningManagementParameters, SelectPlanningManagementParametersDto>();
            CreateMap<PlanningManagementParameters, ListPlanningManagementParametersDto>();
            CreateMap<CreatePlanningManagementParametersDto, PlanningManagementParameters>();
            CreateMap<SelectPlanningManagementParametersDto, CreatePlanningManagementParametersDto>();
            CreateMap<UpdatePlanningManagementParametersDto, PlanningManagementParameters>();
            CreateMap<SelectPlanningManagementParametersDto, UpdatePlanningManagementParametersDto>();

            CreateMap<ProductionManagementParameters, SelectProductionManagementParametersDto>();
            CreateMap<ProductionManagementParameters, ListProductionManagementParametersDto>();
            CreateMap<CreateProductionManagementParametersDto, ProductionManagementParameters>();
            CreateMap<SelectProductionManagementParametersDto, CreateProductionManagementParametersDto>();
            CreateMap<UpdateProductionManagementParametersDto, ProductionManagementParameters>();
            CreateMap<SelectProductionManagementParametersDto, UpdateProductionManagementParametersDto>();

            CreateMap<PurchaseManagementParameters, SelectPurchaseManagementParametersDto>();
            CreateMap<PurchaseManagementParameters, ListPurchaseManagementParametersDto>();
            CreateMap<CreatePurchaseManagementParametersDto, PurchaseManagementParameters>();
            CreateMap<SelectPurchaseManagementParametersDto, CreatePurchaseManagementParametersDto>();
            CreateMap<UpdatePurchaseManagementParametersDto, PurchaseManagementParameters>();
            CreateMap<SelectPurchaseManagementParametersDto, UpdatePurchaseManagementParametersDto>();

            CreateMap<QualityControlParameters, SelectQualityControlParametersDto>();
            CreateMap<QualityControlParameters, ListQualityControlParametersDto>();
            CreateMap<CreateQualityControlParametersDto, QualityControlParameters>();
            CreateMap<SelectQualityControlParametersDto, CreateQualityControlParametersDto>();
            CreateMap<UpdateQualityControlParametersDto, QualityControlParameters>();
            CreateMap<SelectQualityControlParametersDto, UpdateQualityControlParametersDto>();

            CreateMap<SalesManagementParameters, SelectSalesManagementParametersDto>();
            CreateMap<SalesManagementParameters, ListSalesManagementParametersDto>();
            CreateMap<CreateSalesManagementParametersDto, SalesManagementParameters>();
            CreateMap<SelectSalesManagementParametersDto, CreateSalesManagementParametersDto>();
            CreateMap<UpdateSalesManagementParametersDto, SalesManagementParameters>();
            CreateMap<SelectSalesManagementParametersDto, UpdateSalesManagementParametersDto>();

            CreateMap<ShippingManagementParameters, SelectShippingManagementParametersDto>();
            CreateMap<ShippingManagementParameters, ListShippingManagementParametersDto>();
            CreateMap<CreateShippingManagementParametersDto, ShippingManagementParameters>();
            CreateMap<SelectShippingManagementParametersDto, CreateShippingManagementParametersDto>();
            CreateMap<UpdateShippingManagementParametersDto, ShippingManagementParameters>();
            CreateMap<SelectShippingManagementParametersDto, UpdateShippingManagementParametersDto>();

            CreateMap<StockManagementParameters, SelectStockManagementParametersDto>();
            CreateMap<StockManagementParameters, ListStockManagementParametersDto>();
            CreateMap<CreateStockManagementParametersDto, StockManagementParameters>();
            CreateMap<SelectStockManagementParametersDto, CreateStockManagementParametersDto>();
            CreateMap<UpdateStockManagementParametersDto, StockManagementParameters>();
            CreateMap<SelectStockManagementParametersDto, UpdateStockManagementParametersDto>();



            CreateMap<ControlTypes, SelectControlTypesDto>();
            CreateMap<ControlTypes, ListControlTypesDto>();
            CreateMap<CreateControlTypesDto, ControlTypes>();
            CreateMap<SelectControlTypesDto, CreateControlTypesDto>();
            CreateMap<UpdateControlTypesDto, ControlTypes>();
            CreateMap<SelectControlTypesDto, UpdateControlTypesDto>();

            CreateMap<ControlConditions, SelectControlConditionsDto>();
            CreateMap<ControlConditions, ListControlConditionsDto>();
            CreateMap<CreateControlConditionsDto, ControlConditions>();
            CreateMap<SelectControlConditionsDto, CreateControlConditionsDto>();
            CreateMap<UpdateControlConditionsDto, ControlConditions>();
            CreateMap<SelectControlConditionsDto, UpdateControlConditionsDto>();



            CreateMap<OperationalQualityPlans, SelectOperationalQualityPlansDto>();
            CreateMap<OperationalQualityPlans, ListOperationalQualityPlansDto>();
            CreateMap<UpdateOperationalQualityPlansDto, OperationalQualityPlans>();
            CreateMap<CreateOperationalQualityPlansDto, OperationalQualityPlans>();
            CreateMap<SelectOperationalQualityPlansDto, CreateOperationalQualityPlansDto>();
            CreateMap<SelectOperationalQualityPlansDto, UpdateOperationalQualityPlansDto>();
            CreateMap<OperationalQualityPlans, UpdateOperationalQualityPlansDto>();

            CreateMap<OperationalQualityPlanLines, SelectOperationalQualityPlanLinesDto>();
            CreateMap<OperationalQualityPlanLines, ListOperationalQualityPlanLinesDto>();
            CreateMap<CreateOperationalQualityPlanLinesDto, OperationalQualityPlanLines>();
            CreateMap<SelectOperationalQualityPlanLinesDto, CreateOperationalQualityPlanLinesDto>();
            CreateMap<UpdateOperationalQualityPlanLinesDto, OperationalQualityPlanLines>();
            CreateMap<SelectOperationalQualityPlanLinesDto, UpdateOperationalQualityPlanLinesDto>();
            CreateMap<SelectOperationalQualityPlanLinesDto, OperationalQualityPlanLines>();

            CreateMap<OperationPictures, SelectOperationPicturesDto>();
            CreateMap<OperationPictures, ListOperationPicturesDto>();
            CreateMap<CreateOperationPicturesDto, OperationPictures>();
            CreateMap<SelectOperationPicturesDto, CreateOperationPicturesDto>();
            CreateMap<UpdateOperationPicturesDto, OperationPictures>();
            CreateMap<SelectOperationPicturesDto, UpdateOperationPicturesDto>();
            CreateMap<SelectOperationPicturesDto, OperationPictures>();




            CreateMap<ContractQualityPlans, SelectContractQualityPlansDto>();
            CreateMap<ContractQualityPlans, ListContractQualityPlansDto>();
            CreateMap<UpdateContractQualityPlansDto, ContractQualityPlans>();
            CreateMap<CreateContractQualityPlansDto, ContractQualityPlans>();
            CreateMap<SelectContractQualityPlansDto, CreateContractQualityPlansDto>();
            CreateMap<SelectContractQualityPlansDto, UpdateContractQualityPlansDto>();
            CreateMap<ContractQualityPlans, UpdateContractQualityPlansDto>();

            CreateMap<ContractQualityPlanLines, SelectContractQualityPlanLinesDto>();
            CreateMap<ContractQualityPlanLines, ListContractQualityPlanLinesDto>();
            CreateMap<CreateContractQualityPlanLinesDto, ContractQualityPlanLines>();
            CreateMap<SelectContractQualityPlanLinesDto, CreateContractQualityPlanLinesDto>();
            CreateMap<UpdateContractQualityPlanLinesDto, ContractQualityPlanLines>();
            CreateMap<SelectContractQualityPlanLinesDto, UpdateContractQualityPlanLinesDto>();
            CreateMap<SelectContractQualityPlanLinesDto, ContractQualityPlanLines>();

            CreateMap<ContractOperationPictures, SelectContractOperationPicturesDto>();
            CreateMap<ContractOperationPictures, ListContractOperationPicturesDto>();
            CreateMap<CreateContractOperationPicturesDto, ContractOperationPictures>();
            CreateMap<SelectContractOperationPicturesDto, CreateContractOperationPicturesDto>();
            CreateMap<UpdateContractOperationPicturesDto, ContractOperationPictures>();
            CreateMap<SelectContractOperationPicturesDto, UpdateContractOperationPicturesDto>();
            CreateMap<SelectContractOperationPicturesDto, ContractOperationPictures>();

            CreateMap<ContractQualityPlanOperations, SelectContractQualityPlanOperationsDto>();
            CreateMap<ContractQualityPlanOperations, ListContractQualityPlanOperationsDto>();
            CreateMap<CreateContractQualityPlanOperationsDto, ContractQualityPlanOperations>();
            CreateMap<SelectContractQualityPlanOperationsDto, CreateContractQualityPlanOperationsDto>();
            CreateMap<UpdateContractQualityPlanOperationsDto, ContractQualityPlanOperations>();
            CreateMap<SelectContractQualityPlanOperationsDto, UpdateContractQualityPlanOperationsDto>();
            CreateMap<SelectContractQualityPlanOperationsDto, ContractQualityPlanOperations>();



            CreateMap<PurchaseQualityPlans, SelectPurchaseQualityPlansDto>();
            CreateMap<PurchaseQualityPlans, ListPurchaseQualityPlansDto>();
            CreateMap<UpdatePurchaseQualityPlansDto, PurchaseQualityPlans>();
            CreateMap<CreatePurchaseQualityPlansDto, PurchaseQualityPlans>();
            CreateMap<SelectPurchaseQualityPlansDto, CreatePurchaseQualityPlansDto>();
            CreateMap<SelectPurchaseQualityPlansDto, UpdatePurchaseQualityPlansDto>();
            CreateMap<PurchaseQualityPlans, UpdatePurchaseQualityPlansDto>();

            CreateMap<PurchaseQualityPlanLines, SelectPurchaseQualityPlanLinesDto>();
            CreateMap<PurchaseQualityPlanLines, ListPurchaseQualityPlanLinesDto>();
            CreateMap<CreatePurchaseQualityPlanLinesDto, PurchaseQualityPlanLines>();
            CreateMap<SelectPurchaseQualityPlanLinesDto, CreatePurchaseQualityPlanLinesDto>();
            CreateMap<UpdatePurchaseQualityPlanLinesDto, PurchaseQualityPlanLines>();
            CreateMap<SelectPurchaseQualityPlanLinesDto, UpdatePurchaseQualityPlanLinesDto>();
            CreateMap<SelectPurchaseQualityPlanLinesDto, PurchaseQualityPlanLines>();



            CreateMap<ContractTrackingFiches, SelectContractTrackingFichesDto>();
            CreateMap<ContractTrackingFiches, ListContractTrackingFichesDto>();
            CreateMap<UpdateContractTrackingFichesDto, ContractTrackingFiches>();
            CreateMap<CreateContractTrackingFichesDto, ContractTrackingFiches>();
            CreateMap<SelectContractTrackingFichesDto, CreateContractTrackingFichesDto>();
            CreateMap<SelectContractTrackingFichesDto, UpdateContractTrackingFichesDto>();
            CreateMap<ContractTrackingFiches, UpdateContractTrackingFichesDto>();

            CreateMap<ContractTrackingFicheLines, SelectContractTrackingFicheLinesDto>();
            CreateMap<ContractTrackingFicheLines, ListContractTrackingFicheLinesDto>();
            CreateMap<CreateContractTrackingFicheLinesDto, ContractTrackingFicheLines>();
            CreateMap<SelectContractTrackingFicheLinesDto, CreateContractTrackingFicheLinesDto>();
            CreateMap<UpdateContractTrackingFicheLinesDto, ContractTrackingFicheLines>();
            CreateMap<SelectContractTrackingFicheLinesDto, UpdateContractTrackingFicheLinesDto>();
            CreateMap<SelectContractTrackingFicheLinesDto, ContractTrackingFicheLines>();

            CreateMap<ContractTrackingFicheAmountEntryLines, SelectContractTrackingFicheAmountEntryLinesDto>();
            CreateMap<ContractTrackingFicheAmountEntryLines, ListContractTrackingFicheAmountEntryLinesDto>();
            CreateMap<CreateContractTrackingFicheAmountEntryLinesDto, ContractTrackingFicheAmountEntryLines>();
            CreateMap<SelectContractTrackingFicheAmountEntryLinesDto, CreateContractTrackingFicheAmountEntryLinesDto>();
            CreateMap<UpdateContractTrackingFicheAmountEntryLinesDto, ContractTrackingFicheAmountEntryLines>();
            CreateMap<SelectContractTrackingFicheAmountEntryLinesDto, UpdateContractTrackingFicheAmountEntryLinesDto>();
            CreateMap<SelectContractTrackingFicheAmountEntryLinesDto, ContractTrackingFicheAmountEntryLines>();



            CreateMap<OperationalSPCs, SelectOperationalSPCsDto>();
            CreateMap<OperationalSPCs, ListOperationalSPCsDto>();
            CreateMap<UpdateOperationalSPCsDto, OperationalSPCs>();
            CreateMap<CreateOperationalSPCsDto, OperationalSPCs>();
            CreateMap<SelectOperationalSPCsDto, CreateOperationalSPCsDto>();
            CreateMap<SelectOperationalSPCsDto, UpdateOperationalSPCsDto>();
            CreateMap<OperationalSPCs, UpdateOperationalSPCsDto>();

            CreateMap<OperationalSPCLines, SelectOperationalSPCLinesDto>();
            CreateMap<OperationalSPCLines, ListOperationalSPCLinesDto>();
            CreateMap<CreateOperationalSPCLinesDto, OperationalSPCLines>();
            CreateMap<SelectOperationalSPCLinesDto, CreateOperationalSPCLinesDto>();
            CreateMap<UpdateOperationalSPCLinesDto, OperationalSPCLines>();
            CreateMap<SelectOperationalSPCLinesDto, UpdateOperationalSPCLinesDto>();
            CreateMap<SelectOperationalSPCLinesDto, OperationalSPCLines>();




            CreateMap<UnsuitabilityItemSPCs, SelectUnsuitabilityItemSPCsDto>();
            CreateMap<UnsuitabilityItemSPCs, ListUnsuitabilityItemSPCsDto>();
            CreateMap<UpdateUnsuitabilityItemSPCsDto, UnsuitabilityItemSPCs>();
            CreateMap<CreateUnsuitabilityItemSPCsDto, UnsuitabilityItemSPCs>();
            CreateMap<SelectUnsuitabilityItemSPCsDto, CreateUnsuitabilityItemSPCsDto>();
            CreateMap<SelectUnsuitabilityItemSPCsDto, UpdateUnsuitabilityItemSPCsDto>();
            CreateMap<UnsuitabilityItemSPCs, UpdateUnsuitabilityItemSPCsDto>();

            CreateMap<UnsuitabilityItemSPCLines, SelectUnsuitabilityItemSPCLinesDto>();
            CreateMap<UnsuitabilityItemSPCLines, ListUnsuitabilityItemSPCLinesDto>();
            CreateMap<CreateUnsuitabilityItemSPCLinesDto, UnsuitabilityItemSPCLines>();
            CreateMap<SelectUnsuitabilityItemSPCLinesDto, CreateUnsuitabilityItemSPCLinesDto>();
            CreateMap<UpdateUnsuitabilityItemSPCLinesDto, UnsuitabilityItemSPCLines>();
            CreateMap<SelectUnsuitabilityItemSPCLinesDto, UpdateUnsuitabilityItemSPCLinesDto>();
            CreateMap<SelectUnsuitabilityItemSPCLinesDto, UnsuitabilityItemSPCLines>();



            CreateMap<PFMEAs, SelectPFMEAsDto>();
            CreateMap<PFMEAs, ListPFMEAsDto>();
            CreateMap<CreatePFMEAsDto, PFMEAs>();
            CreateMap<SelectPFMEAsDto, CreatePFMEAsDto>();
            CreateMap<UpdatePFMEAsDto, PFMEAs>();
            CreateMap<SelectPFMEAsDto, UpdatePFMEAsDto>();



            CreateMap<MRPs, SelectMRPsDto>();
            CreateMap<MRPs, ListMRPsDto>();
            CreateMap<UpdateMRPsDto, MRPs>();
            CreateMap<CreateMRPsDto, MRPs>();
            CreateMap<SelectMRPsDto, CreateMRPsDto>();
            CreateMap<SelectMRPsDto, UpdateMRPsDto>();
            CreateMap<MRPs, UpdateMRPsDto>();

            CreateMap<MRPLines, SelectMRPLinesDto>();
            CreateMap<MRPLines, ListMRPLinesDto>();
            CreateMap<CreateMRPLinesDto, MRPLines>();
            CreateMap<SelectMRPLinesDto, CreateMRPLinesDto>();
            CreateMap<UpdateMRPLinesDto, MRPLines>();
            CreateMap<SelectMRPLinesDto, UpdateMRPLinesDto>();
            CreateMap<SelectMRPLinesDto, MRPLines>();



            CreateMap<MRPIIs, SelectMRPIIsDto>();
            CreateMap<MRPIIs, ListMRPIIsDto>();
            CreateMap<UpdateMRPIIsDto, MRPIIs>();
            CreateMap<CreateMRPIIsDto, MRPIIs>();
            CreateMap<SelectMRPIIsDto, CreateMRPIIsDto>();
            CreateMap<SelectMRPIIsDto, UpdateMRPIIsDto>();
            CreateMap<MRPIIs, UpdateMRPIIsDto>();

            CreateMap<MRPIILines, SelectMRPIILinesDto>();
            CreateMap<MRPIILines, ListMRPIILinesDto>();
            CreateMap<CreateMRPIILinesDto, MRPIILines>();
            CreateMap<SelectMRPIILinesDto, CreateMRPIILinesDto>();
            CreateMap<UpdateMRPIILinesDto, MRPIILines>();
            CreateMap<SelectMRPIILinesDto, UpdateMRPIILinesDto>();
            CreateMap<SelectMRPIILinesDto, MRPIILines>();




            CreateMap<FirstProductApprovals, SelectFirstProductApprovalsDto>();
            CreateMap<FirstProductApprovals, ListFirstProductApprovalsDto>();
            CreateMap<UpdateFirstProductApprovalsDto, FirstProductApprovals>();
            CreateMap<CreateFirstProductApprovalsDto, FirstProductApprovals>();
            CreateMap<SelectFirstProductApprovalsDto, CreateFirstProductApprovalsDto>();
            CreateMap<SelectFirstProductApprovalsDto, UpdateFirstProductApprovalsDto>();
            CreateMap<FirstProductApprovals, UpdateFirstProductApprovalsDto>();

            CreateMap<FirstProductApprovalLines, SelectFirstProductApprovalLinesDto>();
            CreateMap<FirstProductApprovalLines, ListFirstProductApprovalLinesDto>();
            CreateMap<CreateFirstProductApprovalLinesDto, FirstProductApprovalLines>();
            CreateMap<SelectFirstProductApprovalLinesDto, CreateFirstProductApprovalLinesDto>();
            CreateMap<UpdateFirstProductApprovalLinesDto, FirstProductApprovalLines>();
            CreateMap<SelectFirstProductApprovalLinesDto, UpdateFirstProductApprovalLinesDto>();
            CreateMap<SelectFirstProductApprovalLinesDto, FirstProductApprovalLines>();




            CreateMap<PackageFiches, SelectPackageFichesDto>();
            CreateMap<PackageFiches, ListPackageFichesDto>();
            CreateMap<UpdatePackageFichesDto, PackageFiches>();
            CreateMap<CreatePackageFichesDto, PackageFiches>();
            CreateMap<SelectPackageFichesDto, CreatePackageFichesDto>();
            CreateMap<SelectPackageFichesDto, UpdatePackageFichesDto>();
            CreateMap<PackageFiches, UpdatePackageFichesDto>();

            CreateMap<PackageFicheLines, SelectPackageFicheLinesDto>();
            CreateMap<PackageFicheLines, ListPackageFicheLinesDto>();
            CreateMap<CreatePackageFicheLinesDto, PackageFicheLines>();
            CreateMap<SelectPackageFicheLinesDto, CreatePackageFicheLinesDto>();
            CreateMap<UpdatePackageFicheLinesDto, PackageFicheLines>();
            CreateMap<SelectPackageFicheLinesDto, UpdatePackageFicheLinesDto>();
            CreateMap<SelectPackageFicheLinesDto, PackageFicheLines>();




            CreateMap<PalletRecords, SelectPalletRecordsDto>();
            CreateMap<PalletRecords, ListPalletRecordsDto>();
            CreateMap<UpdatePalletRecordsDto, PalletRecords>();
            CreateMap<CreatePalletRecordsDto, PalletRecords>();
            CreateMap<SelectPalletRecordsDto, CreatePalletRecordsDto>();
            CreateMap<SelectPalletRecordsDto, UpdatePalletRecordsDto>();
            CreateMap<PalletRecords, UpdatePalletRecordsDto>();

            CreateMap<PalletRecordLines, SelectPalletRecordLinesDto>();
            CreateMap<PalletRecordLines, ListPalletRecordLinesDto>();
            CreateMap<CreatePalletRecordLinesDto, PalletRecordLines>();
            CreateMap<SelectPalletRecordLinesDto, CreatePalletRecordLinesDto>();
            CreateMap<UpdatePalletRecordLinesDto, PalletRecordLines>();
            CreateMap<SelectPalletRecordLinesDto, UpdatePalletRecordLinesDto>();
            CreateMap<SelectPalletRecordLinesDto, PalletRecordLines>();




            CreateMap<MaintenanceMRPs, SelectMaintenanceMRPsDto>();
            CreateMap<MaintenanceMRPs, ListMaintenanceMRPsDto>();
            CreateMap<UpdateMaintenanceMRPsDto, MaintenanceMRPs>();
            CreateMap<CreateMaintenanceMRPsDto, MaintenanceMRPs>();
            CreateMap<SelectMaintenanceMRPsDto, CreateMaintenanceMRPsDto>();
            CreateMap<SelectMaintenanceMRPsDto, UpdateMaintenanceMRPsDto>();
            CreateMap<MaintenanceMRPs, UpdateMaintenanceMRPsDto>();

            CreateMap<MaintenanceMRPLines, SelectMaintenanceMRPLinesDto>();
            CreateMap<MaintenanceMRPLines, ListMaintenanceMRPLinesDto>();
            CreateMap<CreateMaintenanceMRPLinesDto, MaintenanceMRPLines>();
            CreateMap<SelectMaintenanceMRPLinesDto, CreateMaintenanceMRPLinesDto>();
            CreateMap<UpdateMaintenanceMRPLinesDto, MaintenanceMRPLines>();
            CreateMap<SelectMaintenanceMRPLinesDto, UpdateMaintenanceMRPLinesDto>();
            CreateMap<SelectMaintenanceMRPLinesDto, MaintenanceMRPLines>();

            CreateMap<StartingSalaries, SelectStartingSalariesDto>();
            CreateMap<StartingSalaries, ListStartingSalariesDto>();
            CreateMap<UpdateStartingSalariesDto, StartingSalaries>();
            CreateMap<CreateStartingSalariesDto, StartingSalaries>();
            CreateMap<SelectStartingSalariesDto, CreateStartingSalariesDto>();
            CreateMap<SelectStartingSalariesDto, UpdateStartingSalariesDto>();
            CreateMap<StartingSalaries, UpdateStartingSalariesDto>();

            CreateMap<StartingSalaryLines, SelectStartingSalaryLinesDto>();
            CreateMap<StartingSalaryLines, ListStartingSalaryLinesDto>();
            CreateMap<CreateStartingSalaryLinesDto, StartingSalaryLines>();
            CreateMap<SelectStartingSalaryLinesDto, CreateStartingSalaryLinesDto>();
            CreateMap<UpdateStartingSalaryLinesDto, StartingSalaryLines>();
            CreateMap<SelectStartingSalaryLinesDto, UpdateStartingSalaryLinesDto>();
            CreateMap<SelectStartingSalaryLinesDto, StartingSalaryLines>();





            CreateMap<PackingLists, SelectPackingListsDto>();
            CreateMap<PackingLists, ListPackingListsDto>();
            CreateMap<UpdatePackingListsDto, PackingLists>();
            CreateMap<CreatePackingListsDto, PackingLists>();
            CreateMap<SelectPackingListsDto, CreatePackingListsDto>();
            CreateMap<SelectPackingListsDto, UpdatePackingListsDto>();
            CreateMap<PackingLists, UpdatePackingListsDto>();

            CreateMap<PackingListPalletCubageLines, SelectPackingListPalletCubageLinesDto>();
            CreateMap<PackingListPalletCubageLines, ListPackingListPalletCubageLinesDto>();
            CreateMap<CreatePackingListPalletCubageLinesDto, PackingListPalletCubageLines>();
            CreateMap<SelectPackingListPalletCubageLinesDto, CreatePackingListPalletCubageLinesDto>();
            CreateMap<UpdatePackingListPalletCubageLinesDto, PackingListPalletCubageLines>();
            CreateMap<SelectPackingListPalletCubageLinesDto, UpdatePackingListPalletCubageLinesDto>();
            CreateMap<SelectPackingListPalletCubageLinesDto, PackingListPalletCubageLines>();

            CreateMap<PackingListPalletLines, SelectPackingListPalletLinesDto>();
            CreateMap<PackingListPalletLines, ListPackingListPalletLinesDto>();
            CreateMap<CreatePackingListPalletLinesDto, PackingListPalletLines>();
            CreateMap<SelectPackingListPalletLinesDto, CreatePackingListPalletLinesDto>();
            CreateMap<UpdatePackingListPalletLinesDto, PackingListPalletLines>();
            CreateMap<SelectPackingListPalletLinesDto, UpdatePackingListPalletLinesDto>();
            CreateMap<SelectPackingListPalletLinesDto, PackingListPalletLines>();

            CreateMap<PackingListPalletPackageLines, SelectPackingListPalletPackageLinesDto>();
            CreateMap<PackingListPalletPackageLines, ListPackingListPalletPackageLinesDto>();
            CreateMap<CreatePackingListPalletPackageLinesDto, PackingListPalletPackageLines>();
            CreateMap<SelectPackingListPalletPackageLinesDto, CreatePackingListPalletPackageLinesDto>();
            CreateMap<UpdatePackingListPalletPackageLinesDto, PackingListPalletPackageLines>();
            CreateMap<SelectPackingListPalletPackageLinesDto, UpdatePackingListPalletPackageLinesDto>();
            CreateMap<SelectPackingListPalletPackageLinesDto, PackingListPalletPackageLines>();




            CreateMap<EmployeeScorings, SelectEmployeeScoringsDto>();
            CreateMap<EmployeeScorings, ListEmployeeScoringsDto>();
            CreateMap<UpdateEmployeeScoringsDto, EmployeeScorings>();
            CreateMap<CreateEmployeeScoringsDto, EmployeeScorings>();
            CreateMap<SelectEmployeeScoringsDto, CreateEmployeeScoringsDto>();
            CreateMap<SelectEmployeeScoringsDto, UpdateEmployeeScoringsDto>();
            CreateMap<EmployeeScorings, UpdateEmployeeScoringsDto>();

            CreateMap<EmployeeScoringLines, SelectEmployeeScoringLinesDto>();
            CreateMap<EmployeeScoringLines, ListEmployeeScoringLinesDto>();
            CreateMap<CreateEmployeeScoringLinesDto, EmployeeScoringLines>();
            CreateMap<SelectEmployeeScoringLinesDto, CreateEmployeeScoringLinesDto>();
            CreateMap<UpdateEmployeeScoringLinesDto, EmployeeScoringLines>();
            CreateMap<SelectEmployeeScoringLinesDto, UpdateEmployeeScoringLinesDto>();
            CreateMap<SelectEmployeeScoringLinesDto, EmployeeScoringLines>();

            CreateMap<EmployeeOperations, SelectEmployeeOperationsDto>();
            CreateMap<EmployeeOperations, ListEmployeeOperationsDto>();
            CreateMap<CreateEmployeeOperationsDto, EmployeeOperations>();
            CreateMap<SelectEmployeeOperationsDto, CreateEmployeeOperationsDto>();
            CreateMap<UpdateEmployeeOperationsDto, EmployeeOperations>();
            CreateMap<SelectEmployeeOperationsDto, UpdateEmployeeOperationsDto>();
            CreateMap<SelectEmployeeOperationsDto, EmployeeOperations>();




            CreateMap<OrderAcceptanceRecords, SelectOrderAcceptanceRecordsDto>();
            CreateMap<OrderAcceptanceRecords, ListOrderAcceptanceRecordsDto>();
            CreateMap<UpdateOrderAcceptanceRecordsDto, OrderAcceptanceRecords>();
            CreateMap<CreateOrderAcceptanceRecordsDto, OrderAcceptanceRecords>();
            CreateMap<SelectOrderAcceptanceRecordsDto, CreateOrderAcceptanceRecordsDto>();
            CreateMap<SelectOrderAcceptanceRecordsDto, UpdateOrderAcceptanceRecordsDto>();
            CreateMap<OrderAcceptanceRecords, UpdateOrderAcceptanceRecordsDto>();

            CreateMap<OrderAcceptanceRecordLines, SelectOrderAcceptanceRecordLinesDto>();
            CreateMap<OrderAcceptanceRecordLines, ListOrderAcceptanceRecordLinesDto>();
            CreateMap<CreateOrderAcceptanceRecordLinesDto, OrderAcceptanceRecordLines>();
            CreateMap<SelectOrderAcceptanceRecordLinesDto, CreateOrderAcceptanceRecordLinesDto>();
            CreateMap<UpdateOrderAcceptanceRecordLinesDto, OrderAcceptanceRecordLines>();
            CreateMap<SelectOrderAcceptanceRecordLinesDto, UpdateOrderAcceptanceRecordLinesDto>();
            CreateMap<SelectOrderAcceptanceRecordLinesDto, OrderAcceptanceRecordLines>();




            CreateMap<ShipmentPlannings, SelectShipmentPlanningsDto>();
            CreateMap<ShipmentPlannings, ListShipmentPlanningsDto>();
            CreateMap<UpdateShipmentPlanningsDto, ShipmentPlannings>();
            CreateMap<CreateShipmentPlanningsDto, ShipmentPlannings>();
            CreateMap<SelectShipmentPlanningsDto, CreateShipmentPlanningsDto>();
            CreateMap<SelectShipmentPlanningsDto, UpdateShipmentPlanningsDto>();
            CreateMap<ShipmentPlannings, UpdateShipmentPlanningsDto>();

            CreateMap<ShipmentPlanningLines, SelectShipmentPlanningLinesDto>();
            CreateMap<ShipmentPlanningLines, ListShipmentPlanningLinesDto>();
            CreateMap<CreateShipmentPlanningLinesDto, ShipmentPlanningLines>();
            CreateMap<SelectShipmentPlanningLinesDto, CreateShipmentPlanningLinesDto>();
            CreateMap<UpdateShipmentPlanningLinesDto, ShipmentPlanningLines>();
            CreateMap<SelectShipmentPlanningLinesDto, UpdateShipmentPlanningLinesDto>();
            CreateMap<SelectShipmentPlanningLinesDto, ShipmentPlanningLines>();

            CreateMap<StockAddresses, SelectStockAddressesDto>();
            CreateMap<StockAddresses, ListStockAddressesDto>();
            CreateMap<UpdateStockAddressesDto, StockAddresses>();
            CreateMap<CreateStockAddressesDto, StockAddresses>();
            CreateMap<SelectStockAddressesDto, CreateStockAddressesDto>();
            CreateMap<SelectStockAddressesDto, UpdateStockAddressesDto>();
            CreateMap<StockAddresses, UpdateStockAddressesDto>();

            CreateMap<StockAddressLines, SelectStockAddressLinesDto>();
            CreateMap<StockAddressLines, ListStockAddressLinesDto>();
            CreateMap<CreateStockAddressLinesDto, StockAddressLines>();
            CreateMap<SelectStockAddressLinesDto, CreateStockAddressLinesDto>();
            CreateMap<UpdateStockAddressLinesDto, StockAddressLines>();
            CreateMap<SelectStockAddressLinesDto, UpdateStockAddressLinesDto>();
            CreateMap<SelectStockAddressLinesDto, StockAddressLines>();

            CreateMap<ProductProperties, SelectProductPropertiesDto>();
            CreateMap<ProductProperties, ListProductPropertiesDto>();
            CreateMap<CreateProductPropertiesDto, ProductProperties>();
            CreateMap<SelectProductPropertiesDto, CreateProductPropertiesDto>();
            CreateMap<UpdateProductPropertiesDto, ProductProperties>();
            CreateMap<SelectProductPropertiesDto, UpdateProductPropertiesDto>();
            CreateMap<SelectProductPropertiesDto, ProductProperties>();




            CreateMap<PurchaseOrdersAwaitingApprovals, SelectPurchaseOrdersAwaitingApprovalsDto>();
            CreateMap<PurchaseOrdersAwaitingApprovals, ListPurchaseOrdersAwaitingApprovalsDto>();
            CreateMap<UpdatePurchaseOrdersAwaitingApprovalsDto, PurchaseOrdersAwaitingApprovals>();
            CreateMap<CreatePurchaseOrdersAwaitingApprovalsDto, PurchaseOrdersAwaitingApprovals>();
            CreateMap<SelectPurchaseOrdersAwaitingApprovalsDto, CreatePurchaseOrdersAwaitingApprovalsDto>();
            CreateMap<SelectPurchaseOrdersAwaitingApprovalsDto, UpdatePurchaseOrdersAwaitingApprovalsDto>();
            CreateMap<PurchaseOrdersAwaitingApprovals, UpdatePurchaseOrdersAwaitingApprovalsDto>();

            CreateMap<PurchaseOrdersAwaitingApprovalLines, SelectPurchaseOrdersAwaitingApprovalLinesDto>();
            CreateMap<PurchaseOrdersAwaitingApprovalLines, ListPurchaseOrdersAwaitingApprovalLinesDto>();
            CreateMap<CreatePurchaseOrdersAwaitingApprovalLinesDto, PurchaseOrdersAwaitingApprovalLines>();
            CreateMap<SelectPurchaseOrdersAwaitingApprovalLinesDto, CreatePurchaseOrdersAwaitingApprovalLinesDto>();
            CreateMap<UpdatePurchaseOrdersAwaitingApprovalLinesDto, PurchaseOrdersAwaitingApprovalLines>();
            CreateMap<SelectPurchaseOrdersAwaitingApprovalLinesDto, UpdatePurchaseOrdersAwaitingApprovalLinesDto>();
            CreateMap<SelectPurchaseOrdersAwaitingApprovalLinesDto, PurchaseOrdersAwaitingApprovalLines>();


            CreateMap<CPRs, SelectCPRsDto>();
            CreateMap<CPRs, ListCPRsDto>();
            CreateMap<UpdateCPRsDto, CPRs>();
            CreateMap<CreateCPRsDto, CPRs>();
            CreateMap<SelectCPRsDto, CreateCPRsDto>();
            CreateMap<SelectCPRsDto, UpdateCPRsDto>();
            CreateMap<CPRs, UpdateCPRsDto>();

            CreateMap<CPRManufacturingCostLines, SelectCPRManufacturingCostLinesDto>();
            CreateMap<CPRManufacturingCostLines, ListCPRManufacturingCostLinesDto>();
            CreateMap<CreateCPRManufacturingCostLinesDto, CPRManufacturingCostLines>();
            CreateMap<SelectCPRManufacturingCostLinesDto, CreateCPRManufacturingCostLinesDto>();
            CreateMap<UpdateCPRManufacturingCostLinesDto, CPRManufacturingCostLines>();
            CreateMap<SelectCPRManufacturingCostLinesDto, UpdateCPRManufacturingCostLinesDto>();
            CreateMap<SelectCPRManufacturingCostLinesDto, CPRManufacturingCostLines>();


            CreateMap<CPRMaterialCostLines, SelectCPRMaterialCostLinesDto>();
            CreateMap<CPRMaterialCostLines, ListCPRMaterialCostLinesDto>();
            CreateMap<CreateCPRMaterialCostLinesDto, CPRMaterialCostLines>();
            CreateMap<SelectCPRMaterialCostLinesDto, CreateCPRMaterialCostLinesDto>();
            CreateMap<UpdateCPRMaterialCostLinesDto, CPRMaterialCostLines>();
            CreateMap<SelectCPRMaterialCostLinesDto, UpdateCPRMaterialCostLinesDto>();
            CreateMap<SelectCPRMaterialCostLinesDto, CPRMaterialCostLines>();

            CreateMap<CPRSetupCostLines, SelectCPRSetupCostLinesDto>();
            CreateMap<CPRSetupCostLines, ListCPRSetupCostLinesDto>();
            CreateMap<CreateCPRSetupCostLinesDto, CPRSetupCostLines>();
            CreateMap<SelectCPRSetupCostLinesDto, CreateCPRSetupCostLinesDto>();
            CreateMap<UpdateCPRSetupCostLinesDto, CPRSetupCostLines>();
            CreateMap<SelectCPRSetupCostLinesDto, UpdateCPRSetupCostLinesDto>();
            CreateMap<SelectCPRSetupCostLinesDto, CPRSetupCostLines>();





            //TEST -------------------------------------------------------

            CreateMap<Continents, SelectContinentsDto>();
            CreateMap<Continents, ListContinentsDto>();
            CreateMap<UpdateContinentsDto, Continents>();
            CreateMap<CreateContinentsDto, Continents>();
            CreateMap<SelectContinentsDto, CreateContinentsDto>();
            CreateMap<SelectContinentsDto, UpdateContinentsDto>();


            CreateMap<ContinentLines, SelectContinentLinesDto>();
            CreateMap<ContinentLines, ListContinentLinesDto>();
            CreateMap<CreateContinentLinesDto, ContinentLines>();
            CreateMap<SelectContinentLinesDto, CreateContinentLinesDto>();
            CreateMap<UpdateContinentLinesDto, ContinentLines>();
            CreateMap<SelectContinentLinesDto, UpdateContinentLinesDto>();
            CreateMap<SelectContinentLinesDto, ContinentLines>();


            CreateMap<Sectors, SelectSectorsDto>();
            CreateMap<Sectors, ListSectorsDto>();
            CreateMap<UpdateSectorsDto, Sectors>();
            CreateMap<CreateSectorsDto, Sectors>();
            CreateMap<SelectSectorsDto, CreateSectorsDto>();
            CreateMap<SelectSectorsDto, UpdateSectorsDto>();


            CreateMap<SectorLines, SelectSectorLinesDto>();
            CreateMap<SectorLines, ListSectorLinesDto>();
            CreateMap<CreateSectorLinesDto, SectorLines>();
            CreateMap<SelectSectorLinesDto, CreateSectorLinesDto>();
            CreateMap<UpdateSectorLinesDto, SectorLines>();
            CreateMap<SelectSectorLinesDto, UpdateSectorLinesDto>();
            CreateMap<SelectSectorLinesDto, SectorLines>();


            CreateMap<Cities, SelectCitiesDto>();
            CreateMap<Cities, ListCitiesDto>();
            CreateMap<UpdateCitiesDto, Cities>();
            CreateMap<CreateCitiesDto, Cities>();
            CreateMap<SelectCitiesDto, CreateCitiesDto>();
            CreateMap<SelectCitiesDto, UpdateCitiesDto>();


            CreateMap<CityLines, SelectCityLinesDto>();
            CreateMap<CityLines, ListCityLinesDto>();
            CreateMap<CreateCityLinesDto, CityLines>();
            CreateMap<SelectCityLinesDto, CreateCityLinesDto>();
            CreateMap<UpdateCityLinesDto, CityLines>();
            CreateMap<SelectCityLinesDto, UpdateCityLinesDto>();
            CreateMap<SelectCityLinesDto, CityLines>();



            //TEST -------------------------------------------------------
        }
    }
}
