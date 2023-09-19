using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch.Dtos;

namespace TsiErp.ErpUI.Pages.GeneralSystemIdentifications.Branch
{
    public partial class BranchesListPage
    {
        protected override void OnInitialized()
        {
            BaseCrudService = BranchesService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectBranchesDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("BranchesChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["BranchesContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["BranchesContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["BranchesContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["BranchesContextRefresh"], Id = "refresh" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("BranchesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}