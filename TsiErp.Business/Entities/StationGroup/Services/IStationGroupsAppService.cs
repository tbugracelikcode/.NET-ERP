using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Services.BusinessCoreServices;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.Business.Entities.StationGroup.Services
{
    public interface IStationGroupsAppService : ICrudAppService<SelectStationGroupsDto, ListStationGroupsDto, CreateStationGroupsDto, UpdateStationGroupsDto, ListStationGroupsParameterDto>
    {
    }
}
