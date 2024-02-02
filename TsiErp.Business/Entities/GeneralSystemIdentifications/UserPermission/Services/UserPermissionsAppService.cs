using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Menu.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.SalesManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services
{
    [ServiceRegistration(typeof(IUserPermissionsAppService), DependencyInjectionType.Scoped)]
    public class UserPermissionsAppService : ApplicationService<SalesManagementParametersResource>, IUserPermissionsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private readonly IMenusAppService _MenusAppService;
        public UserPermissionsAppService(IStringLocalizer<SalesManagementParametersResource> l, IMenusAppService menusAppService) : base(l)
        {
            _MenusAppService = menusAppService;
        }

        public async Task<IDataResult<SelectUserPermissionsDto>> CreateAsync(CreateUserPermissionsDto input)
        {
            foreach (var item in input.SelectUserPermissionsList)
            {
                var query = queryFactory.Query().From(Tables.UserPermissions).Insert(new CreateUserPermissionsDto
                {
                    Id = GuidGenerator.CreateGuid(),
                    IsUserPermitted = item.IsUserPermitted,
                    MenuId = item.MenuId,
                    UserId = item.UserId
                }).UseIsDelete(false);

                var insertedPermissin = queryFactory.Insert<SelectUserPermissionsDto>(query, "Id", true);
            }

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UserPermissions, LogType.Insert, Guid.Empty);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUserPermissionsDto>();
        }

        public async Task<IDataResult<IList<SelectUserPermissionsDto>>> GetListAsyncByUserId(Guid userId)
        {
            var query = queryFactory.Query().From(Tables.UserPermissions).Select<UserPermissions>(null)
                .Join<Users>(u => new { UserName = u.UserName }, nameof(UserPermissions.UserId), nameof(Users.Id), JoinType.Left)
                .Join<Menus>(u => new { MenuName = u.MenuName }, nameof(UserPermissions.MenuId), nameof(Menus.Id), JoinType.Left)
                .Where("UserId", "=", userId, "").UseIsDelete(false);

            var permissions = queryFactory.GetList<SelectUserPermissionsDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<SelectUserPermissionsDto>>(permissions);
        }

        public async Task<IDataResult<SelectUserPermissionsDto>> UpdateAsync(UpdateUserPermissionsDto input)
        {
            foreach (var item in input.SelectUserPermissionsList)
            {
                var query = queryFactory.Query().From(Tables.UserPermissions).Update(new UpdateUserPermissionsDto
                {
                    Id = item.Id,
                    IsUserPermitted = item.IsUserPermitted,
                    MenuId = item.MenuId,
                    UserId = item.UserId
                }).Where(new { Id = item.Id }, false, false, "").UseIsDelete(false);

                var updatedPermissin = queryFactory.Update<SelectUserPermissionsDto>(query, "Id", true);
            }

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UserPermissions, LogType.Update, Guid.Empty);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectUserPermissionsDto>();
        }

        public async Task AllPermissionsAddedUser(Guid userId)
        {
            var menusList = (await _MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();

            foreach(var menu in menusList)
            {
                var query = queryFactory.Query().From(Tables.UserPermissions).Insert(new CreateUserPermissionsDto
                {
                    Id = GuidGenerator.CreateGuid(),
                    IsUserPermitted = true,
                    MenuId = menu.Id,
                    UserId = userId
                }).UseIsDelete(false);

                var insertedPermission = queryFactory.Insert<SelectUserPermissionsDto>(query, "Id", true);
            }
        }






        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectUserPermissionsDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<IList<ListUserPermissionsDto>>> GetListAsync(ListUserPermissionsParameterDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectUserPermissionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

    }
}
