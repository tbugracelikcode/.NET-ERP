using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.SalesManagement.SalesInvoice.Dtos;

namespace TsiErp.Business.Entities.SalesInvoice.Services
{
    public interface ISalesInvoicesAppService : ICrudAppService<SelectSalesInvoiceDto, ListSalesInvoiceDto, CreateSalesInvoiceDto, UpdateSalesInvoiceDto, ListSalesInvoiceParameterDto>
    {
    }
}
