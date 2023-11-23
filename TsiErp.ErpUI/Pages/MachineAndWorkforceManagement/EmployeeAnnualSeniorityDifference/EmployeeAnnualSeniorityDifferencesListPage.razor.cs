using Microsoft.Extensions.Localization;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference.Dtos;
using Syncfusion.Blazor.Grids;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Calendars;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.EmployeeAnnualSeniorityDifference
{
    public partial class EmployeeAnnualSeniorityDifferencesListPage
    {

        DateTime dateYear = DateTime.Today;
        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeeAnnualSeniorityDifferencesService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEmployeeAnnualSeniorityDifferencesDto()
            {
                Year_ = DateTime.Today.Year
        };

            dateYear = DateTime.Today;

            EditPageVisible = true;
            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeAnnualSeniorityDifferencesContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeAnnualSeniorityDifferencesContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeAnnualSeniorityDifferencesContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeAnnualSeniorityDifferencesContextRefresh"], Id = "refresh" });
        }

        public void DateValueChangeHandler(ChangedEventArgs<DateTime> args)
        {
            DataSource.Year_ = dateYear.Year;
        }

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
    }
}
