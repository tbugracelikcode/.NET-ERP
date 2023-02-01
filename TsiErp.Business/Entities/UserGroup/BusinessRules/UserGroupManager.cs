using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.UserGroup;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.UserGroup;

namespace TsiErp.Business.Entities.UserGroup.BusinessRules
{
    public class UserGroupManager
    {
        public async Task CodeControl(IUserGroupsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IUserGroupsRepository _repository, string code, Guid id, UserGroups entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IUserGroupsRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.Users.Any(x => x.GroupID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
