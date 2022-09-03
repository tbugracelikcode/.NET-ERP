using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Authentication.Dtos.Menus;
using Tsi.Authentication.Dtos.RolePermissions;
using Tsi.Authentication.Dtos.Roles;
using Tsi.Authentication.Entities.Menus;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.Authentication.Entities.Roles;
using Tsi.Core.Utilities.Guids;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Authentication.Menus;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.RolePermissions;

namespace TsiErp.Business.Entities.Authentication.RolePermissions
{
    [ServiceRegistration(typeof(IRolePermissionsAppService), DependencyInjectionType.Transient)]
    public class RolePermissionsAppService :  IRolePermissionsAppService
    {
        private readonly IRolePermissionsRepository _repository;

        public RolePermissionsAppService(IRolePermissionsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IDataResult<SelectRolePermissionsDto>> CreateAsync(CreateRolePermissionsDto input)
        {

            var entity = ObjectMapper.Map<CreateRolePermissionsDto, TsiRolePermissions>(input);

            foreach (var item in input.Menus)
            {
                entity.MenuId = item.Id;
                await _repository.InsertAsync(entity);
            }

            return new SuccessDataResult<SelectRolePermissionsDto>(ObjectMapper.Map<TsiRolePermissions, SelectRolePermissionsDto>(entity));
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<SelectRolePermissionsDto>> GetAsync(Guid id)
        {
            var entity = await _repository.GetAsync(t => t.Id == id, t => t.TsiMenus);

            var mappedEntity = ObjectMapper.Map<TsiRolePermissions, SelectRolePermissionsDto>(entity);

            return new SuccessDataResult<SelectRolePermissionsDto>(mappedEntity);
        }

        public async Task<IDataResult<IList<ListRolePermissionsDto>>> GetListAsync()
        {
            var list = await _repository.GetListAsync(null);

            var mappedEntity = ObjectMapper.Map<List<TsiRolePermissions>, List<ListRolePermissionsDto>>(list.ToList());

            return new SuccessDataResult<IList<ListRolePermissionsDto>>(mappedEntity);
        }

        public async Task<IDataResult<SelectRolePermissionsDto>> UpdateAsync(UpdateRolePermissionsDto input)
        {
            throw new NotImplementedException();
        }
    }
}
