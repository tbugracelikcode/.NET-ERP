using Microsoft.Extensions.Localization;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.User;
using TsiErp.Entities.Entities.User;
using TsiErp.Localizations.Resources.Users.Page;

namespace TsiErp.Business.Entities.User.BusinessRules
{
    public class UserManager
    {
        public async Task CodeControl(IUsersRepository _repository, string code, IStringLocalizer<UsersResource> L)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }
        }

        public async Task UpdateControl(IUsersRepository _repository, string code, Guid id, Users entity, IStringLocalizer<UsersResource> L)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }
        }

        public async Task DeleteControl(IUsersRepository _repository, Guid id)
        {
          
        }
    }
}
