using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
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
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("UnitSetsChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnitSetContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnitSetContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnitSetContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["UnitSetContextRefresh"], Id = "refresh" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("UnitSetsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

    }
}
