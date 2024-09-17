using System.Timers;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.UretimEkranUI.Models;
using TsiErp.UretimEkranUI.Services;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class SelectHaltReasonsListPage : IDisposable
    {
        public bool EndHaltReasonButtonDisable { get; set; } = true;

        WorkOrderOperationDetailPage WorkOrderOperationDetailPages;

        WorkOrderOperationDetailPage workOrderOperationDetailPage;

        public bool isPasswordVisible = false;

        string password = string.Empty;

        ListHaltReasonsDto haltReasonIncidental = new ListHaltReasonsDto();

        DateTime starthaltDate = DateTime.MinValue;

        protected override async void OnInitialized()
        {

            workOrderOperationDetailPage = (WorkOrderOperationDetailPage)OperationDetailPage["OperationDetailPage"];

            await StationsAppService.UpdateStationWorkStateAsync(AppService.CurrentOperation.StationID, 0);

            starthaltDate = GetSQLDateAppService.GetDateFromSQL();

            #region Sistem Genel Durum Update
            var generalStatus = (await SystemGeneralStatusLocalDbService.GetListAsync()).FirstOrDefault();

            if (generalStatus != null)
            {
                generalStatus.GeneralStatus = 0;

                await SystemGeneralStatusLocalDbService.UpdateAsync(generalStatus);
            }
            #endregion


            StartTimer();

            await InvokeAsync(() => StateHasChanged());
        }

        #region Halt Reasons

        List<ListHaltReasonsDto> HaltReasonsList = new List<ListHaltReasonsDto>();

        ListHaltReasonsDto SelectedHaltReason = new ListHaltReasonsDto();

        private async Task GetHaltReasonsIsOperator()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsOperator == true && t.IsIncidentalHalt == false).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsMachine()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsMachine == true && t.IsIncidentalHalt == false).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsManagement()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsManagement == true && t.IsIncidentalHalt == false).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsIncidental()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsIncidentalHalt == true).ToList();

            await Task.CompletedTask;
        }

        private async void OnSelectHaltReason(ListHaltReasonsDto haltReason)
        {
            if (!string.IsNullOrEmpty(haltReason.Name))
            {
                SelectedHaltReason = haltReason;
                EndHaltReasonButtonDisable = false;
                await InvokeAsync(() => StateHasChanged());
            }
            else
            {
                EndHaltReasonButtonDisable = true;
                await InvokeAsync(() => StateHasChanged());
            }
        }

        private void OnSelectIncidentalHaltReason(ListHaltReasonsDto haltReason)
        {
            isPasswordVisible = true;
            password = string.Empty;
            haltReasonIncidental = haltReason;
        }

        #endregion

        public async void OnPasswordSubmit()
        {
            if (!string.IsNullOrEmpty(haltReasonIncidental.Name))
            {
                SelectedHaltReason = haltReasonIncidental;
                EndHaltReasonButtonDisable = false;
                await InvokeAsync(() => StateHasChanged());
            }
            else
            {
                EndHaltReasonButtonDisable = true;
                await InvokeAsync(() => StateHasChanged());
            }

            HidePasswordModal();
        }

        public void HidePasswordModal()
        {
            isPasswordVisible = false;
            password = string.Empty;
            haltReasonIncidental = new ListHaltReasonsDto();
        }

        private async void EndHaltReasonButtonClick()
        {
            if (workOrderOperationDetailPage != null)
            {

                //workOrderOperationDetailPage.OperationStartButtonClicked();

                //Operasyonun devamıyla alakalı metod yazılacak

                var today = GetSQLDateAppService.GetDateFromSQL();

                #region Local Operation Halt Reason Table Insert

                OperationHaltReasonsTable haltReasonModel = new OperationHaltReasonsTable
                {
                    EmployeeID = AppService.CurrentOperation.EmployeeID,
                    EmployeeName = AppService.CurrentOperation.EmployeeName,
                    EndHaltDate = DateTime.Now.Date,
                    HaltReasonID = SelectedHaltReason.Id,
                    HaltReasonName = SelectedHaltReason.Name,
                    StartHaltDate = starthaltDate,
                    StationID = AppService.CurrentOperation.StationID,
                    StationCode = AppService.CurrentOperation.StationCode,
                    WorkOrderID = AppService.CurrentOperation.WorkOrderID,
                    WorkOrderNo = AppService.CurrentOperation.WorkOrderNo,
                    TotalHaltReasonTime = (HaltStartTime.Hour * 3600) + (HaltStartTime.Minute * 60) + HaltStartTime.Second,
                };

                await OperationHaltReasonsTableLocalDbService.InsertAsync(haltReasonModel);

                #endregion

                #region ERP Production Tracking Insert

                var workOrder = (await WorkOrdersAppService.GetAsync(AppService.CurrentOperation.WorkOrderID)).Data;

                Guid CurrentAccountID = Guid.Empty;

                if (workOrder != null && workOrder.Id != Guid.Empty)
                {
                    CurrentAccountID = workOrder.CurrentAccountCardID.GetValueOrDefault();
                }

                CreateProductionTrackingsDto trackingModel = new CreateProductionTrackingsDto
                {
                    AdjustmentTime = 0,
                    Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu"),
                    CurrentAccountCardID = CurrentAccountID,
                    HaltReasonID = SelectedHaltReason.Id,
                    EmployeeID = AppService.CurrentOperation.EmployeeID,
                    Description_ = string.Empty,
                    HaltTime = Convert.ToDecimal(today.TimeOfDay.Subtract(starthaltDate.TimeOfDay).TotalSeconds),
                    FaultyQuantity = AppService.CurrentOperation.ScrapQuantity,
                    IsFinished = true,
                    OperationEndDate = today.Date,
                    OperationEndTime = today.TimeOfDay,
                    OperationStartDate = starthaltDate.Date,
                    OperationStartTime = starthaltDate.TimeOfDay,
                    OperationTime = 0,
                    PlannedQuantity = AppService.CurrentOperation.PlannedQuantity,
                    ProducedQuantity = AppService.CurrentOperation.ProducedQuantity,
                    ProductID = AppService.CurrentOperation.ProductID,
                    ProductionTrackingTypes = 0,
                    ProductionOrderID = AppService.CurrentOperation.ProductionOrderID,
                    ProductsOperationID = AppService.CurrentOperation.ProductsOperationID,
                    ShiftID = Guid.Empty,
                    WorkOrderID = AppService.CurrentOperation.WorkOrderID,
                    StationID = AppService.CurrentOperation.StationID,

                };

                await ProductionTrackingsAppService.CreateAsync(trackingModel);

                #endregion

                Navigation.NavigateTo("/work-order-detail");


                HaltTimerDispose();
            }

            await InvokeAsync(() => StateHasChanged());
        }

        #region Timer

        public string TotalHaltReasonTime { get; set; } = "0:0:0";

        System.Timers.Timer _haltTimer = new System.Timers.Timer(1000);

        DateTime HaltStartTime = DateTime.Now;

        void StartTimer()
        {
            HaltStartTime = DateTime.Now;
            _haltTimer = new System.Timers.Timer(1000);
            _haltTimer.Elapsed += OnTimedEvent;
            _haltTimer.AutoReset = true;
            _haltTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            DateTime currentTime = e.SignalTime;

            TotalHaltReasonTime = currentTime.Subtract(HaltStartTime).Hours + ":" + currentTime.Subtract(HaltStartTime).Minutes + ":" + currentTime.Subtract(HaltStartTime).Seconds;

            InvokeAsync(StateHasChanged);
        }

        public void HaltTimerDispose()
        {
            if (_haltTimer != null)
            {
                _haltTimer.Stop();
                _haltTimer.Enabled = false;
                _haltTimer.Dispose();
            }
        }

        #endregion

        public void Dispose()
        {
            if (_haltTimer != null)
            {
                _haltTimer.Stop();
                _haltTimer.Enabled = false;
                _haltTimer.Dispose();
            }
        }
    }
}
