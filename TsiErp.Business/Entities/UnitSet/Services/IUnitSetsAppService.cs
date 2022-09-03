using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.UnitSet.Dtos;

namespace TsiErp.Business.Entities.UnitSet.Services
{
    public interface IUnitSetsAppService : ICrudAppService<UnitSets, SelectUnitSetsDto, ListUnitSetsDto, CreateUnitSetsDto, UpdateUnitSetsDto>
    {
    }
}
