using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.EducationLevelScore
{
    public partial class EducationLevelScoresListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = EducationLevelScoresService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEducationLevelScoresDto()
            {
                Code = FicheNumbersAppService.GetFicheNumberAsync("EducationLevelScoresChildMenu")
            };

            EditPageVisible = true;
            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EducationLevelScoresContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EducationLevelScoresContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EducationLevelScoresContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EducationLevelScoresContextRefresh"], Id = "refresh" });
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
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EducationLevelScoresChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
