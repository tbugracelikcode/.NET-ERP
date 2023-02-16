using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.UnitSet;
using TsiErp.Entities.Entities.UnitSet.Dtos;

namespace TsiErp.Business.Entities.UnitSet.Services
{
    public interface IUnitSetsAppService : ICrudAppService<SelectUnitSetsDto, ListUnitSetsDto, CreateUnitSetsDto, UpdateUnitSetsDto, ListUnitSetsParameterDto>
    {
    }
}
