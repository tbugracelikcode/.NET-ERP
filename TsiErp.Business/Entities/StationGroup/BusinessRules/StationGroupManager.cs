using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.StationGroup;
using TsiErp.Entities.Entities.Branch;
using TsiErp.Entities.Entities.StationGroup;

namespace TsiErp.Business.Entities.StationGroup.BusinessRules
{
    public class StationGroupManager
    {
        public async Task CodeControl(IStationGroupsRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IStationGroupsRepository _repository, string code, Guid id, StationGroups entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IStationGroupsRepository _repository, Guid id)
        {
            if (await _repository.AnyAsync(t => t.Stations.Any(x => x.GroupID == id)))
            {
                throw new Exception("Hareket gören kayıtlar silinemez.");
            }
        }
    }
}
