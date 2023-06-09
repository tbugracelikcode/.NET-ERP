using TsiErp.Entities.Entities.StockManagement.UnitSet.Dtos;

namespace TsiErp.ErpUI.Pages.StockManagement.UnitSet
{
    public partial class UnitSetsListPage
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = UnitSetsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUnitSetsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }


    }
}
