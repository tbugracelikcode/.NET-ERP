using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.UserGroup;
using TsiErp.Entities.Entities.UserGroup.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.EntityContracts.UserGroup;
using TsiErp.Localizations.Resources.UserGroups.Page;

namespace TsiErp.Business.Entities.UserGroup.Services
{
    [ServiceRegistration(typeof(IUserGroupsAppService), DependencyInjectionType.Scoped)]
    public class UserGroupsAppService : ApplicationService<UserGroupsResource>, IUserGroupsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public UserGroupsAppService(IStringLocalizer<UserGroupsResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateUserGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUserGroupsDto>> CreateAsync(CreateUserGroupsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.UserGroups).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<UserGroups>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.UserGroups).Insert(new CreateUserGroupsDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    IsActive = true,
                    Id = addedEntityId,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    IsDeleted = false
                });


                var userGroups = queryFactory.Insert<SelectUserGroupsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UserGroups, LogType.Insert, addedEntityId);


                return new SuccessDataResult<SelectUserGroupsDto>(userGroups);
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.UserGroups).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var userGroups = queryFactory.Update<SelectUserGroupsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.UserGroups, LogType.Delete, id);

                return new SuccessDataResult<SelectUserGroupsDto>(userGroups);
            }
        }

        public async Task<IDataResult<SelectUserGroupsDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory.Query().From(Tables.UserGroups).Select("*").Where(
                new
                {
                    Id = id
                }, true, true, "");
                var userGroup = queryFactory.Get<SelectUserGroupsDto>(query);


                LogsAppService.InsertLogToDatabase(userGroup, userGroup, LoginedUserService.UserId, Tables.UserGroups, LogType.Get, id);

                return new SuccessDataResult<SelectUserGroupsDto>(userGroup);

            }

        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUserGroupsDto>>> GetListAsync(ListUserGroupsParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.UserGroups).Select("*").Where(null, true, true, "");
                var userGroups = queryFactory.GetList<ListUserGroupsDto>(query).ToList();
                return new SuccessDataResult<IList<ListUserGroupsDto>>(userGroups);
            }

        }


        [ValidationAspect(typeof(UpdateUserGroupsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUserGroupsDto>> UpdateAsync(UpdateUserGroupsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.UserGroups).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<UserGroups>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.UserGroups).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<UserGroups>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.UserGroups).Update(new UpdateUserGroupsDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    Id = input.Id,
                    IsActive = input.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, true, true, "");

                var userGroups = queryFactory.Update<SelectUserGroupsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(entity, userGroups, LoginedUserService.UserId, Tables.UserGroups, LogType.Update, entity.Id);

                return new SuccessDataResult<SelectUserGroupsDto>(userGroups);
            }

        }

        public async Task<IDataResult<SelectUserGroupsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.UserGroups).Select("*").Where(new { Id = id }, true, true, "");

                var entity = queryFactory.Get<UserGroups>(entityQuery);

                var query = queryFactory.Query().From(Tables.UserGroups).Update(new UpdateUserGroupsDto
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    IsActive = entity.IsActive,
                    CreationTime = entity.CreationTime.Value,
                    CreatorId = entity.CreatorId.Value,
                    DeleterId = entity.DeleterId.GetValueOrDefault(),
                    DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                    LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                    Id = id,
                    DataOpenStatus = lockRow,
                    DataOpenStatusUserId = userId

                }).Where(new { Id = id }, true, true, "");

                var userGroups = queryFactory.Update<SelectUserGroupsDto>(query, "Id", true);
                return new SuccessDataResult<SelectUserGroupsDto>(userGroups);

            }

        }
    }
}
