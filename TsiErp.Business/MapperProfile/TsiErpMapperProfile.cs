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

namespace TsiErp.Business.MapperProfile
{
    public class TsiErpMapperProfile : Profile
    {
        public TsiErpMapperProfile()
        {
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
