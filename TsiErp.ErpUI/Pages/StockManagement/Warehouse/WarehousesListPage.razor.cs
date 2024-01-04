using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.StockManagement.WareHouse.Dtos;

namespace TsiErp.ErpUI.Pages.StockManagement.Warehouse
{
    public partial class WarehousesListPage : IDisposable
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

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WarehouseContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WarehouseContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WarehouseContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["WarehouseContextRefresh"], Id = "refresh" });
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


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
