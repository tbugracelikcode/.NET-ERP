using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Station.Dtos;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.StationGroup;
using TsiErp.Entities.Entities.StationGroup.Dtos;

namespace TsiErp.Business.Entities.StationGroup.Services
{
    public interface IStationGroupsAppService : ICrudAppService<StationGroups, SelectStationGroupsDto, ListStationGroupsDto, CreateStationGroupsDto, UpdateStationGroupsDto>
    {
    }
}
