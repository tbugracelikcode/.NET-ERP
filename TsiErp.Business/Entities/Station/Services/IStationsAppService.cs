using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using TsiErp.Entities.Entities.Period.Dtos;
using TsiErp.Entities.Entities.Period;
using TsiErp.Entities.Entities.Station;
using TsiErp.Entities.Entities.Station.Dtos;

namespace TsiErp.Business.Entities.Station.Services
{
    public interface IStationsAppService : ICrudAppService<SelectStationsDto, ListStationsDto, CreateStationsDto, UpdateStationsDto, ListStationsParameterDto>
    {
    }
}
