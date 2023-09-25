using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.Department
{
    public partial class DepartmentsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = DepartmentsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectDepartmentsDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("DepartmentsChildMenu")
            };

            EditPageVisible = true;
            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["DepartmentContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["DepartmentContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["DepartmentContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["DepartmentContextRefresh"], Id = "refresh" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("DepartmentsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
