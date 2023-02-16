using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Department.Dtos;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;

namespace TsiErp.ErpUI.Pages.EquipmentRecord
{
    public partial class EquipmentRecordsListPage
    {
        bool cancelReasonVisible = false;
        

        protected override async void OnInitialized()
        {
            BaseCrudService = EquipmentRecordsService;
            await GetDepartmentsList();
        }

        void CancelValueChanged(ChangeEventArgs args)
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
                IsActive = true,
                CancellationDate = null,
                RecordDate = DateTime.Today
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
                DataSource.Department = Guid.Empty;
                DataSource.DepartmentName = string.Empty;
            }
        }

        public async void DepartmentDoubleClickHandler(RecordDoubleClickEventArgs<ListDepartmentsDto> args)
        {
            var selectedDepartment = args.RowData;

            if (selectedDepartment != null)
            {
                DataSource.Department = selectedDepartment.Id;
                DataSource.DepartmentName = selectedDepartment.Name;
                SelectDepartmentPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        private async Task GetDepartmentsList()
        {
            DepartmentList = (await DepartmentsAppService.GetListAsync(new ListDepartmentsParameterDto())).Data.ToList();
        }

     

    }
}
