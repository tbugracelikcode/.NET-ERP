using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;

namespace TsiErp.Business.Entities.PalletRecord.Services
{
    public interface IPalletRecordsAppService : ICrudAppService<SelectPalletRecordsDto, ListPalletRecordsDto, CreatePalletRecordsDto, UpdatePalletRecordsDto, ListPalletRecordsParameterDto>
    {
    }
}
