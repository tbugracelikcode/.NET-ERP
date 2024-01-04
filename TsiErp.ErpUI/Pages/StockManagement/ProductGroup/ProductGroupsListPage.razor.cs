using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;

namespace TsiErp.ErpUI.Pages.StockManagement.ProductGroup
{
    public partial class ProductGroupsListPage : IDisposable
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = ProductGroupsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectProductGroupsDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("ProductGroupsChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductGroupContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductGroupContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductGroupContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["ProductGroupContextRefresh"], Id = "refresh" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("ProductGroupsChildMenu");
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
