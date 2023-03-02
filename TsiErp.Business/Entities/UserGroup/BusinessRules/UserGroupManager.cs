using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UserGroup;
using TsiErp.Entities.Entities.UserGroup;
using TsiErp.Localizations.Resources.UserGroups.Page;

namespace TsiErp.Business.Entities.UserGroup.BusinessRules
{
    public class UserGroupManager
    {
        public async Task CodeControl(IUserGroupsRepository _repository, string code, IStringLocalizer<UserGroupsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IUserGroupsRepository _repository, string code, Guid id, UserGroups entity, IStringLocalizer<UserGroupsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IUserGroupsRepository _repository, Guid id, IStringLocalizer<UserGroupsResource> L)
        {
            if (await _repository.AnyAsync(t => t.Users.Any(x => x.GroupID == id)))
            {
                throw new Exception(L["DeleteControlManager"]);
            }
        }
    }
}
