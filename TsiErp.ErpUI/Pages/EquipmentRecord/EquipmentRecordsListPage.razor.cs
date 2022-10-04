using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;

namespace TsiErp.ErpUI.Pages.EquipmentRecord
{
    public partial class EquipmentRecordsListPage
    {
        bool cancelReasonVisible = false;

        private SfGrid<ListEquipmentRecordsDto> _grid;

        SfComboBox<string, ListDepartmentsDto> DepartmentsComboBox;
        List<ListDepartmentsDto> DepartmentsList = new List<ListDepartmentsDto>();

        protected override async void OnInitialized()
        {
            BaseCrudService = EquipmentRecordsService;
            await GetDepartmentsList();
        }

        public void ShowColumns()
        {
            this._grid.OpenColumnChooserAsync(1250, 50);
        }

        void CheckValueChanged(ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);

            if (argsValue)
            {
                DataSource.CancellationDate = DateTime.Today;
                cancelReasonVisible = true;
            }
            else
            {
                DataSource.CancellationDate = null;
                cancelReasonVisible = false;
            }
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEquipmentRecordsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }

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
                DataSource.Department = args.ItemData.Id;
                DataSource.DepartmentName = args.ItemData.Name;
            }
            else
            {
                DataSource.Department = Guid.Empty;
                DataSource.DepartmentName = string.Empty;
            }
            await InvokeAsync(StateHasChanged);
        }

    }
}
