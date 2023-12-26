using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.Employee
{
    public partial class EmployeesListPage
    {

        [Inject]
        ModalManager ModalManager { get; set; }

        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeesService;
            _L = L;
        }

        #region Combobox İşlemleri

        public IEnumerable<SelectEmployeesDto> bloodtypes = GetEnumDisplayBloodTypesNames<BloodTypeEnum>();

        public static List<SelectEmployeesDto> GetEnumDisplayBloodTypesNames<T>()
        {
            var type = typeof(T);

            return Enum.GetValues(type)
                       .Cast<BloodTypeEnum>()
                       .Select(x => new SelectEmployeesDto
                       {
                           BloodType = x,
                           BloodTypeName = type.GetMember(x.ToString())
                       .First()
                       .GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString()

                       }).ToList();

        }

        #endregion

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEmployeesDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeesChildMenu"),
                IsProductionScreenUser = false,
                IsProductionScreenSettingUser = false,
                HiringDate = DateTime.Today,
            };

            foreach (var item in bloodtypes)
            {
                item.BloodTypeName = L[item.BloodTypeName];
            }

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeContextAdd"], Id = "new" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeContextChange"], Id = "changed" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeContextDelete"], Id = "delete" });
            GridContextMenu.Add(new ContextMenuItemModel { Text = L["EmployeeContextRefresh"], Id = "refresh" });
        }

        public override async void ShowEditPage()
        {
            foreach (var item in bloodtypes)
            {
                item.BloodTypeName = L[item.BloodTypeName];
            }

            if (DataSource != null)
            {

                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
                {
                    EditPageVisible = false;
                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], L["MessagePopupInformationDescriptionBase"]);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        #region Departman ButtonEdit

        SfTextBox DepartmentButtonEdit;
        bool SelectDepartmentPopupVisible = false;
        List<ListDepartmentsDto> DepartmentList = new List<ListDepartmentsDto>();

        public async Task DepartmentOnCreateIcon()
        {
            var DepartmentButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, DepartmentButtonClickEvent);
            await DepartmentButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", DepartmentButtonClick } });
        }

        public async void DepartmentButtonClickEvent()
        {
            SelectDepartmentPopupVisible = true;
            DepartmentList = (await DepartmentsAppService.GetListAsync(new ListDepartmentsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void DepartmentOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.DepartmentID = Guid.Empty;
                DataSource.Department = string.Empty;
            }
        }

        public async void DepartmentDoubleClickHandler(RecordDoubleClickEventArgs<ListDepartmentsDto> args)
        {
            var selectedDepartment = args.RowData;

            if (selectedDepartment != null)
            {
                DataSource.DepartmentID = selectedDepartment.Id;
                DataSource.Department = selectedDepartment.Name;
                SelectDepartmentPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Eğitim Seviyesi ButtonEdit

        SfTextBox EducationLevelScoresButtonEdit;
        bool SelectEducationLevelScoresPopupVisible = false;
        List<ListEducationLevelScoresDto> EducationLevelScoresList = new List<ListEducationLevelScoresDto>();

        public async Task EducationLevelScoresOnCreateIcon()
        {
            var EducationLevelScoresButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EducationLevelScoresButtonClickEvent);
            await EducationLevelScoresButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EducationLevelScoresButtonClick } });
        }

        public async void EducationLevelScoresButtonClickEvent()
        {
            SelectEducationLevelScoresPopupVisible = true;
            EducationLevelScoresList = (await EducationLevelScoresAppService.GetListAsync(new ListEducationLevelScoresParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void EducationLevelScoresOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.EducationLevelID = Guid.Empty;
                DataSource.EducationLevelName = string.Empty;
            }
        }

        public async void EducationLevelScoresDoubleClickHandler(RecordDoubleClickEventArgs<ListEducationLevelScoresDto> args)
        {
            var selectedEducationLevelScores = args.RowData;

            if (selectedEducationLevelScores != null)
            {
                DataSource.EducationLevelID = selectedEducationLevelScores.Id;
                DataSource.EducationLevelName = selectedEducationLevelScores.Name;
                SelectEducationLevelScoresPopupVisible = false;
                await InvokeAsync(StateHasChanged);
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




        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("EmployeesChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
