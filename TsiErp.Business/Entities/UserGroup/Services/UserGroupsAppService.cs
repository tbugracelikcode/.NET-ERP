using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.UserGroups.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.UserGroup.BusinessRules;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.UserGroup;
using TsiErp.Entities.Entities.UserGroup.Dtos;
using TsiErp.EntityContracts.UserGroup;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.UserGroup.Services
{
    [ServiceRegistration(typeof(IUserGroupsAppService), DependencyInjectionType.Scoped)]
    public class UserGroupsAppService : ApplicationService<UserGroupsResource>, IUserGroupsAppService
    {
        public UserGroupsAppService(IStringLocalizer<UserGroupsResource> l) : base(l)
        {
        }

        UserGroupManager _manager { get; set; } = new UserGroupManager();

        [ValidationAspect(typeof(CreateUserGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUserGroupsDto>> CreateAsync(CreateUserGroupsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.UserGroupsRepository, input.Code,L);

                var entity = ObjectMapper.Map<CreateUserGroupsDto, UserGroups>(input);

                var addedEntity = await _uow.UserGroupsRepository.InsertAsync(entity);
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "UserGroups", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectUserGroupsDto>(ObjectMapper.Map<UserGroups, SelectUserGroupsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.UserGroupsRepository, id,L);
                await _uow.UserGroupsRepository.DeleteAsync(id);
                var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "UserGroups", LogType.Delete, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessResult(L["DeleteSuccessMessage"]);
            }
        }

        public async Task<IDataResult<SelectUserGroupsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.UserGroupsRepository.GetAsync(t => t.Id == id, t => t.Users);
                var mappedEntity = ObjectMapper.Map<UserGroups, SelectUserGroupsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "UserGroups", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
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

                await _manager.UpdateControl(_uow.UserGroupsRepository, input.Code, input.Id, entity,L);

                var mappedEntity = ObjectMapper.Map<UpdateUserGroupsDto, UserGroups>(input);

                await _uow.UserGroupsRepository.UpdateAsync(mappedEntity);
                var before = ObjectMapper.Map<UserGroups, UpdateUserGroupsDto>(entity);
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "UserGroups", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
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
