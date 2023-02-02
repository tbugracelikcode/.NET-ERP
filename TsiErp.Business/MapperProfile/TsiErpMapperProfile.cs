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

            CreateMap<StationInventories, SelectStationInventoriesDto>();
            CreateMap<StationInventories, ListStationInventoriesDto>();
            CreateMap<CreateStationInventoriesDto, StationInventories>();
            CreateMap<SelectStationInventoriesDto, CreateStationInventoriesDto>();
            CreateMap<UpdateStationInventoriesDto, StationInventories>();
            CreateMap<SelectStationInventoriesDto, UpdateStationInventoriesDto>();

            CreateMap<CalibrationRecords, SelectCalibrationRecordsDto>().ForMember(x => x.Equipment, y => y.MapFrom(z => z.EquipmentRecords.Name));
            CreateMap<CalibrationRecords, ListCalibrationRecordsDto>().ForMember(x => x.Equipment, y => y.MapFrom(z => z.EquipmentRecords.Name));
            CreateMap<CreateCalibrationRecordsDto, CalibrationRecords>();
            CreateMap<SelectCalibrationRecordsDto, CreateCalibrationRecordsDto>();
            CreateMap<UpdateCalibrationRecordsDto, CalibrationRecords>();
            CreateMap<SelectCalibrationRecordsDto, UpdateCalibrationRecordsDto>();

            CreateMap<CalibrationVerifications, SelectCalibrationVerificationsDto>().ForMember(x => x.Equipment, y => y.MapFrom(z => z.EquipmentRecords.Name));
            CreateMap<CalibrationVerifications, ListCalibrationVerificationsDto>().ForMember(x => x.Equipment, y => y.MapFrom(z => z.EquipmentRecords.Name));
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

            CreateMap<CurrentAccountCards, SelectCurrentAccountCardsDto>().ForMember(x => x.Currency, y => y.MapFrom(z => z.Currencies.Name));
            CreateMap<CurrentAccountCards, ListCurrentAccountCardsDto>().ForMember(x => x.Currency, y => y.MapFrom(z => z.Currencies.Name));
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

            CreateMap<Employees, SelectEmployeesDto>().ForMember(x => x.Department, y => y.MapFrom(z => z.Departments.Name));
            CreateMap<Employees, ListEmployeesDto>()
                .ForMember(x => x.Department, y => y.MapFrom(z => z.Departments.Name));
            CreateMap<CreateEmployeesDto, Employees>();
            CreateMap<SelectEmployeesDto, CreateEmployeesDto>();
            CreateMap<UpdateEmployeesDto, Employees>();
            CreateMap<SelectEmployeesDto, UpdateEmployeesDto>();

            CreateMap<EquipmentRecords, SelectEquipmentRecordsDto>().ForMember(x => x.DepartmentName, y => y.MapFrom(z => z.Departments.Name));
            CreateMap<EquipmentRecords, ListEquipmentRecordsDto>().ForMember(x => x.DepartmentName, y => y.MapFrom(z => z.Departments.Name));
            CreateMap<CreateEquipmentRecordsDto, EquipmentRecords>();
            CreateMap<SelectEquipmentRecordsDto, CreateEquipmentRecordsDto>();
            CreateMap<UpdateEquipmentRecordsDto, EquipmentRecords>();
            CreateMap<SelectEquipmentRecordsDto, UpdateEquipmentRecordsDto>();

            CreateMap<ExchangeRates, SelectExchangeRatesDto>().ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Name));
            CreateMap<ExchangeRates, ListExchangeRatesDto>().ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Name));
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

            CreateMap<Periods, SelectPeriodsDto>().ForMember(x => x.BranchName, y => y.MapFrom(z => z.Branches.Name));
            CreateMap<Periods, ListPeriodsDto>().ForMember(x => x.BranchName, y => y.MapFrom(z => z.Branches.Name));
            CreateMap<CreatePeriodsDto, Periods>();
            CreateMap<SelectPeriodsDto, CreatePeriodsDto>();
            CreateMap<UpdatePeriodsDto, Periods>();
            CreateMap<SelectPeriodsDto, UpdatePeriodsDto>();

            CreateMap<Products, SelectProductsDto>().ForMember(x => x.UnitSet, y => y.MapFrom(z => z.UnitSets.Code))
                .ForMember(x => x.ProductGrp, y => y.MapFrom(z => z.ProductGroups.Code));
            CreateMap<Products, ListProductsDto>().ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
                .ForMember(x => x.ProductGrp, y => y.MapFrom(z => z.ProductGroups.Code));
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

            CreateMap<Stations, SelectStationsDto>().ForMember(x => x.StationGroup, y => y.MapFrom(z => z.StationGroups.Name));
            CreateMap<Stations, ListStationsDto>().ForMember(x => x.StationGroup, y => y.MapFrom(z => z.StationGroups.Name));
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


            CreateMap<SalesPropositions, SelectSalesPropositionsDto>()
                .ForMember(x => x.WarehouseCode, y => y.MapFrom(z => z.Warehouses.Code))
                .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Name))
                .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Name))
                .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlan.Name));

            CreateMap<SalesPropositions, ListSalesPropositionsDto>()
                .ForMember(x => x.WarehouseCode, y => y.MapFrom(z => z.Warehouses.Code))
                .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Name))
                .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Name))
                .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlan.Name));

            CreateMap<UpdateSalesPropositionsDto, SalesPropositions>()
                .ForMember(x => x.SalesPropositionLines, y => y.Ignore());

            CreateMap<CreateSalesPropositionsDto, SalesPropositions>();
            CreateMap<SelectSalesPropositionsDto, CreateSalesPropositionsDto>();
            CreateMap<SelectSalesPropositionsDto, UpdateSalesPropositionsDto>();


            CreateMap<SalesPropositionLines, SelectSalesPropositionLinesDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
                .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
               .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlans.Name));
            CreateMap<SalesPropositionLines, ListSalesPropositionLinesDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
                .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
               .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlans.Name));
            CreateMap<CreateSalesPropositionLinesDto, SalesPropositionLines>();
            CreateMap<SelectSalesPropositionLinesDto, CreateSalesPropositionLinesDto>();
            CreateMap<UpdateSalesPropositionLinesDto, SalesPropositionLines>();
            CreateMap<SelectSalesPropositionLinesDto, UpdateSalesPropositionLinesDto>();
            CreateMap<SelectSalesPropositionLinesDto, SalesPropositionLines>();



            CreateMap<SalesOrders, SelectSalesOrderDto>()
                .ForMember(x => x.WarehouseCode, y => y.MapFrom(z => z.Warehouses.Code))
                .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Name))
                .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Name))
                .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlan.Name));

            CreateMap<SalesOrders, ListSalesOrderDto>()
                .ForMember(x => x.WarehouseCode, y => y.MapFrom(z => z.Warehouses.Code))
                .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Name))
                .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Name))
                .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlan.Name));

            CreateMap<UpdateSalesOrderDto, SalesOrders>()
                .ForMember(x => x.SalesOrderLines, y => y.Ignore());

            CreateMap<CreateSalesOrderDto, SalesOrders>();
            CreateMap<SelectSalesOrderDto, CreateSalesOrderDto>();
            CreateMap<SelectSalesOrderDto, UpdateSalesOrderDto>();



            CreateMap<SalesOrderLines, SelectSalesOrderLinesDto>()
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
               .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
              .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlans.Name));
            CreateMap<SalesOrderLines, ListSalesOrderLinesDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
                .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
               .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlans.Name));
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

            CreateMap<ShippingAdresses, SelectShippingAdressesDto>().ForMember(x => x.CustomerCard, y => y.MapFrom(z => z.CurrentAccountCards.Name));
            CreateMap<ShippingAdresses, ListShippingAdressesDto>().ForMember(x => x.CustomerCard, y => y.MapFrom(z => z.CurrentAccountCards.Name));
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


            CreateMap<Routes, SelectRoutesDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<Routes, ListRoutesDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<CreateRoutesDto, Routes>();
            CreateMap<SelectRoutesDto, CreateRoutesDto>();
            CreateMap<UpdateRoutesDto, Routes>()
                .ForMember(x => x.RouteLines, y => y.Ignore());
            CreateMap<SelectRoutesDto, UpdateRoutesDto>();


            CreateMap<RouteLines, SelectRouteLinesDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.RouteCode, y => y.MapFrom(z => z.Routes.Code))
                .ForMember(x => x.OperationName, y => y.MapFrom(z => z.ProductsOperations.Name))
                .ForMember(x => x.OperationCode, y => y.MapFrom(z => z.ProductsOperations.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<RouteLines, ListRouteLinesDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.RouteCode, y => y.MapFrom(z => z.Routes.Code))
                .ForMember(x => x.OperationName, y => y.MapFrom(z => z.ProductsOperations.Name))
                .ForMember(x => x.OperationCode, y => y.MapFrom(z => z.ProductsOperations.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<CreateRouteLinesDto, RouteLines>();
            CreateMap<SelectRouteLinesDto, RouteLines>();
            CreateMap<SelectRouteLinesDto, CreateRouteLinesDto>();
            CreateMap<UpdateRouteLinesDto, RouteLines>();
            CreateMap<SelectRouteLinesDto, UpdateRouteLinesDto>();


            CreateMap<Shifts, SelectShiftsDto>();
            CreateMap<Shifts, ListShiftsDto>();
            CreateMap<UpdateShiftsDto, Shifts>()
                .ForMember(x => x.ShiftLines, y => y.Ignore());
            CreateMap<CreateShiftsDto, Shifts>();
            CreateMap<SelectShiftsDto, CreateShiftsDto>();
            CreateMap<SelectShiftsDto, UpdateShiftsDto>();


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
            CreateMap<UpdateCalendarsDto, Calendars>()
                .ForMember(x => x.CalendarLines, y => y.Ignore());
            CreateMap<SelectCalendarsDto, UpdateCalendarsDto>();

            CreateMap<CalendarDays, SelectCalendarDaysDto>();
            CreateMap<CalendarDays, ListCalendarDaysDto>();
            CreateMap<CreateCalendarDaysDto, CalendarDays>();
            CreateMap<SelectCalendarDaysDto, CreateCalendarDaysDto>();
            CreateMap<UpdateCalendarDaysDto, CalendarDays>();
            CreateMap<SelectCalendarDaysDto, UpdateCalendarDaysDto>();
            CreateMap<SelectCalendarDaysDto, CalendarDays>();

            CreateMap<CalendarLines, SelectCalendarLinesDto>()
                .ForMember(x => x.StationName, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.ShiftOverTime, y => y.MapFrom(z => z.Shifts.Overtime))
                .ForMember(x => x.ShiftOrder, y => y.MapFrom(z => z.Shifts.ShiftOrder))
                .ForMember(x => x.ShiftName, y => y.MapFrom(z => z.Shifts.Code))
                .ForMember(x => x.ShiftTime, y => y.MapFrom(z => z.Shifts.TotalWorkTime));
            CreateMap<CalendarLines, ListCalendarLinesDto>()
                .ForMember(x => x.StationName, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.ShiftOverTime, y => y.MapFrom(z => z.Shifts.Overtime))
                .ForMember(x => x.ShiftOrder, y => y.MapFrom(z => z.Shifts.ShiftOrder))
                .ForMember(x => x.ShiftName, y => y.MapFrom(z => z.Shifts.Code))
                .ForMember(x => x.ShiftTime, y => y.MapFrom(z => z.Shifts.TotalWorkTime));
            CreateMap<CreateCalendarLinesDto, CalendarLines>();
            CreateMap<SelectCalendarLinesDto, CreateCalendarLinesDto>();
            CreateMap<UpdateCalendarLinesDto, CalendarLines>();
            CreateMap<SelectCalendarLinesDto, UpdateCalendarLinesDto>();
            CreateMap<SelectCalendarLinesDto, CalendarLines>();


            CreateMap<TemplateOperations, SelectTemplateOperationsDto>();
            CreateMap<TemplateOperations, ListTemplateOperationsDto>();
            CreateMap<UpdateTemplateOperationsDto, TemplateOperations>()
                .ForMember(x => x.TemplateOperationLines, y => y.Ignore());
            CreateMap<CreateTemplateOperationsDto, TemplateOperations>();
            CreateMap<SelectTemplateOperationsDto, CreateTemplateOperationsDto>();
            CreateMap<SelectTemplateOperationsDto, UpdateTemplateOperationsDto>();


            CreateMap<TemplateOperationLines, SelectTemplateOperationLinesDto>()
                .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.StationName, y => y.MapFrom(z => z.Stations.Name))
                .ForMember(x => x.TemplateOperationName, y => y.MapFrom(z => z.TemplateOperations.Name));
            CreateMap<TemplateOperationLines, ListTemplateOperationLinesDto>()
                .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.StationName, y => y.MapFrom(z => z.Stations.Name))
                .ForMember(x => x.TemplateOperationName, y => y.MapFrom(z => z.TemplateOperations.Name));
            CreateMap<CreateTemplateOperationLinesDto, TemplateOperationLines>();
            CreateMap<SelectTemplateOperationLinesDto, CreateTemplateOperationLinesDto>();
            CreateMap<UpdateTemplateOperationLinesDto, TemplateOperationLines>();
            CreateMap<SelectTemplateOperationLinesDto, UpdateTemplateOperationLinesDto>();
            CreateMap<SelectTemplateOperationLinesDto, TemplateOperationLines>();


            CreateMap<ProductsOperations, SelectProductsOperationsDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<ProductsOperations, ListProductsOperationsDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
                .ForMember(x => x.ProductId, y => y.MapFrom(z => z.Products.Id));
            CreateMap<UpdateProductsOperationsDto, ProductsOperations>()
                .ForMember(x => x.ProductsOperationLines, y => y.Ignore());
            CreateMap<CreateProductsOperationsDto, ProductsOperations>();
            CreateMap<SelectProductsOperationsDto, CreateProductsOperationsDto>();
            CreateMap<SelectProductsOperationsDto, UpdateProductsOperationsDto>();


            CreateMap<ProductsOperationLines, SelectProductsOperationLinesDto>()
                .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.StationName, y => y.MapFrom(z => z.Stations.Name))
                .ForMember(x => x.ProductsOperationName, y => y.MapFrom(z => z.ProductsOperations.Name))
                .ForMember(x => x.ProductsOperationCode, y => y.MapFrom(z => z.ProductsOperations.Code));
            CreateMap<ProductsOperationLines, ListProductsOperationLinesDto>()
                .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.StationName, y => y.MapFrom(z => z.Stations.Name))
                .ForMember(x => x.ProductsOperationName, y => y.MapFrom(z => z.ProductsOperations.Name))
                .ForMember(x => x.ProductsOperationCode, y => y.MapFrom(z => z.ProductsOperations.Code));
            CreateMap<CreateProductsOperationLinesDto, ProductsOperationLines>();
            CreateMap<SelectProductsOperationLinesDto, CreateProductsOperationLinesDto>();
            CreateMap<UpdateProductsOperationLinesDto, ProductsOperationLines>();
            CreateMap<SelectProductsOperationLinesDto, UpdateProductsOperationLinesDto>();
            CreateMap<SelectProductsOperationLinesDto, ProductsOperationLines>();


            CreateMap<BillsofMaterials, SelectBillsofMaterialsDto>()
                .ForMember(x => x.FinishedProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.FinishedProducName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<BillsofMaterials, ListBillsofMaterialsDto>()
                .ForMember(x => x.FinishedProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.FinishedProducName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<UpdateBillsofMaterialsDto, BillsofMaterials>()
                .ForMember(x => x.BillsofMaterialLines, y => y.Ignore());
            CreateMap<CreateBillsofMaterialsDto, BillsofMaterials>();
            CreateMap<SelectBillsofMaterialsDto, CreateBillsofMaterialsDto>();
            CreateMap<SelectBillsofMaterialsDto, UpdateBillsofMaterialsDto>();


            CreateMap<BillsofMaterialLines, SelectBillsofMaterialLinesDto>()
                .ForMember(x => x.FinishedProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<BillsofMaterialLines, ListBillsofMaterialLinesDto>()
                .ForMember(x => x.FinishedProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<CreateBillsofMaterialLinesDto, BillsofMaterialLines>();
            CreateMap<SelectBillsofMaterialLinesDto, CreateBillsofMaterialLinesDto>();
            CreateMap<UpdateBillsofMaterialLinesDto, BillsofMaterialLines>();
            CreateMap<SelectBillsofMaterialLinesDto, UpdateBillsofMaterialLinesDto>();
            CreateMap<SelectBillsofMaterialLinesDto, BillsofMaterialLines>();

            CreateMap<ProductionOrders, SelectProductionOrdersDto>()
                .ForMember(x => x.OrderFicheNo, y => y.MapFrom(z => z.SalesOrders.FicheNo))
                .ForMember(x => x.PropositionFicheNo, y => y.MapFrom(z => z.SalesPropositions.FicheNo))
                .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
                .ForMember(x => x.BOMCode, y => y.MapFrom(z => z.BillsofMaterials.Code))
                .ForMember(x => x.BOMName, y => y.MapFrom(z => z.BillsofMaterials.Name))
                .ForMember(x => x.CurrentAccountCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.CurrentAccountName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
                .ForMember(x => x.RouteCode, y => y.MapFrom(z => z.Routes.Code))
                .ForMember(x => x.RouteName, y => y.MapFrom(z => z.Routes.Name))
                .ForMember(x => x.FinishedProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.FinishedProductName, y => y.MapFrom(z => z.Products.Name))
                .ForMember(x => x.LinkedProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.LinkedProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<ProductionOrders, ListProductionOrdersDto>()
                .ForMember(x => x.OrderFicheNo, y => y.MapFrom(z => z.SalesOrders.FicheNo))
                .ForMember(x => x.PropositionFicheNo, y => y.MapFrom(z => z.SalesPropositions.FicheNo))
                .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
                .ForMember(x => x.BOMCode, y => y.MapFrom(z => z.BillsofMaterials.Code))
                .ForMember(x => x.BOMName, y => y.MapFrom(z => z.BillsofMaterials.Name))
                .ForMember(x => x.CurrentAccountCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.CurrentAccountName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
                .ForMember(x => x.RouteCode, y => y.MapFrom(z => z.Routes.Code))
                .ForMember(x => x.RouteName, y => y.MapFrom(z => z.Routes.Name))
                .ForMember(x => x.FinishedProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.FinishedProductName, y => y.MapFrom(z => z.Products.Name))
                .ForMember(x => x.LinkedProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.LinkedProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<CreateProductionOrdersDto, ProductionOrders>();
            CreateMap<SelectProductionOrdersDto, CreateProductionOrdersDto>();
            CreateMap<UpdateProductionOrdersDto, ProductionOrders>();
            CreateMap<SelectProductionOrdersDto, UpdateProductionOrdersDto>();


            CreateMap<WorkOrders, SelectWorkOrdersDto>()
               .ForMember(x => x.ProductionOrderFicheNo, y => y.MapFrom(z => z.ProductionOrders.FicheNo))
               .ForMember(x => x.PropositionFicheNo, y => y.MapFrom(z => z.SalesPropositions.FicheNo))
               .ForMember(x => x.RouteCode, y => y.MapFrom(z => z.Routes.Code))
               .ForMember(x => x.ProductsOperationCode, y => y.MapFrom(z => z.ProductsOperations.Code))
               .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
               .ForMember(x => x.StationName, y => y.MapFrom(z => z.Stations.Name))
               .ForMember(x => x.StationGroupCode, y => y.MapFrom(z => z.StationGroups.Code))
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
               .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
               .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name));
            CreateMap<WorkOrders, ListWorkOrdersDto>()
                 .ForMember(x => x.ProductionOrderFicheNo, y => y.MapFrom(z => z.ProductionOrders.FicheNo))
               .ForMember(x => x.PropositionFicheNo, y => y.MapFrom(z => z.SalesPropositions.FicheNo))
               .ForMember(x => x.RouteCode, y => y.MapFrom(z => z.Routes.Code))
               .ForMember(x => x.ProductsOperationCode, y => y.MapFrom(z => z.ProductsOperations.Code))
               .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
               .ForMember(x => x.StationName, y => y.MapFrom(z => z.Stations.Name))
               .ForMember(x => x.StationGroupCode, y => y.MapFrom(z => z.StationGroups.Code))
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
               .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
               .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name));
            CreateMap<CreateWorkOrdersDto, WorkOrders>();
            CreateMap<SelectWorkOrdersDto, CreateWorkOrdersDto>();
            CreateMap<UpdateWorkOrdersDto, WorkOrders>();
            CreateMap<SelectWorkOrdersDto, UpdateWorkOrdersDto>();


            CreateMap<PurchaseOrders, SelectPurchaseOrdersDto>()
               .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlan.Name))
               .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Code))
               .ForMember(x => x.BranchName, y => y.MapFrom(z => z.Branches.Name))
               .ForMember(x => x.WarehouseCode, y => y.MapFrom(z => z.Warehouses.Code))
               .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code))
               .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
               .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
               .ForMember(x => x.ShippingAdressCode, y => y.MapFrom(z => z.ShippingAdresses.Code))
               .ForMember(x => x.ShippingAdressName, y => y.MapFrom(z => z.ShippingAdresses.Name));
            CreateMap<PurchaseOrders, ListPurchaseOrdersDto>()
               .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlan.Name))
               .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Code))
               .ForMember(x => x.BranchName, y => y.MapFrom(z => z.Branches.Name))
               .ForMember(x => x.WarehouseCode, y => y.MapFrom(z => z.Warehouses.Code))
               .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code))
               .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
               .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
               .ForMember(x => x.ShippingAdressCode, y => y.MapFrom(z => z.ShippingAdresses.Code))
               .ForMember(x => x.ShippingAdressName, y => y.MapFrom(z => z.ShippingAdresses.Name));
            CreateMap<UpdatePurchaseOrdersDto, PurchaseOrders>()
                .ForMember(x => x.PurchaseOrderLines, y => y.Ignore());
            CreateMap<CreatePurchaseOrdersDto, PurchaseOrders>();
            CreateMap<SelectPurchaseOrdersDto, CreatePurchaseOrdersDto>();
            CreateMap<SelectPurchaseOrdersDto, UpdatePurchaseOrdersDto>();


            CreateMap<PurchaseOrderLines, SelectPurchaseOrderLinesDto>()
               .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlans.Name))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<PurchaseOrderLines, ListPurchaseOrderLinesDto>()
               .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlans.Name))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<CreatePurchaseOrderLinesDto, PurchaseOrderLines>();
            CreateMap<SelectPurchaseOrderLinesDto, CreatePurchaseOrderLinesDto>();
            CreateMap<UpdatePurchaseOrderLinesDto, PurchaseOrderLines>();
            CreateMap<SelectPurchaseOrderLinesDto, UpdatePurchaseOrderLinesDto>();
            CreateMap<SelectPurchaseOrderLinesDto, PurchaseOrderLines>();

            CreateMap<PurchaseRequests, SelectPurchaseRequestsDto>()
              .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlan.Name))
              .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Code))
              .ForMember(x => x.BranchName, y => y.MapFrom(z => z.Branches.Name))
              .ForMember(x => x.WarehouseCode, y => y.MapFrom(z => z.Warehouses.Code))
              .ForMember(x => x.WarehouseName, y => y.MapFrom(z => z.Warehouses.Name))
              .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code))
              .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
              .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
              .ForMember(x => x.ShippingAdressCode, y => y.MapFrom(z => z.ShippingAdresses.Code));
            CreateMap<PurchaseRequests, ListPurchaseRequestsDto>()
              .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlan.Name))
              .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Code))
              .ForMember(x => x.BranchName, y => y.MapFrom(z => z.Branches.Name))
              .ForMember(x => x.WarehouseCode, y => y.MapFrom(z => z.Warehouses.Code))
              .ForMember(x => x.WarehouseName, y => y.MapFrom(z => z.Warehouses.Name))
              .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code))
              .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
              .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
              .ForMember(x => x.ShippingAdressCode, y => y.MapFrom(z => z.ShippingAdresses.Code));
            CreateMap<UpdatePurchaseRequestsDto, PurchaseRequests>()
                .ForMember(x => x.PurchaseRequestLines, y => y.Ignore());
            CreateMap<CreatePurchaseRequestsDto, PurchaseRequests>();
            CreateMap<SelectPurchaseRequestsDto, CreatePurchaseRequestsDto>();
            CreateMap<SelectPurchaseRequestsDto, UpdatePurchaseRequestsDto>();
            CreateMap<SelectPurchaseRequestsDto, PurchaseRequests>();
            CreateMap<UpdatePurchaseRequestsDto, PurchaseRequests>();


            CreateMap<PurchaseRequestLines, SelectPurchaseRequestLinesDto>()
               .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlans.Name))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<PurchaseRequestLines, ListPurchaseRequestLinesDto>()
               .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code))
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.PaymentPlanName, y => y.MapFrom(z => z.PaymentPlans.Name))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<CreatePurchaseRequestLinesDto, PurchaseRequestLines>();
            CreateMap<SelectPurchaseRequestLinesDto, CreatePurchaseRequestLinesDto>();
            CreateMap<UpdatePurchaseRequestLinesDto, PurchaseRequestLines>();
            CreateMap<SelectPurchaseRequestLinesDto, UpdatePurchaseRequestLinesDto>();
            CreateMap<SelectPurchaseRequestLinesDto, PurchaseRequestLines>();


            CreateMap<PurchaseUnsuitabilityReports, SelectPurchaseUnsuitabilityReportsDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
                .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.OrderFicheNo, y => y.MapFrom(z => z.PurchaseOrders.FicheNo));
            CreateMap<PurchaseUnsuitabilityReports, ListPurchaseUnsuitabilityReportsDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
                .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.OrderFicheNo, y => y.MapFrom(z => z.PurchaseOrders.FicheNo));
            CreateMap<CreatePurchaseUnsuitabilityReportsDto, PurchaseUnsuitabilityReports>();
            CreateMap<SelectPurchaseUnsuitabilityReportsDto, CreatePurchaseUnsuitabilityReportsDto>();
            CreateMap<UpdatePurchaseUnsuitabilityReportsDto, PurchaseUnsuitabilityReports>();
            CreateMap<SelectPurchaseUnsuitabilityReportsDto, UpdatePurchaseUnsuitabilityReportsDto>();

            CreateMap<OperationUnsuitabilityReports, SelectOperationUnsuitabilityReportsDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
                .ForMember(x => x.WorkOrderNo, y => y.MapFrom(z => z.WorkOrders.WorkOrderNo))
                .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.StationName, y => y.MapFrom(z => z.Stations.Name))
                .ForMember(x => x.StationGroupCode, y => y.MapFrom(z => z.StationGroups.Code))
                .ForMember(x => x.StationGroupName, y => y.MapFrom(z => z.StationGroups.Name))
                .ForMember(x => x.EmployeeName, y => y.MapFrom(z => z.Employees.Name))
                .ForMember(x => x.ProductionOrderFicheNo, y => y.MapFrom(z => z.ProductionOrders.FicheNo))
                .ForMember(x => x.OperationCode, y => y.MapFrom(z => z.ProductsOperations.Code))
                .ForMember(x => x.OperationName, y => y.MapFrom(z => z.ProductsOperations.Name));
            CreateMap<OperationUnsuitabilityReports, ListOperationUnsuitabilityReportsDto>()
                .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
                .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
                .ForMember(x => x.WorkOrderNo, y => y.MapFrom(z => z.WorkOrders.WorkOrderNo))
                .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.StationName, y => y.MapFrom(z => z.Stations.Name))
                .ForMember(x => x.StationGroupCode, y => y.MapFrom(z => z.StationGroups.Code))
                .ForMember(x => x.StationGroupName, y => y.MapFrom(z => z.StationGroups.Name))
                .ForMember(x => x.EmployeeName, y => y.MapFrom(z => z.Employees.Name))
                .ForMember(x => x.ProductionOrderFicheNo, y => y.MapFrom(z => z.ProductionOrders.FicheNo))
                .ForMember(x => x.OperationCode, y => y.MapFrom(z => z.ProductsOperations.Code))
                .ForMember(x => x.OperationName, y => y.MapFrom(z => z.ProductsOperations.Name));
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

            CreateMap<ProductionTrackings, SelectProductionTrackingsDto>()
                .ForMember(x => x.ShiftCode, y => y.MapFrom(z => z.Shifts.Code))
                .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.EmployeeName, y => y.MapFrom(z => z.Employees.Name));
            CreateMap<ProductionTrackings, ListProductionTrackingsDto>()
                .ForMember(x => x.ShiftCode, y => y.MapFrom(z => z.Shifts.Code))
                .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.EmployeeName, y => y.MapFrom(z => z.Employees.Name));
            CreateMap<CreateProductionTrackingsDto, ProductionTrackings>();
            CreateMap<SelectProductionTrackingsDto, CreateProductionTrackingsDto>();
            CreateMap<UpdateProductionTrackingsDto, ProductionTrackings>();
            CreateMap<SelectProductionTrackingsDto, UpdateProductionTrackingsDto>();
            CreateMap<SelectProductionTrackingsDto, ProductionTrackings>();

            CreateMap<ProductionTrackingHaltLines, SelectProductionTrackingHaltLinesDto>()
                .ForMember(x => x.HaltCode, y => y.MapFrom(z => z.HaltReasons.Code))
                .ForMember(x => x.HaltName, y => y.MapFrom(z => z.HaltReasons.Name));
            CreateMap<ProductionTrackingHaltLines, ListProductionTrackingHaltLinesDto>()
                .ForMember(x => x.HaltCode, y => y.MapFrom(z => z.HaltReasons.Code))
                .ForMember(x => x.HaltName, y => y.MapFrom(z => z.HaltReasons.Name));
            CreateMap<CreateProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>();
            CreateMap<SelectProductionTrackingHaltLinesDto, CreateProductionTrackingHaltLinesDto>();
            CreateMap<UpdateProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>();
            CreateMap<SelectProductionTrackingHaltLinesDto, UpdateProductionTrackingHaltLinesDto>();
            CreateMap<SelectProductionTrackingHaltLinesDto, ProductionTrackingHaltLines>();

            CreateMap<ContractProductionTrackings, SelectContractProductionTrackingsDto>()
                .ForMember(x => x.ShiftCode, y => y.MapFrom(z => z.Shifts.Code))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.EmployeeName, y => y.MapFrom(z => z.Employees.Name));
            CreateMap<ContractProductionTrackings, ListContractProductionTrackingsDto>()
                .ForMember(x => x.ShiftCode, y => y.MapFrom(z => z.Shifts.Code))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
                .ForMember(x => x.EmployeeName, y => y.MapFrom(z => z.Employees.Name));
            CreateMap<CreateContractProductionTrackingsDto, ContractProductionTrackings>();
            CreateMap<SelectContractProductionTrackingsDto, CreateContractProductionTrackingsDto>();
            CreateMap<UpdateContractProductionTrackingsDto, ContractProductionTrackings>();
            CreateMap<SelectContractProductionTrackingsDto, UpdateContractProductionTrackingsDto>();
            CreateMap<SelectContractProductionTrackingsDto, ContractProductionTrackings>();

            CreateMap<Forecasts, SelectForecastsDto>()
           .ForMember(x => x.PeriodCode, y => y.MapFrom(z => z.Periods.Code))
           .ForMember(x => x.PeriodName, y => y.MapFrom(z => z.Periods.Name))
           .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Code))
           .ForMember(x => x.BranchName, y => y.MapFrom(z => z.Branches.Name))
           .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
           .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name));
            CreateMap<Forecasts, ListForecastsDto>()
              .ForMember(x => x.PeriodCode, y => y.MapFrom(z => z.Periods.Code))
           .ForMember(x => x.PeriodName, y => y.MapFrom(z => z.Periods.Name))
           .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Code))
           .ForMember(x => x.BranchName, y => y.MapFrom(z => z.Branches.Name))
           .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
           .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name));
            CreateMap<UpdateForecastsDto, Forecasts>()
                .ForMember(x => x.ForecastLines, y => y.Ignore());
            CreateMap<CreateForecastsDto, Forecasts>();
            CreateMap<SelectForecastsDto, CreateForecastsDto>();
            CreateMap<SelectForecastsDto, UpdateForecastsDto>();


            CreateMap<ForecastLines, SelectForecastLinesDto>()
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<ForecastLines, ListForecastLinesDto>()
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<CreateForecastLinesDto, ForecastLines>();
            CreateMap<SelectForecastLinesDto, CreateForecastLinesDto>();
            CreateMap<UpdateForecastLinesDto, ForecastLines>();
            CreateMap<SelectForecastLinesDto, UpdateForecastLinesDto>();
            CreateMap<SelectForecastLinesDto, ForecastLines>();


            CreateMap<SalesPrices, SelectSalesPricesDto>().ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code));
            CreateMap<SalesPrices, ListSalesPricesDto>().ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code));
            CreateMap<UpdateSalesPricesDto, SalesPrices>()
                .ForMember(x => x.SalesPriceLines, y => y.Ignore());
            CreateMap<CreateSalesPricesDto, SalesPrices>();
            CreateMap<SelectSalesPricesDto, CreateSalesPricesDto>();
            CreateMap<SelectSalesPricesDto, UpdateSalesPricesDto>();


            CreateMap<SalesPriceLines, SelectSalesPriceLinesDto>()
               .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code))
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<SalesPriceLines, ListSalesPriceLinesDto>()
               .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code))
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<CreateSalesPriceLinesDto, SalesPriceLines>();
            CreateMap<SelectSalesPriceLinesDto, CreateSalesPriceLinesDto>();
            CreateMap<UpdateSalesPriceLinesDto, SalesPriceLines>();
            CreateMap<SelectSalesPriceLinesDto, UpdateSalesPriceLinesDto>();
            CreateMap<SelectSalesPriceLinesDto, SalesPriceLines>();


            CreateMap<PurchasePrices, SelectPurchasePricesDto>()
                .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name));
            CreateMap<PurchasePrices, ListPurchasePricesDto>()
                .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name));
            CreateMap<UpdatePurchasePricesDto, PurchasePrices>()
                .ForMember(x => x.PurchasePriceLines, y => y.Ignore());
            CreateMap<CreatePurchasePricesDto, PurchasePrices>();
            CreateMap<SelectPurchasePricesDto, CreatePurchasePricesDto>();
            CreateMap<SelectPurchasePricesDto, UpdatePurchasePricesDto>();


            CreateMap<PurchasePriceLines, SelectPurchasePriceLinesDto>()
               .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code))
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
            CreateMap<PurchasePriceLines, ListPurchasePriceLinesDto>()
               .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Code))
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name));
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


            CreateMap<Users, SelectUsersDto>().ForMember(x => x.GroupName, y => y.MapFrom(z => z.UserGroups.Name));
            CreateMap<Users, ListUsersDto>().ForMember(x => x.GroupName, y => y.MapFrom(z => z.UserGroups.Name));
            CreateMap<CreateUsersDto, Users>();
            CreateMap<SelectUsersDto, CreateUsersDto>();
            CreateMap<UpdateUsersDto, Users>();
            CreateMap<SelectUsersDto, UpdateUsersDto>();


            CreateMap<FinalControlUnsuitabilityReports, SelectFinalControlUnsuitabilityReportsDto>()
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
               .ForMember(x => x.EmployeeName, y => y.MapFrom(z => z.Employees.Name));
            CreateMap<FinalControlUnsuitabilityReports, ListFinalControlUnsuitabilityReportsDto>()
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.ProductName, y => y.MapFrom(z => z.Products.Name))
               .ForMember(x => x.EmployeeName, y => y.MapFrom(z => z.Employees.Name));
            CreateMap<CreateFinalControlUnsuitabilityReportsDto, FinalControlUnsuitabilityReports>();
            CreateMap<SelectFinalControlUnsuitabilityReportsDto, CreateFinalControlUnsuitabilityReportsDto>();
            CreateMap<UpdateFinalControlUnsuitabilityReportsDto, FinalControlUnsuitabilityReports>();
            CreateMap<SelectFinalControlUnsuitabilityReportsDto, UpdateFinalControlUnsuitabilityReportsDto>();



            CreateMap<MaintenanceInstructions, SelectMaintenanceInstructionsDto>()
            .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
            .ForMember(x => x.PeriodName, y => y.MapFrom(z => z.MaintenancePeriods.Name));

            CreateMap<MaintenanceInstructions, ListMaintenanceInstructionsDto>()
            .ForMember(x => x.StationCode, y => y.MapFrom(z => z.Stations.Code))
            .ForMember(x => x.PeriodName, y => y.MapFrom(z => z.MaintenancePeriods.Name));

            CreateMap<UpdateMaintenanceInstructionsDto, MaintenanceInstructions>()
                .ForMember(x => x.MaintenanceInstructionLines, y => y.Ignore());

            CreateMap<CreateMaintenanceInstructionsDto, MaintenanceInstructions>();
            CreateMap<SelectMaintenanceInstructionsDto, CreateMaintenanceInstructionsDto>();
            CreateMap<SelectMaintenanceInstructionsDto, UpdateMaintenanceInstructionsDto>();



            CreateMap<MaintenanceInstructionLines, SelectMaintenanceInstructionLinesDto>()
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code));
            CreateMap<MaintenanceInstructionLines, ListMaintenanceInstructionLinesDto>()
               .ForMember(x => x.ProductCode, y => y.MapFrom(z => z.Products.Code))
               .ForMember(x => x.UnitSetCode, y => y.MapFrom(z => z.UnitSets.Code));
            CreateMap<CreateMaintenanceInstructionLinesDto, MaintenanceInstructionLines>();
            CreateMap<SelectMaintenanceInstructionLinesDto, CreateMaintenanceInstructionLinesDto>();
            CreateMap<UpdateMaintenanceInstructionLinesDto, MaintenanceInstructionLines>();
            CreateMap<SelectMaintenanceInstructionLinesDto, UpdateMaintenanceInstructionLinesDto>();
            CreateMap<SelectMaintenanceInstructionLinesDto, MaintenanceInstructionLines>();
        }
    }
}
