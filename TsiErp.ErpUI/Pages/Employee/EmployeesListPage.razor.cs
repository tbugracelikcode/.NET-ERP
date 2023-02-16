using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.Employee.Dtos;
using TsiErp.Entities.Enums;
using TsiErp.ErpUI.Helpers;

namespace TsiErp.ErpUI.Pages.Employee
{
    public partial class EmployeesListPage
    {
        SfComboBox<string, ListDepartmentsDto> DepartmentsComboBox;

        List<ListDepartmentsDto> DepartmentsList = new List<ListDepartmentsDto>();

        List<ComboBoxEnumItem<BloodTypeEnum>> BloodTypesList = new List<ComboBoxEnumItem<BloodTypeEnum>>();

        public string[] Types { get; set; }

        public string[] EnumValues = Enum.GetNames(typeof(BloodTypeEnum));




        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeesService;
        }

       

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEmployeesDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
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


        #region Kan Grubu
        //public async Task BloodTypeFiltering(FilteringEventArgs args)
        //{

        //    args.PreventDefaultAction = true;

        //    var pre = new WhereFilter();
        //    var predicate = new List<WhereFilter>();
        //    //predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
        //    predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
        //    pre = WhereFilter.Or(predicate);

        //    var query = new Query();
        //    query = args.Text == "" ? new Query() : new Query().Where(pre);

        //    await BloodTypesComboBox.FilterAsync(BloodTypesList, query);
        //}

        //private async Task GetBloodTypesList()
        //{
        //    BloodTypesList = (await DepartmentsAppService.GetListAsync(new ListDepartmentsParameterDto())).Data.ToList();
        //}

        //public async Task BloodTypeOpened(PopupEventArgs args)
        //{
        //    if (DepartmentsList.Count == 0)
        //    {
        //        await GetEquipmentRecordsList();
        //    }
        //}

        //private void BloodTypeValueChanged(ChangeEventArgs<string, ListDepartmentsDto> args)
        //{
        //    DataSource.DepartmentID = args.ItemData.Id;
        //    DataSource.Department = args.ItemData.Name;
        //}
        #endregion


    }
}
