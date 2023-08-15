using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.SalesManagementParameter.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.User;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.SalesManagementParameter.Page;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services
{
    [ServiceRegistration(typeof(IUserPermissionsAppService), DependencyInjectionType.Scoped)]
    public class UserPermissionsAppService : ApplicationService<SalesManagementParametersResource>, IUserPermissionsAppService
    {

        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public UserPermissionsAppService(IStringLocalizer<SalesManagementParametersResource> l) : base(l)
        {
        }

        public async Task<IDataResult<SelectUserPermissionsDto>> CreateAsync(CreateUserPermissionsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
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

                return new SuccessDataResult<SelectUserPermissionsDto>();
            }
        }

        public async Task<IDataResult<IList<SelectUserPermissionsDto>>> GetListAsyncByUserId(Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.UserPermissions).Select("*")
                    .Join<Users>(u => new { UserName = u.UserName }, nameof(UserPermissions.UserId), nameof(Users.Id), JoinType.Left)
                    .Join<Menus>(u => new { MenuName = u.MenuName }, nameof(UserPermissions.MenuId), nameof(Menus.Id), JoinType.Left)
                    .Where("UserId", "=", userId, "").UseIsDelete(false);

                var permissions = queryFactory.GetList<SelectUserPermissionsDto>(query).ToList();

                return new SuccessDataResult<IList<SelectUserPermissionsDto>>(permissions);
            }
        }

        public async Task<IDataResult<SelectUserPermissionsDto>> UpdateAsync(UpdateUserPermissionsDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                foreach (var item in input.SelectUserPermissionsList)
                {
                    var query = queryFactory.Query().From(Tables.UserPermissions).Update(new UpdateUserPermissionsDto
                    {
                        Id = item.Id,
                        IsUserPermitted = item.IsUserPermitted,
                        MenuId = item.MenuId,
                        UserId = item.UserId
                    }).UseIsDelete(false);

                    var updatedPermissin = queryFactory.Update<SelectUserPermissionsDto>(query, "Id", true);
                }

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.UserPermissions, LogType.Update, Guid.Empty);

                return new SuccessDataResult<SelectUserPermissionsDto>();
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
