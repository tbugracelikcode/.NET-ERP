using TsiErp.Business.BusinessCoreServices;
using TsiErp.Entities.Entities.StockManagement.ProductReceiptTransaction.Dtos;

namespace TsiErp.Business.Entities.ProductReceiptTransaction.Services
{
    public interface IProductReceiptTransactionsAppService : ICrudAppService<SelectProductReceiptTransactionsDto, ListProductReceiptTransactionsDto, CreateProductReceiptTransactionsDto, UpdateProductReceiptTransactionsDto, ListProductReceiptTransactionsParameterDto>
    {
    }
}
