using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Syncfusion.Blazor.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.UretimEkranUI.Models;
using TsiErp.UretimEkranUI.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class WorkOrderOperationDetailPage : IDisposable
    {

        #region Button Disable Properties
        public bool StartOperationButtonDisabled { get; set; } = false;
        public bool PauseOperationButtonDisabled { get; set; } = true;
        public bool EndOperationButtonDisabled { get; set; } = true;
        public bool ScrapQuantityButtonDisabled { get; set; } = true;
        public bool ChangeOperationButtonDisabled { get; set; } = false;
        public bool ChangeShiftButtonDisabled { get; set; } = false;
        public bool ChangeCaseButtonDisabled { get; set; } = false;
        #endregion

        public string TotalAdjusmentTime { get; set; }

        public decimal QualityPercent { get; set; } = 0;

        public bool ScrapQuantityEntryModalVisible = false;

        public decimal scrapQuantity = 0;

        protected override async void OnInitialized()
        {
            var totalAdjusmentTime = (await OperationAdjustmentAppService.GetTotalAdjustmentTimeAsync(AppService.CurrentOperation.WorkOrderID));

            if (totalAdjusmentTime > 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(totalAdjusmentTime);

                TotalAdjusmentTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }


            QualityPercent = 1;

            ScrapQuantityCalculate();

        }

        private async void ProducedQuantityAdded()
        {
            //AppService.CurrentOperation.WorkOrder.ProducedQuantity = AppService.CurrentOperation.WorkOrder.ProducedQuantity + 1;

            //QualityPercent = 1 - (AppService.CurrentOperation.ScrapQuantity / AppService.CurrentOperation.WorkOrder.ProducedQuantity);

            await InvokeAsync(StateHasChanged);
        }

        #region Operation Start

        public decimal FirstProducedQuantity = 0;


        public async void OperationStartButtonClicked()
        {
            FirstProducedQuantity = AppService.CurrentOperation.ProducedQuantity;

            ScrapQuantityButtonDisabled = false;
            EndOperationButtonDisabled = false;
            PauseOperationButtonDisabled = false;
            StartOperationButtonDisabled = true;

            StartOperationTimer();

            await InvokeAsync(StateHasChanged);
        }


        public void IncreaseQuantity()
        {
            AppService.CurrentOperation.ProducedQuantity = AppService.CurrentOperation.ProducedQuantity + 1;

            FirstProducedQuantity = AppService.CurrentOperation.ProducedQuantity;

            UpdatedOperationStartTime = DateTime.Now;


        }

        #endregion

        #region Operation Stop

        public async void OperationStopButtonClicked()
        {
            StopOperationTimer();

            NavigationManager.NavigateTo("/halt-reasons");


            await InvokeAsync(StateHasChanged);
        }

        void StopOperationTimer()
        {
            operationStartTimer.Stop();
            operationStartTimer.Enabled = false;
        }

        #endregion

        #region End Operation

        public async void EndOperationButtonClicked()
        {
            var plannedQuantity = AppService.CurrentOperation.PlannedQuantity;
            var producedQuantity = AppService.CurrentOperation.ProducedQuantity;
            var scrapQuantity = AppService.CurrentOperation.ScrapQuantity;

            if (plannedQuantity == producedQuantity + scrapQuantity)
            {
                if (plannedQuantity > producedQuantity)
                {
                    ScrapQuantityEntryModalVisible = true;
                }

                OperationStartTimerDispose();

                TotalOperationTime = "0:0:0";

                ScrapQuantityButtonDisabled = true;
                PauseOperationButtonDisabled = true;
                EndOperationButtonDisabled = true;
                StartOperationButtonDisabled = false;

                AppService.CurrentOperation.PlannedQuantity = 0;
                AppService.CurrentOperation.ProducedQuantity = 0;
                AppService.CurrentOperation.ScrapQuantity = 0;
            }


            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Scrap Quantity Entry 

        public async void ScrapQuantityEntryButtonClicked()
        {
            ScrapQuantityEntryModalVisible = true;

            await InvokeAsync(StateHasChanged);
        }

        public async void ScrapQuantityEntryOnSubmit()
        {
            AppService.CurrentOperation.ScrapQuantity = scrapQuantity;

            // Miktar girişi ve girilen hurda için oto Operasyon Uygunsuzluk Kaydı açma kodu

            HideScrapQuantityEntryModal();

            await InvokeAsync(StateHasChanged); 
        }

        public void HideScrapQuantityEntryModal()
        {
            ScrapQuantityEntryModalVisible = false;
        }

        #endregion

        private async void ScrapQuantityCalculate()
        {
            if (AppService.CurrentOperation.ScrapQuantity > 0)
            {

                if (AppService.CurrentOperation.ScrapQuantity <= AppService.CurrentOperation.ProducedQuantity)
                {
                    //AppService.CurrentOperation.ScrapQuantity = AppService.CurrentOperation.ScrapQuantity + 1;

                    QualityPercent = 1 - (AppService.CurrentOperation.ScrapQuantity / AppService.CurrentOperation.ProducedQuantity);

                    await InvokeAsync(StateHasChanged);
                }
            }
        }


        #region Operation Timer

        public string TotalOperationTime { get; set; } = "0:0:0";

        System.Timers.Timer operationStartTimer = new System.Timers.Timer(1000);

        DateTime OperationStartTime = DateTime.Now;

        DateTime UpdatedOperationStartTime = DateTime.Now;

        void StartOperationTimer()
        {
            OperationStartTime = DateTime.Now;
            UpdatedOperationStartTime = DateTime.Now;
            operationStartTimer = new System.Timers.Timer(1000);
            operationStartTimer.Elapsed += OnTimedEvent;
            operationStartTimer.AutoReset = true;
            operationStartTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            DateTime currentTime = e.SignalTime;

            TotalOperationTime = currentTime.Subtract(OperationStartTime).Hours + ":" + currentTime.Subtract(OperationStartTime).Minutes + ":" + currentTime.Subtract(OperationStartTime).Seconds;


            if (currentTime.Subtract(UpdatedOperationStartTime).Minutes == 3 && AppService.CurrentOperation.ProducedQuantity == FirstProducedQuantity)
            {
                StopOperationTimer();

                NavigationManager.NavigateTo("/halt-reasons");
            }

            InvokeAsync(StateHasChanged);
        }

        public void OperationStartTimerDispose()
        {
            if (operationStartTimer != null)
            {
                operationStartTimer.Stop();
                operationStartTimer.Enabled = false;
                operationStartTimer.Dispose();
            }
        }

        #endregion

        #region System Idle Time

        public int TotalSystemIdleTime { get; set; }


        System.Timers.Timer _systemIdleTimer = new System.Timers.Timer(1000);

        DateTime SystemIdleStartTime = DateTime.Now;

        void StartSystemIdleTimer()
        {
            SystemIdleStartTime = DateTime.Now;
            _systemIdleTimer = new System.Timers.Timer(1000);
            _systemIdleTimer.Elapsed += SystemIdleOnTimedEvent;
            _systemIdleTimer.AutoReset = true;
            _systemIdleTimer.Enabled = true;
        }

        private void SystemIdleOnTimedEvent(object source, ElapsedEventArgs e)
        {
            TotalSystemIdleTime++;

            TimeSpan time = TimeSpan.FromSeconds(TotalSystemIdleTime);

            if (time.Minutes == (((AppService.ProgramParameters.HaltTriggerSecond) / 1000) / 60))
            {

                HaltReasonModalVisible = true;
                StartHaltReasonTimer();
                _systemIdleTimer.Stop();
                _systemIdleTimer.Enabled = false;
                InvokeAsync(StateHasChanged);

            }

            InvokeAsync(StateHasChanged);
        }


        #endregion

        #region Halt Reasons

        public bool HaltReasonModalVisible { get; set; }

        public bool EndHaltReasonButtonDisable { get; set; } = true;

        List<ListHaltReasonsDto> HaltReasonsList = new List<ListHaltReasonsDto>();

        ListHaltReasonsDto SelectedHaltReason = new ListHaltReasonsDto();

        private async Task GetHaltReasonsIsOperator()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsOperator == true).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsMachine()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsMachine == true).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsManagement()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsManagement == true).ToList();

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

        #region Halt Reason Timer

        public int TotalHaltReasonTime { get; set; }

        public string HaltReasonTime { get; set; } = "0:0:0";

        System.Timers.Timer _haltReasonTimer = new System.Timers.Timer(1000);

        DateTime HaltReasonStartTime = DateTime.Now;

        void StartHaltReasonTimer()
        {
            HaltReasonStartTime = DateTime.Now;
            _haltReasonTimer = new System.Timers.Timer(1000);
            _haltReasonTimer.Elapsed += HaltReasonOnTimedEvent;
            _haltReasonTimer.AutoReset = true;
            _haltReasonTimer.Enabled = true;
        }

        private void HaltReasonOnTimedEvent(object source, ElapsedEventArgs e)
        {
            TotalHaltReasonTime++;

            TimeSpan time = TimeSpan.FromSeconds(TotalHaltReasonTime);

            HaltReasonTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                 time.Hours,
                 time.Minutes,
                 time.Seconds);

            InvokeAsync(StateHasChanged);
        }

        #endregion

        private async void EndHaltReasonButtonClick()
        {
            _haltReasonTimer.Enabled = false;
            _haltReasonTimer.Stop();
            StartSystemIdleTimer();

            var haltReason = new OperationHaltReasonsTable
            {
                EmployeeID = AppService.EmployeeID,
                HaltReasonID = SelectedHaltReason.Id,
                StationID = AppService.ProgramParameters.StationID,
                TotalHaltReasonTime = TotalHaltReasonTime,
                WorkOrderID = AppService.CurrentOperation.WorkOrderID
            };

            await OperationHaltReasonsTableLocalDbService.InsertAsync(haltReason);

            TotalHaltReasonTime = 0;

            HaltReasonTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                 0,
                 0,
                 0);
            SelectedHaltReason = new ListHaltReasonsDto();
            TotalSystemIdleTime = 0;


            HaltReasonModalVisible = false;

        }

        #endregion

        public void StartAdjustment()
        {
            AppService.AdjustmentState = Utilities.EnumUtilities.States.AdjustmentState.FromOperation;
            NavigationManager.NavigateTo("/operation-adjustment");
        }


        private void AmountControl()
        {

        }



        public void Dispose()
        {
            if (operationStartTimer != null)
            {
                operationStartTimer.Stop();
                operationStartTimer.Enabled = false;
                //operationStartTimer.Dispose();
            }
        }
    }
}
