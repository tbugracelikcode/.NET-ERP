using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Grids;
using Microsoft.Extensions.Localization;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;

namespace TsiErp.ErpUI.Pages.ProductionManagement.HaltReason
{
    public partial class HaltReasonsListPage : IDisposable
    {
        protected override void OnInitialized()
        {
            BaseCrudService = HaltReasonsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectHaltReasonsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("HaltReasonsChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["HaltReasonContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["HaltReasonContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["HaltReasonContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["HaltReasonContextRefresh"], Id = "refresh" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("HaltReasonsChildMenu");
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
