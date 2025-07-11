﻿using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationOccupancy.Dtos;

namespace TsiErp.Business.Entities.MachineAndWorkforceManagement.StationOccupancy
{
    public interface IStationOccupanciesAppService : ICrudAppService<SelectStationOccupanciesDto, ListStationOccupanciesDto, CreateStationOccupanciesDto, UpdateStationOccupanciesDto, ListStationOccupanciesParameterDto>
    {
        Task<IDataResult<SelectStationOccupanciesDto>> GetbyStationAsync(Guid stationID);
    }
}
