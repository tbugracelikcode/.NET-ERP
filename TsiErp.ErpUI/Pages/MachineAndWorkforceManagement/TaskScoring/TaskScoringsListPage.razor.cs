using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.TaskScoring.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.TaskScoring
{
    public partial class TaskScoringsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = TaskScoringsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectTaskScoringsDto()
            {
                Score = 0,
                Code = FicheNumbersAppService.GetFicheNumberAsync("TaskScoringsChildMenu")
            };

            EditPageVisible = true;
            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["TaskScoringsContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["TaskScoringsContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["TaskScoringsContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["TaskScoringsContextRefresh"], Id = "refresh" });
        }

        #region Switch Change Methodları

        private void IsTaskDoneChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsTaskDone)
            {
                DataSource.Score += 1;
            }
            else
            {
                DataSource.Score -= 1;
            }
        }

        private void IsDetectFaultChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsDetectFault)
            {
                DataSource.Score += 1;
            }
            else
            {
                DataSource.Score -= 1;
            }
        }
        private void IsAdjustmentChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsAdjustment)
            {
                DataSource.Score += 1;
            }
            else
            {
                DataSource.Score -= 1;
            }
        }
        private void IsDeveloperIdeaChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsDeveloperIdea)
            {
                DataSource.Score += 1;
            }
            else
            {
                DataSource.Score -= 1;
            }
        }
        private void IsTaskSharingChange(Syncfusion.Blazor.Buttons.ChangeEventArgs<bool> args)
        {
            if (DataSource.IsTaskSharing)
            {
                DataSource.Score += 1;
            }
            else
            {
                DataSource.Score -= 1;
            }
        }

        #endregion


        #region Kıdem ButtonEdit

        SfTextBox EmployeeSenioritiesButtonEdit;
        bool SelectEmployeeSenioritiesPopupVisible = false;
        List<ListEmployeeSenioritiesDto> EmployeeSenioritiesList = new List<ListEmployeeSenioritiesDto>();

        public async Task EmployeeSenioritiesOnCreateIcon()
        {
            var EmployeeSenioritiesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EmployeeSenioritiesButtonClickEvent);
            await EmployeeSenioritiesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EmployeeSenioritiesButtonClick } });
        }

        public async void EmployeeSenioritiesButtonClickEvent()
        {
            SelectEmployeeSenioritiesPopupVisible = true;
            EmployeeSenioritiesList = (await EmployeeSenioritiesService.GetListAsync(new ListEmployeeSenioritiesParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void EmployeeSenioritiesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.SeniorityID = Guid.Empty;
                DataSource.SeniorityName = string.Empty;
            }
        }

        public async void EmployeeSenioritiesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeeSenioritiesDto> args)
        {
            var selectedEmployeeSenioritie = args.RowData;

            if (selectedEmployeeSenioritie != null)
            {
                DataSource.SeniorityID = selectedEmployeeSenioritie.Id;
                DataSource.SeniorityName = selectedEmployeeSenioritie.Name;
                SelectEmployeeSenioritiesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion


        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("TaskScoringsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
