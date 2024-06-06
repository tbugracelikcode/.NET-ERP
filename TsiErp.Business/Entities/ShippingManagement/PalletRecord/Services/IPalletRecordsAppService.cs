using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;

namespace TsiErp.Business.Entities.PalletRecord.Services
{
    public interface IPalletRecordsAppService : ICrudAppService<SelectPalletRecordsDto, ListPalletRecordsDto, CreatePalletRecordsDto, UpdatePalletRecordsDto, ListPalletRecordsParameterDto>
    {
        Task<List<SelectPalletRecordLinesDto>> GetPalletLines();
    }
}
