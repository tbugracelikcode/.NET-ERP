using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserGroup;
using TsiErp.Entities.TableConstant;
using TsiErp.EntityContracts.User;
using TsiErp.Localizations.Resources.Users.Page;

namespace TsiErp.Business.Entities.User.Services
{
    [ServiceRegistration(typeof(IUsersAppService), DependencyInjectionType.Scoped)]
    public class UsersAppService : ApplicationService<UsersResource>, IUsersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public UsersAppService(IStringLocalizer<UsersResource> l) : base(l)
        {
        }

        [ValidationAspect(typeof(CreateUsersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUsersDto>> CreateAsync(CreateUsersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Users).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<Users>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                Guid addedEntityId = GuidGenerator.CreateGuid();

                var query = queryFactory.Query().From(Tables.Users).Insert(new CreateUsersDto
                {
                    Code = input.Code,
                    Email = input.Email,
                    GroupID = input.GroupID,
                    NameSurname = input.NameSurname,
                    Password = input.Password,
                    UserName = input.UserName,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = addedEntityId,
                    IsActive = true,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                });

                var users = queryFactory.Insert<SelectUsersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Users, LogType.Insert, addedEntityId);

                return new SuccessDataResult<SelectUsersDto>(users);
            }

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Users).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var users = queryFactory.Update<SelectUsersDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Users, LogType.Delete, id);

                return new SuccessDataResult<SelectUsersDto>(users);
            }
        }

        public async Task<IDataResult<SelectUsersDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.Users).Select<Users>(u => new { u.GroupID, u.Id, u.IsActive, u.Code, u.DataOpenStatus, u.DataOpenStatusUserId, u.Email, u.NameSurname, u.Password, u.UserName })
                            .Join<UserGroups>
                            (
                                ug => new { GroupID = ug.Id, GroupName = ug.Name },
                                nameof(Users.GroupID),
                                nameof(UserGroups.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, true, true, Tables.Users);

                var user = queryFactory.Get<SelectUsersDto>(query);

                LogsAppService.InsertLogToDatabase(user, user, LoginedUserService.UserId, Tables.Users, LogType.Get, id);

                return new SuccessDataResult<SelectUsersDto>(user);

            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUsersDto>>> GetListAsync(ListUsersParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.Users).Select<Users>(u => new { u.GroupID, u.Id, u.IsActive, u.Code, u.DataOpenStatus, u.DataOpenStatusUserId, u.Email, u.NameSurname, u.Password, u.UserName })
                            .Join<UserGroups>
                            (
                                ug => new { GroupName = ug.Name },
                                nameof(Users.GroupID),
                                nameof(UserGroups.Id),
                                JoinType.Left
                            ).Where(null, true, true, Tables.Users);

                var users = queryFactory.GetList<ListUsersDto>(query).ToList();

                return new SuccessDataResult<IList<ListUsersDto>>(users);
            }

        }


        [ValidationAspect(typeof(UpdateUsersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUsersDto>> UpdateAsync(UpdateUsersDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Users).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<Users>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.Users).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<Users>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.Users).Update(new UpdateUsersDto
                {
                    Code = input.Code,
                    Id = input.Id,
                    Email = input.Email,
                    GroupID = input.GroupID,
                    NameSurname = input.NameSurname,
                    Password = input.Password,
                    UserName = input.UserName,
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

                var users = queryFactory.Update<SelectUsersDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, users, LoginedUserService.UserId, Tables.Users, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectUsersDto>(users);
            }

        }

        public async Task<IDataResult<SelectUsersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Users).Select("*").Where(new { Id = id }, true, true, "");
                var entity = queryFactory.Get<Users>(entityQuery);

                var query = queryFactory.Query().From(Tables.Users).Update(new UpdateUsersDto
                {
                    Code = entity.Code,
                    Email = entity.Email,
                    GroupID = entity.GroupID,
                    NameSurname = entity.NameSurname,
                    Password = entity.Password,
                    UserName = entity.UserName,
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
                    DataOpenStatusUserId = userId,

                }).Where(new { Id = id }, true, true, "");

                var users = queryFactory.Update<SelectUsersDto>(query, "Id", true);

                return new SuccessDataResult<SelectUsersDto>(users);

            }

        }
    }
}
