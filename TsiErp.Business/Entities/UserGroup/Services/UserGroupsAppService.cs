using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UserGroup;
using TsiErp.EntityContracts.UserGroup;
using TsiErp.Entities.Entities.UserGroup.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.UserGroup;
using TsiErp.Business.Entities.UserGroup.BusinessRules;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.UserGroup.Services
{
    [ServiceRegistration(typeof(IUserGroupsAppService), DependencyInjectionType.Scoped)]
    public class UserGroupsAppService : ApplicationService, IUserGroupsAppService
    {

        UserGroupManager _manager { get; set; } = new UserGroupManager();

        [ValidationAspect(typeof(CreateUserGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUserGroupsDto>> CreateAsync(CreateUserGroupsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.UserGroupsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateUserGroupsDto, UserGroups>(input);

                var addedEntity = await _uow.UserGroupsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectUserGroupsDto>(ObjectMapper.Map<UserGroups, SelectUserGroupsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.UserGroupsRepository, id);
                await _uow.UserGroupsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectUserGroupsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UserGroupsRepository.GetAsync(t => t.Id == id, t => t.Users);
                var mappedEntity = ObjectMapper.Map<UserGroups, SelectUserGroupsDto>(entity);
                return new SuccessDataResult<SelectUserGroupsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUserGroupsDto>>> GetListAsync(ListUserGroupsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.UserGroupsRepository.GetListAsync(t => t.IsActive == input.IsActive, t => t.Users);

                var mappedEntity = ObjectMapper.Map<List<UserGroups>, List<ListUserGroupsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListUserGroupsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateUserGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUserGroupsDto>> UpdateAsync(UpdateUserGroupsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UserGroupsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.UserGroupsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateUserGroupsDto, UserGroups>(input);

                await _uow.UserGroupsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectUserGroupsDto>(ObjectMapper.Map<UserGroups, SelectUserGroupsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectUserGroupsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UserGroupsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.UserGroupsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<UserGroups, SelectUserGroupsDto>(updatedEntity);

                return new SuccessDataResult<SelectUserGroupsDto>(mappedEntity);
            }
        }
    }
}
