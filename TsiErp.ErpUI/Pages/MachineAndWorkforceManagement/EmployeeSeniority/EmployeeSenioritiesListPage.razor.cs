using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.EmployeeSeniority
{
    public partial class EmployeeSenioritiesListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeeSenioritiesService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEmployeeSenioritiesDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeSenioritiesChildMenu")
            };

            EditPageVisible = true;
            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeSenioritiesContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeSenioritiesContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeSenioritiesContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeSenioritiesContextRefresh"], Id = "refresh" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeSenioritiesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
