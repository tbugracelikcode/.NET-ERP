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
using TsiErp.Business.Entities.Employee.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
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
                    AppService.CurrentOperation.OperationAdjustment.AdjustmentUserId = Guid.Empty;
                    AppService.CurrentOperation.OperationAdjustment.AdjustmentUserName = string.Empty;
                    AppService.CurrentOperation.OperationAdjustment.AdjustmentUserPassword = string.Empty;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void EmployeesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeesDto> args)
        {
            var selectedEmployee = args.RowData;

            if (selectedEmployee != null)
            {
                AppService.CurrentOperation.OperationAdjustment.AdjustmentUserId = selectedEmployee.Id;
                AppService.CurrentOperation.OperationAdjustment.AdjustmentUserName = selectedEmployee.Name + " " + selectedEmployee.Surname;
                AppService.CurrentOperation.OperationAdjustment.AdjustmentUserPassword = selectedEmployee.ProductionScreenPassword;
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


            if (AppService.CurrentOperation.OperationAdjustment.AdjustmentUserId != Guid.Empty)
            {
                if (AppService.CurrentOperation.OperationAdjustment.AdjustmentUserPassword == SettingUserWrittenPassword)
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
        public string FirstApprovalCode { get; set; }
        public bool FirstApprovalCodeVisible { get; set; } = false;

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


            #region İlk Ürün Kaydı Oluşturuluyor
            Guid OperationQualityPlanID = (await OperationalQualityPlansAppService.GetListAsync(new ListOperationalQualityPlansParameterDto())).Data.Where(t => t.ProductID == AppService.CurrentOperation.WorkOrder.ProductID && t.ProductsOperationID == AppService.CurrentOperation.WorkOrder.ProductsOperationID).Select(t => t.Id).FirstOrDefault();

            if (OperationQualityPlanID != Guid.Empty)
            {
                var OperationalQualityPlanDataSource = (await OperationalQualityPlansAppService.GetAsync(OperationQualityPlanID)).Data;
                var OperationalQualityPlanLineList = OperationalQualityPlanDataSource.SelectOperationalQualityPlanLines.Where(t => t.PeriodicControlMeasure == true).ToList();
                var OperationPictureDataSource = OperationalQualityPlanDataSource.SelectOperationPictures.LastOrDefault();

                var createdFirstProductApproval = new CreateFirstProductApprovalsDto()
                {
                    Code = FicheNumbersAppService.GetFicheNumberAsync("FirstProductApprovalChildMenu"),
                    WorkOrderID = AppService.CurrentOperation.WorkOrder.Id,
                    CreatorId = AppService.CurrentOperation.OperationAdjustment.AdjustmentUserId,
                    Description_ = string.Empty,
                    EmployeeID = Guid.Empty,
                    ControlDate = null,
                    ProductID = AppService.CurrentOperation.WorkOrder.ProductID,
                    OperationQualityPlanID = OperationQualityPlanID,
                    SelectFirstProductApprovalLines = new List<SelectFirstProductApprovalLinesDto>(),
                    IsApproval = false,
                    AdjustmentUserID = AppService.CurrentOperation.OperationAdjustment.AdjustmentUserId
                };

                foreach (var qualityplanline in OperationalQualityPlanLineList)
                {
                    SelectFirstProductApprovalLinesDto firstProductApprovalLineModel = new SelectFirstProductApprovalLinesDto
                    {
                        BottomTolerance = qualityplanline.BottomTolerance,
                        Description_ = string.Empty,
                        IdealMeasure = qualityplanline.IdealMeasure,
                        IsCriticalMeasurement = qualityplanline.PeriodicControlMeasure,
                        LineNr = createdFirstProductApproval.SelectFirstProductApprovalLines.Count + 1,
                        UpperTolerance = qualityplanline.UpperTolerance,
                        MeasurementValue = string.Empty,
                        CreatorId = AppService.CurrentOperation.OperationAdjustment.AdjustmentUserId
                    };

                    createdFirstProductApproval.SelectFirstProductApprovalLines.Add(firstProductApprovalLineModel);
                }

                var selectFirstProductApproval = (await FirstProductApprovalsAppService.CreateAsync(createdFirstProductApproval)).Data;

                if (selectFirstProductApproval.Id != Guid.Empty)
                {
                    SendQualityControlApprovalButtonDisabled = true;
                    FirstApprovalCode = selectFirstProductApproval.Code;
                    FirstApprovalCodeVisible = true;
                    await (InvokeAsync(StateHasChanged));
                }
                #endregion
            }


        }
        #endregion



        async void FinishAdjustmentButtonClick()
        {

        }

    }
}
