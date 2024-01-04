using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.StationGroup
{
    public partial class StationGroupsListPage : IDisposable
    {

        protected override async void OnInitialized()
        {
            BaseCrudService = StationGroupsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource= new SelectStationGroupsDto()
            { 
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("StationGroupChildMenu")
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["StationGroupContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["StationGroupContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["StationGroupContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["StationGroupContextRefresh"], Id = "refresh" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("StationGroupChildMenu");
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
