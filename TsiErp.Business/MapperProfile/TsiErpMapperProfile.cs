using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Authentication.Dtos.Menus;
using Tsi.Authentication.Dtos.RolePermissions;
using Tsi.Authentication.Dtos.Roles;
using Tsi.Authentication.Entities.Menus;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.Authentication.Entities.Roles;
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

            CreateMap<Branches, SelectBranchesDto>();
            CreateMap<Branches, ListBranchesDto>();
            CreateMap<CreateBranchesDto, Branches>();
            CreateMap<SelectBranchesDto, CreateBranchesDto>();
            CreateMap<UpdateBranchesDto, Branches>();
            CreateMap<SelectBranchesDto, UpdateBranchesDto>();

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
            CreateMap<Employees, ListEmployeesDto>().ForMember(x => x.Department, y => y.MapFrom(z => z.Departments.Name));
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
            CreateMap<Products, ListProductsDto>().ForMember(x => x.UnitSet, y => y.MapFrom(z => z.UnitSets.Code))
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

            CreateMap<Warehouses, SelectWarehousesDto>();
            CreateMap<Warehouses, ListWarehousesDto>();
            CreateMap<CreateWarehousesDto, Warehouses>();
            CreateMap<SelectWarehousesDto, CreateWarehousesDto>();
            CreateMap<UpdateWarehousesDto, Warehouses>();
            CreateMap<SelectWarehousesDto, UpdateWarehousesDto>();

            CreateMap<SalesPropositions, SelectSalesPropositionsDto>()
                .ForMember(x => x.WarehouseCode, y => y.MapFrom(z => z.Warehouses.Name))
                .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Name))
                .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Name))
                .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
               /* .ForMember(x => x.PaymentPlanDay, y => y.MapFrom(z => z.PaymentPlan.Days_))*/;

            CreateMap<SalesPropositions, ListSalesPropositionsDto>()
                .ForMember(x => x.WarehouseCode, y => y.MapFrom(z => z.Warehouses.Name))
                .ForMember(x => x.BranchCode, y => y.MapFrom(z => z.Branches.Name))
                .ForMember(x => x.CurrencyCode, y => y.MapFrom(z => z.Currencies.Name))
                .ForMember(x => x.CurrentAccountCardName, y => y.MapFrom(z => z.CurrentAccountCards.Name))
                .ForMember(x => x.CurrentAccountCardCode, y => y.MapFrom(z => z.CurrentAccountCards.Code))
                .ForMember(x => x.PaymentPlanDay, y => y.MapFrom(z => z.PaymentPlan.Days_));

            CreateMap<UpdateSalesPropositionsDto, SalesPropositions>()
                .ForMember(x => x.SalesPropositionLines, y => y.Ignore());

            CreateMap<CreateSalesPropositionsDto, SalesPropositions>();
            CreateMap<SelectSalesPropositionsDto, CreateSalesPropositionsDto>();
            CreateMap<SelectSalesPropositionsDto, UpdateSalesPropositionsDto>();

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
        }
    }
}
