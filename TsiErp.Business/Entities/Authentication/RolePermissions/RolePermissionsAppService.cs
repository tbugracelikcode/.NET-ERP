using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Authentication.Dtos.RolePermissions;
using Tsi.Authentication.Dtos.Roles;
using Tsi.Authentication.Entities.RolePermissions;
using Tsi.Authentication.Entities.Roles;
using Tsi.Core.Utilities.Guids;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.Logging.Tsi.Services;
using TsiErp.Business.Entities.Authentication.Menus;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Authentication.RolePermissions;
using TsiErp.DataAccess.EntityFrameworkCore.UnitOfWork;

namespace TsiErp.Business.Entities.Authentication.RolePermissions
{
    [ServiceRegistration(typeof(IRolePermissionsAppService), DependencyInjectionType.Singleton)]
    public class RolePermissionsAppService : ApplicationService, IRolePermissionsAppService
    {
        //private readonly IRolePermissionsRepository _repository;

        private readonly ILogsAppService _logger;

        private readonly IUnitOfWork _uow;

        public RolePermissionsAppService(ILogsAppService logger, IUnitOfWork uow)
        {
            _logger = logger;
            _uow = uow;

        }


        public async Task<IDataResult<SelectRolePermissionsDto>> CreateAsync(CreateRolePermissionsDto input)
        {
            var uow = _uow.GetRepository<TsiRolePermissions>();

            input.Menus.ForEach(t =>
            {
                var entity = ObjectMapper.Map<CreateRolePermissionsDto, TsiRolePermissions>(input);
                entity.Id = GuidGenerator.CreateGuid();
                entity.CreatorId = Guid.NewGuid();
                entity.CreationTime = DateTime.Now;
                entity.IsDeleted = false;
                entity.DeleterId = null;
                entity.DeletionTime = null;
                entity.LastModifierId = null;
                entity.LastModificationTime = null;
                entity.MenuId = t.Id;
                //_repository.InsertAsync(entity);
                uow.InsertAsync(entity);
            });

            _uow.SaveChanges();

            var completed = Task.CompletedTask;

            return new SuccessDataResult<SelectRolePermissionsDto>(ObjectMapper.Map<TsiRolePermissions, SelectRolePermissionsDto>(null));
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResult<SelectRolePermissionsDto>> GetAsync(Guid id)
        {
            var uow = _uow.GetRepository<TsiRolePermissions>();

            var entity = await uow.GetAsync(t => t.Id == id, t => t.TsiMenus);

            var mappedEntity = ObjectMapper.Map<TsiRolePermissions, SelectRolePermissionsDto>(entity);

            return new SuccessDataResult<SelectRolePermissionsDto>(mappedEntity);
        }

        public async Task<IDataResult<IList<ListRolePermissionsDto>>> GetListAsync()
        {
            

            throw new NotImplementedException();
        }

        public async Task<IDataResult<SelectRolePermissionsDto>> UpdateAsync(UpdateRolePermissionsDto input)
        {
            throw new NotImplementedException();
        }
    }
}
