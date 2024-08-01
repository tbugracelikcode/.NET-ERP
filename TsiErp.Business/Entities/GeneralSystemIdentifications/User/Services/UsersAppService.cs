using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
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
        private readonly IGetSQLDateAppService _GetSQLDateAppService;


        private readonly IUserPermissionsAppService _UserPermissionsAppService;

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public UsersAppService(IStringLocalizer<UsersResource> l, IFicheNumbersAppService ficheNumbersAppService, IUserPermissionsAppService userPermissionsAppService, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _UserPermissionsAppService = userPermissionsAppService;
            _GetSQLDateAppService = getSQLDateAppService;
        }

        [ValidationAspect(typeof(CreateUsersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUsersDto>> CreateAsync(CreateUsersDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Users).Select("*").Where(new { Code = input.Code }, false, false, "");

            var list = queryFactory.ControlList<Users>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();

            var query = queryFactory.Query().From(Tables.Users).Insert(new CreateUsersDto
            {
                Code = input.Code,
                Email = input.Email,
                GroupID = input.GroupID.GetValueOrDefault(),
                NameSurname = input.NameSurname,
                Password = input.Password,
                UserName = input.UserName,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
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

            await FicheNumbersAppService.UpdateFicheNumberAsync("UsersChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Users, LogType.Insert, addedEntityId);

            await _UserPermissionsAppService.AllPermissionsAddedUser(addedEntityId);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(users);

        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.Users).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

            var users = queryFactory.Update<SelectUsersDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Users, LogType.Delete, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(users);
        }

        public async Task<IDataResult<SelectUsersDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.Users).Select<Users>(null)
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

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(user);
        }

        public async Task<IDataResult<SelectUsersDto>> GetAsyncByUserNameAndPassword(string userName, string password)
        {
            var query = queryFactory.Query().From(Tables.Users).Select("*").Where(new { UserName = userName, Password = password }, true, true, Tables.Users);

            var user = queryFactory.Get<SelectUsersDto>(query);

            LogsAppService.InsertLogToDatabase(user, user, LoginedUserService.UserId, Tables.Users, LogType.Get, user.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(user);
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListUsersDto>>> GetListAsync(ListUsersParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.Users).Select<Users>(null)
                        .Join<UserGroups>
                        (
                            ug => new { GroupName = ug.Name },
                            nameof(Users.GroupID),
                            nameof(UserGroups.Id),
                            JoinType.Left
                        ).Where(null, true, true, Tables.Users);

            var users = queryFactory.GetList<ListUsersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListUsersDto>>(users);

        }


        [ValidationAspect(typeof(UpdateUsersValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectUsersDto>> UpdateAsync(UpdateUsersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Users).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<Users>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Users).Select("*").Where(new { Code = input.Code }, false, false, "");
            var list = queryFactory.GetList<Users>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.Users).Update(new UpdateUsersDto
            {
                Code = input.Code,
                Id = input.Id,
                Email = input.Email,
                GroupID = input.GroupID.GetValueOrDefault(),
                NameSurname = input.NameSurname,
                Password = input.Password,
                UserName = input.UserName,
                IsActive = input.IsActive,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, true, true, "");

            var users = queryFactory.Update<SelectUsersDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, users, LoginedUserService.UserId, Tables.Users, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(users);
        }

        public async Task<IDataResult<SelectUsersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
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

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, true, true, "");

            var users = queryFactory.Update<SelectUsersDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUsersDto>(users);

        }
    }
}
