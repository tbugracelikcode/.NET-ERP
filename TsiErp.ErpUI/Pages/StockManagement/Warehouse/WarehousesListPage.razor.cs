using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;

namespace TsiErp.ErpUI.Pages.StockManagement.Warehouse
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
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("WarehousesChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }


        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("WarehousesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

    }
}
