using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.UnitSet.Dtos;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.StationGroup.Dtos;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.CalibrationRecord.Dtos;
using TsiErp.Entities.Entities.CalibrationRecord;
using TsiErp.Entities.Entities.CalibrationVerification.Dtos;
using TsiErp.Entities.Entities.CalibrationVerification;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.ContractUnsuitabilityItem;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.Department;
using TsiErp.Entities.Entities.Employee.Dtos;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;
using TsiErp.Entities.Entities.EquipmentRecord;
using TsiErp.Entities.Entities.Currency;
using TsiErp.Entities.Entities.Currency.Dtos;
using TsiErp.Entities.Entities.PaymentPlan;
using TsiErp.Entities.Entities.PaymentPlan.Dtos;
using TsiErp.Entities.Entities.WareHouse;
using TsiErp.Entities.Entities.WareHouse.Dtos;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem;
using TsiErp.Entities.Entities.OperationUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.CurrentAccountCard;
using TsiErp.Entities.Entities.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.CustomerComplaintItem;
using TsiErp.Entities.Entities.CustomerComplaintItem.Dtos;
using TsiErp.Entities.Entities.ExchangeRate;
using TsiErp.Entities.Entities.ExchangeRate.Dtos;
using TsiErp.Entities.Entities.Product.Dtos;
using TsiErp.Entities.Entities.Product;
using TsiErp.Entities.Entities.ProductionOrderChangeItem;
using TsiErp.Entities.Entities.ProductionOrderChangeItem.Dtos;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem;
using TsiErp.Entities.Entities.PurchasingUnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.SalesProposition;
using TsiErp.Entities.Entities.SalesProposition.Dtos;
using TsiErp.Entities.Entities.ProductGroup;
using TsiErp.Entities.Entities.ProductGroup.Dtos;
using TsiErp.Entities.Entities.ShippingAdress;
using TsiErp.Entities.Entities.ShippingAdress.Dtos;
using TsiErp.Entities.Entities.SalesPropositionLine.Dtos;
using TsiErp.Entities.Entities.SalesPropositionLine;
using TsiErp.Entities.Entities.Route.Dtos;
using TsiErp.Entities.Entities.Route;
using TsiErp.Entities.Entities.RouteLine;
using TsiErp.Entities.Entities.RouteLine.Dtos;
using TsiErp.Entities.Entities.Shift;
using TsiErp.Entities.Entities.Shift.Dtos;
using TsiErp.Entities.Entities.ShiftLine;
using TsiErp.Entities.Entities.ShiftLine.Dtos;
using TsiErp.Entities.Entities.Calendar.Dtos;
using TsiErp.Entities.Entities.CalendarLine.Dtos;
using TsiErp.Entities.Entities.CalendarLine;
using TsiErp.Entities.Entities.Calendar;
using TsiErp.Entities.Entities.TemplateOperation;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.TemplateOperationLine;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;
using TsiErp.Entities.Entities.ProductsOperation;
using TsiErp.Entities.Entities.ProductsOperation.Dtos;
using TsiErp.Entities.Entities.ProductsOperationLine;
using TsiErp.Entities.Entities.ProductsOperationLine.Dtos;
using TsiErp.Entities.Entities.SalesOrder;
using TsiErp.Entities.Entities.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesOrderLine;
using TsiErp.Entities.Entities.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.SalesPrice;
using TsiErp.Entities.Entities.SalesPrice.Dtos;
using TsiErp.Entities.Entities.SalesPriceLine;
using TsiErp.Entities.Entities.SalesPriceLine.Dtos;
using TsiErp.Entities.Entities.BillsofMaterial;
using TsiErp.Entities.Entities.BillsofMaterial.Dtos;
using TsiErp.Entities.Entities.BillsofMaterialLine;
using TsiErp.Entities.Entities.BillsofMaterialLine.Dtos;
using TsiErp.Entities.Entities.CalendarDay.Dtos;
using TsiErp.Entities.Entities.CalendarDay;
using TsiErp.Entities.Entities.ProductionOrder;
using TsiErp.Entities.Entities.ProductionOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseOrder.Dtos;
using TsiErp.Entities.Entities.PurchaseOrderLine.Dtos;
using TsiErp.Entities.Entities.Forecast.Dtos;
using TsiErp.Entities.Entities.ForecastLine.Dtos;
using TsiErp.Entities.Entities.PurchaseRequest.Dtos;
using TsiErp.Entities.Entities.PurchaseRequestLine.Dtos;
using TsiErp.Entities.Entities.PurchaseOrder;
using TsiErp.Entities.Entities.PurchaseOrderLine;
using TsiErp.Entities.Entities.PurchaseRequest;
using TsiErp.Entities.Entities.PurchaseRequestLine;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport;
using TsiErp.Entities.Entities.PurchaseUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.OperationUnsuitabilityReport;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Entities.Entities.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionTracking;
using TsiErp.Entities.Entities.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine;
using TsiErp.Entities.Entities.ProductionTrackingHaltLine.Dtos;
using TsiErp.Entities.Entities.ContractProductionTracking.Dtos;
using TsiErp.Entities.Entities.ContractProductionTracking;
using TsiErp.Entities.Entities.Forecast;
using TsiErp.Entities.Entities.ForecastLine;
using TsiErp.Entities.Entities.PurchasePriceLine;
using TsiErp.Entities.Entities.PurchasePrice;
using TsiErp.Entities.Entities.PurchasePrice.Dtos;
using TsiErp.Entities.Entities.PurchasePriceLine.Dtos;
using TsiErp.Entities.Entities.UserGroup;
using TsiErp.Entities.Entities.UserGroup.Dtos;
using TsiErp.Entities.Entities.User;
using TsiErp.Entities.Entities.User.Dtos;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport;
using TsiErp.Entities.Entities.FinalControlUnsuitabilityReport.Dtos;
using TsiErp.Entities.Entities.StationInventory.Dtos;
using TsiErp.Entities.Entities.StationInventory;
using TsiErp.Entities.Entities.MaintenancePeriod;
using TsiErp.Entities.Entities.MaintenancePeriod.Dtos;
using TsiErp.Entities.Entities.MaintenanceInstruction;
using TsiErp.Entities.Entities.MaintenanceInstruction.Dtos;
using TsiErp.Entities.Entities.MaintenanceInstructionLine;
using TsiErp.Entities.Entities.MaintenanceInstructionLine.Dtos;
using TsiErp.Entities.Entities.PlannedMaintenance;
using TsiErp.Entities.Entities.PlannedMaintenance.Dtos;
using TsiErp.Entities.Entities.PlannedMaintenanceLine;
using TsiErp.Entities.Entities.PlannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.UnplannedMaintenance;
using TsiErp.Entities.Entities.UnplannedMaintenance.Dtos;
using TsiErp.Entities.Entities.UnplannedMaintenanceLine;
using TsiErp.Entities.Entities.UnplannedMaintenanceLine.Dtos;
using TsiErp.Entities.Entities.GrandTotalStockMovement;
using TsiErp.Entities.Entities.GrandTotalStockMovement.Dtos;
using TsiErp.Entities.Entities.ByDateStockMovement;
using TsiErp.Entities.Entities.ByDateStockMovement.Dtos;
using TsiErp.Entities.Entities.TechnicalDrawing;
using TsiErp.Entities.Entities.TechnicalDrawing.Dtos;
using TsiErp.Entities.Entities.ProductReferanceNumber;
using TsiErp.Entities.Entities.ProductReferanceNumber.Dtos;
using TsiErp.Entities.Entities.StockFiche.Dtos;
using TsiErp.Entities.Entities.StockFicheLine.Dtos;
using AutoMapper.EquivalencyExpression;
using TsiErp.Entities.Entities.StockFiche;
using TsiErp.Entities.Entities.StockFicheLine;

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

            CreateMap<ContractUnsuitabilityItems, SelectContractUnsuitabilityItemsDto>();
            CreateMap<ContractUnsuitabilityItems, ListContractUnsuitabilityItemsDto>();
            CreateMap<CreateContractUnsuitabilityItemsDto, ContractUnsuitabilityItems>();
            CreateMap<SelectContractUnsuitabilityItemsDto, CreateContractUnsuitabilityItemsDto>();
            CreateMap<UpdateContractUnsuitabilityItemsDto, ContractUnsuitabilityItems>();
            CreateMap<SelectContractUnsuitabilityItemsDto, UpdateContractUnsuitabilityItemsDto>();

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

            CreateMap<CustomerComplaintItems, SelectCustomerComplaintItemsDto>();
            CreateMap<CustomerComplaintItems, ListCustomerComplaintItemsDto>();
            CreateMap<CreateCustomerComplaintItemsDto, CustomerComplaintItems>();
            CreateMap<SelectCustomerComplaintItemsDto, CreateCustomerComplaintItemsDto>();
            CreateMap<UpdateCustomerComplaintItemsDto, CustomerComplaintItems>();
            CreateMap<SelectCustomerComplaintItemsDto, UpdateCustomerComplaintItemsDto>();

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

            CreateMap<FinalControlUnsuitabilityItems, SelectFinalControlUnsuitabilityItemsDto>();
            CreateMap<FinalControlUnsuitabilityItems, ListFinalControlUnsuitabilityItemsDto>();
            CreateMap<CreateFinalControlUnsuitabilityItemsDto, FinalControlUnsuitabilityItems>();
            CreateMap<SelectFinalControlUnsuitabilityItemsDto, CreateFinalControlUnsuitabilityItemsDto>();
            CreateMap<UpdateFinalControlUnsuitabilityItemsDto, FinalControlUnsuitabilityItems>();
            CreateMap<SelectFinalControlUnsuitabilityItemsDto, UpdateFinalControlUnsuitabilityItemsDto>();

            CreateMap<OperationUnsuitabilityItems, SelectOperationUnsuitabilityItemsDto>();
            CreateMap<OperationUnsuitabilityItems, ListOperationUnsuitabilityItemsDto>();
            CreateMap<CreateOperationUnsuitabilityItemsDto, OperationUnsuitabilityItems>();
            CreateMap<SelectOperationUnsuitabilityItemsDto, CreateOperationUnsuitabilityItemsDto>();
            CreateMap<UpdateOperationUnsuitabilityItemsDto, OperationUnsuitabilityItems>();
            CreateMap<SelectOperationUnsuitabilityItemsDto, UpdateOperationUnsuitabilityItemsDto>();

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

            CreateMap<ProductionOrderChangeItems, SelectProductionOrderChangeItemsDto>();
            CreateMap<ProductionOrderChangeItems, ListProductionOrderChangeItemsDto>();
            CreateMap<CreateProductionOrderChangeItemsDto, ProductionOrderChangeItems>();
            CreateMap<SelectProductionOrderChangeItemsDto, CreateProductionOrderChangeItemsDto>();
            CreateMap<UpdateProductionOrderChangeItemsDto, ProductionOrderChangeItems>();
            CreateMap<SelectProductionOrderChangeItemsDto, UpdateProductionOrderChangeItemsDto>();

            CreateMap<PurchasingUnsuitabilityItems, SelectPurchasingUnsuitabilityItemsDto>();
            CreateMap<PurchasingUnsuitabilityItems, ListPurchasingUnsuitabilityItemsDto>();
            CreateMap<CreatePurchasingUnsuitabilityItemsDto, PurchasingUnsuitabilityItems>();
            CreateMap<SelectPurchasingUnsuitabilityItemsDto, CreatePurchasingUnsuitabilityItemsDto>();
            CreateMap<UpdatePurchasingUnsuitabilityItemsDto, PurchasingUnsuitabilityItems>();
            CreateMap<SelectPurchasingUnsuitabilityItemsDto, UpdatePurchasingUnsuitabilityItemsDto>();

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
        }
    }
}
