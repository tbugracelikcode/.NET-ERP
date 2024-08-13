using Tsi.Core.Utilities.Results;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;

namespace TsiErp.Business.Entities.PalletRecord.Services
{
    public interface IPalletRecordsAppService : ICrudAppService<SelectPalletRecordsDto, ListPalletRecordsDto, CreatePalletRecordsDto, UpdatePalletRecordsDto, ListPalletRecordsParameterDto>
    {
        Task<List<SelectPalletRecordLinesDto>> GetPalletLines();

        Task<IDataResult<SelectPalletRecordsDto>> UpdatePreparingAsync(UpdatePalletRecordsDto input);

        Task<IDataResult<SelectPalletRecordsDto>> UpdateCompletedAsync(UpdatePalletRecordsDto input);

        Task<IDataResult<SelectPalletRecordsDto>> UpdateApprovedAsync(UpdatePalletRecordsDto input);

        Task<IDataResult<SelectPalletRecordsDto>> UpdateTicketPendingAsync(UpdatePalletRecordsDto input);

        Task<IDataResult<SelectPalletRecordsDto>> UpdateTicketCompletedAsync(UpdatePalletRecordsDto input);

        Task<IDataResult<SelectPalletRecordsDto>> UpdatePalletDetailAsync(UpdatePalletRecordsDto input);
    }
}
