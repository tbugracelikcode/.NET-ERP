using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.UretimEkranUI.Services;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class OperationAdjustmentDetailPage
    {
        public bool StartAdjustmentButtonDisabled { get; set; } = true;

        #region Ayar Yapan Kullanıcı ButtonEdit

        SfTextBox EmployeesButtonEdit;
        bool SelectEmployeesPopupVisible = false;
        List<ListEmployeesDto> EmployeesList = new List<ListEmployeesDto>();

        public async Task EmployeesOnCreateIcon()
        {
            var EmployeesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, EmployeesButtonClickEvent);
            await EmployeesButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", EmployeesButtonClick } });
        }

        public async void EmployeesButtonClickEvent()
        {
            SelectEmployeesPopupVisible = true;
            EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.Where(t=>t.IsProductionScreenUser==true && t.IsProductionScreenSettingUser==true).ToList();
            await InvokeAsync(StateHasChanged);
        }

        public async void EmployeesOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                AppService.CurrentOperation.OperationAdjustment.SettingUserId = Guid.Empty;
                AppService.CurrentOperation.OperationAdjustment.SettingUserName = string.Empty;
                StartAdjustmentButtonDisabled = true;
                await InvokeAsync(StateHasChanged);
            }
        }

        public async void EmployeesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeesDto> args)
        {
            var selectedEmployee = args.RowData;

            if (selectedEmployee != null)
            {
                AppService.CurrentOperation.OperationAdjustment.SettingUserId = selectedEmployee.Id;
                AppService.CurrentOperation.OperationAdjustment.SettingUserName = selectedEmployee.Name + " " + selectedEmployee.Surname;
                StartAdjustmentButtonDisabled = false;
                SelectEmployeesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

    }
}
