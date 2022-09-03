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
        }
    }
}
