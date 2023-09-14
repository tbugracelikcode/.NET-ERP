using AutoMapper;
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
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationInventory.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.MaintenanceManagement.MaintenanceInstructionLine.Dtos;
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
using TsiErp.Entities.Entities.PlanningManagement.Calendar;
using TsiErp.Entities.Entities.PlanningManagement.Calendar.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay;
using TsiErp.Entities.Entities.PlanningManagement.CalendarDay.Dtos;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine;
using TsiErp.Entities.Entities.PlanningManagement.CalendarLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine;
using TsiErp.Entities.Entities.ProductionManagement.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ContractProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder;
using TsiErp.Entities.Entities.ProductionManagement.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTrackingHaltLine.Dtos;
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
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord;
using TsiErp.Entities.Entities.QualityControl.EquipmentRecord.Dtos;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.FinalControlUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.ContractUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.QualityControl.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPC.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItemSPCLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalSPCLine;
using TsiErp.Entities.Entities.QualityControl.OperationalSPCLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC.Dtos;
using TsiErp.Entities.Entities.SalesManagement.Forecast;
using TsiErp.Entities.Entities.SalesManagement.Forecast.Dtos;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine;
using TsiErp.Entities.Entities.SalesManagement.ForecastLine.Dtos;
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
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress;
using TsiErp.Entities.Entities.ShippingManagement.ShippingAdress.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber;
using TsiErp.Entities.Entities.StockManagement.ProductReferanceNumber.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFiche;
using TsiErp.Entities.Entities.StockManagement.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine;
using TsiErp.Entities.Entities.StockManagement.StockFicheLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.StockManagement.UnitSet;
using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;
using TsiErp.Entities.Entities.StockManagement.WareHouse;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlType;
using TsiErp.Entities.Entities.QualityControl.ControlType.Dtos;
using TsiErp.Entities.Entities.QualityControl.ControlCondition;
using TsiErp.Entities.Entities.QualityControl.ControlCondition.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.OperationPicture;
using TsiErp.Entities.Entities.QualityControl.OperationPicture.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractOperationPicture;
using TsiErp.Entities.Entities.QualityControl.ContractOperationPicture.Dtos;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation;
using TsiErp.Entities.Entities.QualityControl.ContractQualityPlanOperation.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan.Dtos;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlan;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine;
using TsiErp.Entities.Entities.QualityControl.PurchaseQualityPlanLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.PFMEA;
using TsiErp.Entities.Entities.QualityControl.PFMEA.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFiche.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine;
using TsiErp.Entities.Entities.ProductionManagement.ContractTrackingFicheLine.Dtos;

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

            CreateMap<ProductionTrackingHaltLines, SelectProductionTrackingHaltLinesDto>();
            CreateMap<ProductionTrackingHaltLines, ListProductionTrackingHaltLinesDto>();
            CreateMap<CreateProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>();
            CreateMap<SelectProductionTrackingHaltLinesDto, CreateProductionTrackingHaltLinesDto>();
            CreateMap<UpdateProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>();
            CreateMap<SelectProductionTrackingHaltLinesDto, UpdateProductionTrackingHaltLinesDto>();
            CreateMap<SelectProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>();

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
        }
    }
}
