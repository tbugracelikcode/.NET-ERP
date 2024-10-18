using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.LeanProduction.GeneralOEE.Dtos;
using TsiErp.Entities.Entities.LeanProduction.OEEDetail.Dtos;

namespace TsiErp.Business.Entities.LeanProduction.OEEDetail.Services
{
    public interface IOEEDetailsAppService : ICrudAppService<SelectOEEDetailsDto, ListOEEDetailsDto, CreateOEEDetailsDto, UpdateOEEDetailsDto, ListOEEDetailsParameterDto>
    {
        Task<IDataResult<SelectOEEDetailsDto>> GetbyDateStationEmployeeWorkOrderAsync(DateTime date, Guid stationID, Guid employeeID, Guid workOrderID);

        Task<IDataResult<IList<ListOEEDetailsDto>>> GetListbyDateAsync(DateTime date);
        Task<IDataResult<IList<ListOEEDetailsDto>>> GetListbyStartEndDateAsync(DateTime startDate, DateTime endDate);
    }
}
