using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
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

        private SfGrid<ListEmployeesDto> _grid;

        List<ComboBoxEnumItem<BloodTypeEnum>> BloodTypesList = new List<ComboBoxEnumItem<BloodTypeEnum>>();


        protected override async void OnInitialized()
        {
            BaseCrudService = EmployeesService;
            await GetDepartmentsList();
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEmployeesDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(200, 50);
        }

        #region Departmanlar
        public async Task DepartmentFiltering(FilteringEventArgs args)
        {

            args.PreventDefaultAction = true;

            var pre = new WhereFilter();
            var predicate = new List<WhereFilter>();
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Code", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            predicate.Add(new WhereFilter() { Condition = "or", Field = "Name", value = args.Text, Operator = "contains", IgnoreAccent = true, IgnoreCase = true });
            pre = WhereFilter.Or(predicate);

            var query = new Query();
            query = args.Text == "" ? new Query() : new Query().Where(pre);

            await DepartmentsComboBox.FilterAsync(DepartmentsList, query);
        }

        private async Task GetDepartmentsList()
        {
            DepartmentsList = (await DepartmentsAppService.GetListAsync(new ListDepartmentsParameterDto())).Data.ToList();
        }

        public async Task DepartmentValueChangeHandler(ChangeEventArgs<string, ListDepartmentsDto> args)
        {
            if (args.ItemData != null)
            {
                DataSource.DepartmentID = args.ItemData.Id;
                DataSource.Department = args.ItemData.Name;
            }
            else
            {
                DataSource.DepartmentID = Guid.Empty;
                DataSource.Department = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
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
