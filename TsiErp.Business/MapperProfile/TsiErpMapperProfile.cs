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

            CreateMap<Branches, SelectBranchesDto>();
            CreateMap<Branches, ListBranchesDto>();
            CreateMap<CreateBranchesDto, Branches>();
            CreateMap<SelectBranchesDto, CreateBranchesDto>();
            CreateMap<UpdateBranchesDto, Branches>();
            CreateMap<SelectBranchesDto, UpdateBranchesDto>();


            CreateMap<Periods, SelectPeriodsDto>()
                .ForMember(x => x.BranchName, y => y.MapFrom(z => z.Branches.Name));
            CreateMap<Periods, ListPeriodsDto>().ForMember(x => x.BranchName, y => y.MapFrom(z => z.Branches.Name));
            CreateMap<CreatePeriodsDto, Periods>();
            CreateMap<SelectPeriodsDto, CreatePeriodsDto>();
            CreateMap<UpdatePeriodsDto, Periods>();
            CreateMap<SelectPeriodsDto, UpdatePeriodsDto>();

            CreateMap<TsiRoles, SelectRolesDto>();
            CreateMap<TsiRoles, ListRolesDto>();
            CreateMap<CreateRolesDto, TsiRoles>();
            CreateMap<SelectRolesDto, CreateRolesDto>();
            CreateMap<UpdateRolesDto, TsiRoles>();
            CreateMap<SelectRolesDto, UpdateRolesDto>();

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


            CreateMap<TsiMenus, ListMenusDto>();



            CreateMap<TsiRolePermissions, SelectRolePermissionsDto>()
                .ForMember(x=>x.RoleName,y=>y.MapFrom(z=>z.TsiRoles.RoleName))
                .ForMember(x=>x.Menus,y=>y.MapFrom(z=>z.TsiMenus));

            CreateMap<TsiRolePermissions, ListRolePermissionsDto>()
                .ForMember(x => x.RoleName, y => y.MapFrom(z => z.TsiRoles.RoleName));
            CreateMap<CreateRolePermissionsDto, TsiRolePermissions>();
            CreateMap<SelectRolePermissionsDto, CreateRolePermissionsDto>();
            CreateMap<UpdateRolePermissionsDto, TsiRolePermissions>();
            CreateMap<SelectRolePermissionsDto, UpdateRolePermissionsDto>();

            CreateMap<Currencies, SelectCurrenciesDto>();
            CreateMap<Currencies, ListCurrenciesDto>();
            CreateMap<CreateCurrenciesDto, Currencies>();
            CreateMap<SelectCurrenciesDto, CreateCurrenciesDto>();
            CreateMap<UpdateCurrenciesDto, Currencies>();
            CreateMap<SelectCurrenciesDto, UpdateCurrenciesDto>();

            CreateMap<PaymentPlans, SelectPaymentPlansDto>();
            CreateMap<PaymentPlans, ListPaymentPlansDto>();
            CreateMap<CreatePaymentPlansDto, PaymentPlans>();
            CreateMap<SelectPaymentPlansDto, CreatePaymentPlansDto>();
            CreateMap<UpdatePaymentPlansDto, PaymentPlans>();
            CreateMap<SelectPaymentPlansDto, UpdatePaymentPlansDto>();

            CreateMap<Warehouses, SelectWarehousesDto>();
            CreateMap<Warehouses, ListWarehousesDto>();
            CreateMap<CreateWarehousesDto, Warehouses>();
            CreateMap<SelectWarehousesDto, CreateWarehousesDto>();
            CreateMap<UpdateWarehousesDto, Warehouses>();
            CreateMap<SelectWarehousesDto, UpdateWarehousesDto>();

            CreateMap<OperationUnsuitabilityItems, SelectOperationUnsuitabilityItemsDto>();
            CreateMap<OperationUnsuitabilityItems, ListOperationUnsuitabilityItemsDto>();
            CreateMap<CreateOperationUnsuitabilityItemsDto, OperationUnsuitabilityItems>();
            CreateMap<SelectOperationUnsuitabilityItemsDto, CreateOperationUnsuitabilityItemsDto>();
            CreateMap<UpdateOperationUnsuitabilityItemsDto, OperationUnsuitabilityItems>();
            CreateMap<SelectOperationUnsuitabilityItemsDto, UpdateOperationUnsuitabilityItemsDto>();

            CreateMap<FinalControlUnsuitabilityItems, SelectFinalControlUnsuitabilityItemsDto>();
            CreateMap<FinalControlUnsuitabilityItems, ListFinalControlUnsuitabilityItemsDto>();
            CreateMap<CreateFinalControlUnsuitabilityItemsDto, FinalControlUnsuitabilityItems>();
            CreateMap<SelectFinalControlUnsuitabilityItemsDto, CreateFinalControlUnsuitabilityItemsDto>();
            CreateMap<UpdateFinalControlUnsuitabilityItemsDto, FinalControlUnsuitabilityItems>();
            CreateMap<SelectFinalControlUnsuitabilityItemsDto, UpdateFinalControlUnsuitabilityItemsDto>();
        }
    }
}
