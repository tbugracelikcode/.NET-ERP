using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.EmployeeSeniority.Services;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.Department
{
    public partial class DepartmentsListPage : IDisposable
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
            EmployeeSenioritiesList = (await EmployeeSenioritiesAppService.GetListAsync(new ListEmployeeSenioritiesParameterDto())).Data.ToList();
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
            var selectedEmployeeSeniorities = args.RowData;

            if (selectedEmployeeSeniorities != null)
            {
                DataSource.SeniorityID = selectedEmployeeSeniorities.Id;
                DataSource.SeniorityName = selectedEmployeeSeniorities.Name;
                SelectEmployeeSenioritiesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
