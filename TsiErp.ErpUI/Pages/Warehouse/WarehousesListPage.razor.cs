using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;

namespace TsiErp.ErpUI.Pages.Warehouse
{
    public partial class WarehousesListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = WarehousesService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectWarehousesDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }


    }
}
