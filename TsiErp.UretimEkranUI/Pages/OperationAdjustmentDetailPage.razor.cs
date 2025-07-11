﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System.Timers;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.OperationAdjustment.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApproval.Dtos;
using TsiErp.Entities.Entities.QualityControl.FirstProductApprovalLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalQualityPlan.Dtos;
using TsiErp.UretimEkranUI.Models;
using TsiErp.UretimEkranUI.Services;
using TsiErp.UretimEkranUI.Utilities.EnumUtilities;
using TsiErp.UretimEkranUI.Utilities.ModalUtilities;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class OperationAdjustmentDetailPage : IDisposable
    {
        public string AdjustmentUserWrittenPassword { get; set; }

        public bool AdjustmentUserComponenetEnabled { get; set; } = true;

        public bool FinishAdjustmentButtonDisabled { get; set; } = true;

        OperationAdjustmentTable Adjustment = new OperationAdjustmentTable();

        [Inject]
        ModalManager ModalManager { get; set; }

        protected override async void OnInitialized()
        {

            await StationsAppService.UpdateStationWorkStateAsync(AppService.CurrentOperation.StationID, 3);

            #region Sistem Genel Durum Update
            var generalStatus = (await SystemGeneralStatusLocalDbService.GetListAsync()).FirstOrDefault();

            if (generalStatus != null)
            {
                generalStatus.GeneralStatus = 3;

                await SystemGeneralStatusLocalDbService.UpdateAsync(generalStatus);
            }
            #endregion

        }

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
            if (AdjustmentUserComponenetEnabled)
            {
                SelectEmployeesPopupVisible = true;
                EmployeesList = (await EmployeesAppService.GetListAsync(new ListEmployeesParameterDto())).Data.Where(t => t.IsProductionScreenUser == true && t.IsProductionScreenSettingUser == true).ToList();
                await InvokeAsync(StateHasChanged);
            }

        }

        public async void EmployeesOnValueChange(ChangedEventArgs args)
        {
            if (AdjustmentUserComponenetEnabled)
            {
                if (args.Value == null)
                {
                    Adjustment.AdjustmentUserID = Guid.Empty;
                    Adjustment.AdjustmentUserName = string.Empty;
                    Adjustment.AdjustmentUserPassword = string.Empty;
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        public async void EmployeesDoubleClickHandler(RecordDoubleClickEventArgs<ListEmployeesDto> args)
        {
            var selectedEmployee = args.RowData;

            if (selectedEmployee != null)
            {
                Adjustment.AdjustmentUserID = selectedEmployee.Id;
                Adjustment.AdjustmentUserName = selectedEmployee.Name + " " + selectedEmployee.Surname;
                Adjustment.AdjustmentUserPassword = selectedEmployee.ProductionScreenPassword;
                SelectEmployeesPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Ayar Süresi Metotlar

        System.Timers.Timer AdjustmentTimer;

        public int TotalAdjustmentTime { get; set; }
        public bool StartAdjustmenButtonDisabled { get; set; } = false;

        public string AdjustmentStartTime { get; set; }

        private void SettingsTimedEvent(object source, ElapsedEventArgs e)
        {
            TotalAdjustmentTime++;

            TimeSpan time = TimeSpan.FromSeconds(TotalAdjustmentTime);

            AdjustmentStartTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                time.Hours,
                time.Minutes,
                time.Seconds);

            InvokeAsync(StateHasChanged);
        }

        async void StartAdjustmentButtonClick()
        {
            if (Adjustment.AdjustmentUserID != Guid.Empty)
            {
                if (Adjustment.AdjustmentUserPassword == AdjustmentUserWrittenPassword)
                {
                    AdjustmentUserComponenetEnabled = false;
                    SendQualityControlApprovalButtonDisabled = false;
                    Adjustment.AdjustmentDate = GetSQLDateAppService.GetDateFromSQL();

                    AdjustmentTimer = new System.Timers.Timer(1000);
                    AdjustmentTimer.Elapsed += SettingsTimedEvent;
                    AdjustmentTimer.Enabled = true;
                    StartAdjustmenButtonDisabled = true;

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

        System.Timers.Timer QualityControlApprovalTimer;

        public int TotalQualityControlApprovalTime { get; set; }

        public bool SendQualityControlApprovalButtonDisabled { get; set; } = true;

        public string QualityControlApprovalStartTime { get; set; }

        public string FirstApprovalCode { get; set; }

        public string FirstApprovalStatus { get; set; } = "Onay Bekliyor";

        public Guid FirstApprovalId { get; set; }

        public bool FirstApprovalCodeVisible { get; set; } = false;

        private void QualityControlApprovalTimedEvent(object source, ElapsedEventArgs e)
        {
            TotalQualityControlApprovalTime++;

            TimeSpan time = TimeSpan.FromSeconds(TotalQualityControlApprovalTime);

            QualityControlApprovalStartTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                time.Hours,
                time.Minutes,
                time.Seconds);

            InvokeAsync(StateHasChanged);
        }

        async void SendQualityControlButtonClick()
        {
            AdjustmentTimer.Stop();
            AdjustmentTimer.Enabled = false;
            StartAdjustmenButtonDisabled = true;

            AppService.CurrentOperation.QualitControlApprovalDate = GetSQLDateAppService.GetDateFromSQL();
            QualityControlApprovalTimer = new System.Timers.Timer(1000);
            QualityControlApprovalTimer.Elapsed += QualityControlApprovalTimedEvent;
            QualityControlApprovalTimer.Enabled = true;

            #region İlk Ürün Kaydı Oluşturuluyor
            Guid OperationQualityPlanID = (await OperationalQualityPlansAppService.GetListAsync(new ListOperationalQualityPlansParameterDto())).Data.Where(t => t.ProductID == AppService.CurrentOperation.ProductID && t.ProductsOperationID == AppService.CurrentOperation.ProductsOperationID).Select(t => t.Id).FirstOrDefault();

            if (OperationQualityPlanID != Guid.Empty)
            {
                var OperationalQualityPlanDataSource = (await OperationalQualityPlansAppService.GetAsync(OperationQualityPlanID)).Data;
                var OperationalQualityPlanLineList = OperationalQualityPlanDataSource.SelectOperationalQualityPlanLines.Where(t => t.PeriodicControlMeasure == true).ToList();
                var OperationPictureDataSource = OperationalQualityPlanDataSource.SelectOperationPictures.LastOrDefault();

                var createdFirstProductApproval = new CreateFirstProductApprovalsDto()
                {
                    Code = FicheNumbersAppService.GetFicheNumberAsync("FirstProductApprovalChildMenu"),
                    WorkOrderID = AppService.CurrentOperation.WorkOrderID,
                    CreatorId = Adjustment.AdjustmentUserID,
                    Description_ = string.Empty,
                    EmployeeID = Guid.Empty,
                    ControlDate = GetSQLDateAppService.GetDateFromSQL(),
                    ProductID = AppService.CurrentOperation.ProductID,
                    OperationQualityPlanID = OperationQualityPlanID,
                    SelectFirstProductApprovalLines = new List<SelectFirstProductApprovalLinesDto>(),
                    IsApproval = false,
                    AdjustmentUserID = Adjustment.AdjustmentUserID,
                    ApprovedQuantity = 0,
                    ScrapQuantity = 0,
                    ProductionOrderID = AppService.CurrentOperation.ProductionOrderID
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
                        CreatorId = Adjustment.AdjustmentUserID
                    };

                    createdFirstProductApproval.SelectFirstProductApprovalLines.Add(firstProductApprovalLineModel);

                }

                var selectFirstProductApproval = (await FirstProductApprovalsAppService.CreateAsync(createdFirstProductApproval)).Data;


                if (selectFirstProductApproval.Id != Guid.Empty && selectFirstProductApproval != null)
                {
                    SendQualityControlApprovalButtonDisabled = true;
                    FirstApprovalId = selectFirstProductApproval.Id;
                    FirstApprovalCode = selectFirstProductApproval.Code;
                    FirstApprovalCodeVisible = true;

                    //System.Threading.Thread.Sleep(2000);
                    FirstApprovalControlTimer = new System.Timers.Timer(10000);
                    FirstApprovalControlTimer.Enabled = true;
                    FirstApprovalControlTimer.Elapsed += FirstApprovalTimedEvent;
                }
            }

            #endregion

        }
        #endregion

        #region Kalite Kontrol Onay Bekleme Metotlar

        System.Timers.Timer FirstApprovalControlTimer;

        private async void FirstApprovalTimedEvent(object source, ElapsedEventArgs e)
        {
            bool isFirstApprovalRecordIsApproved = (await FirstApprovalControl());

            if (isFirstApprovalRecordIsApproved)
            {
                FirstApprovalControlTimer.Stop();
                FirstApprovalControlTimer.Enabled = false;

                QualityControlApprovalTimer.Stop();
                AppService.CurrentOperation.TotalQualityControlApprovalTime = TotalQualityControlApprovalTime;
                QualityControlApprovalTimer.Enabled = false;

                AdjustmentTimer.Start();
                AdjustmentTimer.Enabled = true;

                FinishAdjustmentButtonDisabled = false;
            }

            await InvokeAsync(StateHasChanged);
        }

        public async Task<bool> FirstApprovalControl()
        {
            bool firstApprovalControl = false;

            if (!firstApprovalControl)
            {
                var firstApprovalRecord = (await FirstProductApprovalsAppService.GetAsync(FirstApprovalId)).Data;

                if (firstApprovalRecord.Id != Guid.Empty)
                {
                    firstApprovalControl = firstApprovalRecord.IsApproval;

                    ApprovedQuantity = firstApprovalRecord.ApprovedQuantity;

                    ScrapQuantity = firstApprovalRecord.ScrapQuantity;
                }
            }

            if (firstApprovalControl)
            {

                FirstApprovalStatus = "Onaylandı";
            }

            return firstApprovalControl;
        }


        #endregion

        public decimal ApprovedQuantity { get; set; }
        public decimal ScrapQuantity { get; set; }

        private async void FinishAdjustmentButtonClick()
        {
            AdjustmentTimer.Stop();
            AdjustmentTimer.Enabled = false;



            QualityControlApprovalTimer.Stop();
            QualityControlApprovalTimer.Enabled = false;

            FirstApprovalControlTimer.Stop();
            FirstApprovalControlTimer.Enabled = false;



            Adjustment.TotalAdjustmentTime = TotalAdjustmentTime;
            AppService.CurrentOperation.ApprovedQuantity = ApprovedQuantity;
            AppService.CurrentOperation.ScrapQuantity = ScrapQuantity;
            AppService.CurrentOperation.ProducedQuantity = ApprovedQuantity;

            AppService.OperationAdjustment = Adjustment;

            #region Local Operation Adjustment Insert
            OperationAdjustmentTable operationAdjustmentLocal = new OperationAdjustmentTable
            {
                AdjustmentDate = Adjustment.AdjustmentDate,
                AdjustmentUserID = Adjustment.AdjustmentUserID,
                AdjustmentUserName = Adjustment.AdjustmentUserName,
                AdjustmentUserPassword = Adjustment.AdjustmentUserPassword,
                TotalAdjustmentTime = Adjustment.TotalAdjustmentTime,
                TotalQualityControlApprovalTime = Adjustment.TotalQualityControlApprovalTime
            };

            await OperationAdjustmentLocalDbService.InsertAsync(operationAdjustmentLocal);
            #endregion

            #region ERP Operation Adjustment Insert
            var createAdjustmentDto = new CreateOperationAdjustmentsDto
            {
                AdjustmentStartDate = Adjustment.AdjustmentDate,
                AdjustmentUserId = Adjustment.AdjustmentUserID,
                TotalAdjustmentTime = Adjustment.TotalAdjustmentTime,
                WorkOrderId = AppService.CurrentOperation.WorkOrderID,
                ApprovedQuantity = AppService.CurrentOperation.ApprovedQuantity,
                ScrapQuantity = AppService.CurrentOperation.ScrapQuantity,
                OperatorId = Guid.NewGuid(),
                TotalQualityControlApprovedTime = AppService.CurrentOperation.TotalQualityControlApprovalTime
            };

            await OperationAdjustmentAppService.CreateAsync(createAdjustmentDto);
            #endregion

            #region ERP Production Tracking Insert

            var workOrder = (await WorkOrdersAppService.GetAsync(AppService.CurrentOperation.WorkOrderID)).Data;

            Guid CurrentAccountID = Guid.Empty;

            var endDate = GetSQLDateAppService.GetDateFromSQL();

            if (workOrder != null && workOrder.Id != Guid.Empty)
            {
                CurrentAccountID = workOrder.CurrentAccountCardID.GetValueOrDefault();
            }

            CreateProductionTrackingsDto productionTrackingModel = new CreateProductionTrackingsDto
            {
                AdjustmentTime = Convert.ToDecimal(endDate.TimeOfDay.Subtract(Adjustment.AdjustmentDate.TimeOfDay).TotalSeconds),
                Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu"),
                CurrentAccountCardID = CurrentAccountID,
                EmployeeID = AppService.CurrentOperation.EmployeeID,
                IsFinished = true,
                StationID = AppService.CurrentOperation.StationID,
                WorkOrderID = AppService.CurrentOperation.WorkOrderID,
                HaltReasonID = Guid.Empty,
                Description_ = string.Empty,
                FaultyQuantity = ScrapQuantity,
                ShiftID = Guid.Empty,
                HaltTime = 0,
                PlannedQuantity = AppService.CurrentOperation.PlannedQuantity,
                OperationStartDate = Adjustment.AdjustmentDate.Date,
                OperationStartTime = Adjustment.AdjustmentDate.TimeOfDay,
                OperationEndDate = endDate.Date,
                OperationEndTime = endDate.TimeOfDay,
                OperationTime = 0,
                ProductionTrackingTypes = 3,
                ProductsOperationID = AppService.CurrentOperation.ProductsOperationID,
                ProductID = AppService.CurrentOperation.ProductID,
                ProducedQuantity = ApprovedQuantity,
                ProductionOrderID = AppService.CurrentOperation.ProductionOrderID,
            };

            await ProductionTrackingsAppService.CreateAsync(productionTrackingModel);

            #endregion


            NavigationManager.NavigateTo("/work-order-detail");

            await Task.CompletedTask;
        }

        private async void CancelButtonClick()
        {

            if (AdjustmentTimer != null)
            {
                AdjustmentTimer.Stop();
                AdjustmentTimer.Dispose();
                AdjustmentTimer.Enabled = false;
            }

            if (QualityControlApprovalTimer != null)
            {
                QualityControlApprovalTimer.Stop();
                QualityControlApprovalTimer.Dispose();
                QualityControlApprovalTimer.Enabled = false;
            }

            if (FirstApprovalControlTimer != null)
            {
                FirstApprovalControlTimer.Stop();
                FirstApprovalControlTimer.Dispose();
                FirstApprovalControlTimer.Enabled = false;
            }

            if (AppService.AdjustmentState == States.AdjustmentState.FromNonOperation)
            {
                NavigationManager.NavigateTo("/before-operation");
            }

            if (AppService.AdjustmentState == States.AdjustmentState.FromOperation)
            {
                NavigationManager.NavigateTo("/work-order-detail");
            }


            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (AdjustmentTimer != null)
            {
                AdjustmentTimer.Dispose();
            }

            if (QualityControlApprovalTimer != null)
            {
                QualityControlApprovalTimer.Dispose();
            }

            if (FirstApprovalControlTimer != null)
            {
                FirstApprovalControlTimer.Dispose();
            }
        }
    }
}