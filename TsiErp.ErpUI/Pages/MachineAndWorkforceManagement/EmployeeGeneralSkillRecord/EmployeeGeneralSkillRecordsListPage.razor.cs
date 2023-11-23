using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.EmployeeGeneralSkillRecord
{
    public partial class EmployeeGeneralSkillRecordsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeeGeneralSkillRecordsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEmployeeGeneralSkillRecordsDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeGeneralSkillRecordsChildMenu")
            };

            EditPageVisible = true;
            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeGeneralSkillRecordsContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeGeneralSkillRecordsContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeGeneralSkillRecordsContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeGeneralSkillRecordsContextRefresh"], Id = "refresh" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeeGeneralSkillRecordsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
