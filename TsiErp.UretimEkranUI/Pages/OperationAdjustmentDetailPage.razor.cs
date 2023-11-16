using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.StockManagement.ProductGroup.Dtos;
using TsiErp.UretimEkranUI.Services;
using TsiErp.UretimEkranUI.Utilities.ModalUtilities;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class OperationAdjustmentDetailPage
    {



        public string SettingUserWrittenPassword { get; set; }

        public bool SettingUserComponenetEnabled { get; set; } = true;


        public bool FinishAdjustmentButtonDisabled { get; set; } = true;


        [Inject]
        ModalManager ModalManager { get; set; }

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
            if (SettingUserComponenetEnabled)
            {
                SelectEmployeesPopupVisible = true;
                EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.Where(t => t.IsProductionScreenUser == true && t.IsProductionScreenSettingUser == true).ToList();
                await InvokeAsync(StateHasChanged);
            }

        }

        public async void EmployeesOnValueChange(ChangedEventArgs args)
        {
            if (SettingUserComponenetEnabled)
            {
                if (args.Value == null)
                {
                    AppService.CurrentOperation.OperationAdjustment.SettingUserId = Guid.Empty;
                    AppService.CurrentOperation.OperationAdjustment.SettingUserName = string.Empty;
                    AppService.CurrentOperation.OperationAdjustment.SettingUserPassword = string.Empty;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void EmployeesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeesDto> args)
        {
            var selectedEmployee = args.RowData;

            if (selectedEmployee != null)
            {
                AppService.CurrentOperation.OperationAdjustment.SettingUserId = selectedEmployee.Id;
                AppService.CurrentOperation.OperationAdjustment.SettingUserName = selectedEmployee.Name + " " + selectedEmployee.Surname;
                AppService.CurrentOperation.OperationAdjustment.SettingUserPassword = selectedEmployee.ProductionScreenPassword;
                SelectEmployeesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion


        #region Ayar Süresi Metotlar

        System.Timers.Timer AdjustmentTimer = new System.Timers.Timer(1);
        public bool StartAdjustmenButtonDisabled { get; set; } = false;

        public string AdjustmentStartTime { get; set; } = "00:00:00";

        private void SettingsTimedEvent(object source, ElapsedEventArgs e)
        {
            DateTime currentTime = e.SignalTime;

            AdjustmentStartTime = currentTime.Subtract(AppService.CurrentOperation.OperationAdjustment.AdjustmentStartDate).Hours.ToString("D2") +
                ":" + currentTime.Subtract(AppService.CurrentOperation.OperationAdjustment.AdjustmentStartDate).Minutes.ToString("D2") +
                ":" + currentTime.Subtract(AppService.CurrentOperation.OperationAdjustment.AdjustmentStartDate).Seconds.ToString("D2");

            InvokeAsync(StateHasChanged);
        }

        async void StartAdjustmentButtonClick()
        {


            if (AppService.CurrentOperation.OperationAdjustment.SettingUserId != Guid.Empty)
            {
                if (AppService.CurrentOperation.OperationAdjustment.SettingUserPassword == SettingUserWrittenPassword)
                {
                    SettingUserComponenetEnabled = false;
                    SendQualityControlApprovalButtonDisabled = false;
                    AppService.CurrentOperation.OperationAdjustment.AdjustmentStartDate = DateTime.Now;

                    AdjustmentTimer = new System.Timers.Timer(1);
                    AdjustmentTimer.Elapsed += SettingsTimedEvent;
                    AdjustmentTimer.AutoReset = true;
                    AdjustmentTimer.Enabled = true;
                    StartAdjustmenButtonDisabled = true;


                    //AdjustmentButtonText = "Ayara Başla";
                    //AppService.CurrentOperation.OperationAdjustment.AdjustmentEndDate = DateTime.Now;
                    //AdjustmentTimer.Elapsed -= OnTimedEvent;
                    //AdjustmentTimer.Enabled = false;

                }
                else
                {
                    await ModalManager.MessagePopupAsync("Bilgi", "Lütfen şifrenizi kontrol edin.");
                }
            }
            else
            {
                await ModalManager.MessagePopupAsync("Bilgi", "Lütfen ayarı yapacak personeli seçin.");
            }
        }

        #endregion


        #region Kalite Kontrol Onay Metotlar

        System.Timers.Timer QualityControlApprovalTimer = new System.Timers.Timer(1);

        public bool SendQualityControlApprovalButtonDisabled { get; set; } = true;

        public string QualityControlApprovalStartTime { get; set; } = "00:00:00";

        private void QualityControlApprovalTimedEvent(object source, ElapsedEventArgs e)
        {
            DateTime currentTime = e.SignalTime;

            QualityControlApprovalStartTime = currentTime.Subtract(AppService.CurrentOperation.OperationAdjustment.QualitControlApprovalStartDate).Hours.ToString("D2") +
                ":" + currentTime.Subtract(AppService.CurrentOperation.OperationAdjustment.QualitControlApprovalStartDate).Minutes.ToString("D2") +
                ":" + currentTime.Subtract(AppService.CurrentOperation.OperationAdjustment.QualitControlApprovalStartDate).Seconds.ToString("D2");

            InvokeAsync(StateHasChanged);
        }

        async void SendQualityControlButtonClick()
        {
            AdjustmentTimer.Stop();
            StartAdjustmenButtonDisabled = true;
            AppService.CurrentOperation.OperationAdjustment.QualitControlApprovalStartDate = DateTime.Now;
            QualityControlApprovalTimer = new System.Timers.Timer(1);
            QualityControlApprovalTimer.Elapsed += QualityControlApprovalTimedEvent;
            QualityControlApprovalTimer.AutoReset = true;
            QualityControlApprovalTimer.Enabled = true;


            //İlk ürün onay kaydı oluşturulacak.
        }
        #endregion



        async void FinishAdjustmentButtonClick()
        {

        }

    }
}
