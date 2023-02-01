using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.StationInventory.Dtos;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Utilities.Results;

namespace TsiErp.Business.Entities.StationInventory.Services
{
    public interface IStationInventoriesAppService : ICrudAppService<SelectStationInventoriesDto, ListStationInventoriesDto, CreateStationInventoriesDto, UpdateStationInventoriesDto, ListStationInventoriesParameterDto>

    {
    }
}
