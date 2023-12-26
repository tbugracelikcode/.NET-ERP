using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.UretimEkranUI.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class WorkOrderOperationDetailPage : IDisposable
    {

        #region Button Disable Properties
        public bool StartOperationButtonDisabled { get; set; } = true;
        public bool PauseOperationButtonDisabled { get; set; } = true;
        public bool EndOperationButtonDisabled { get; set; } = true;
        public bool ScrapQuantityButtonDisabled { get; set; } = true;
        public bool ChangeOperationButtonDisabled { get; set; } = true;
        public bool ChangeShiftButtonDisabled { get; set; } = true;
        public bool ChangeCaseButtonDisabled { get; set; } = true;
        #endregion

        public string TotalAdjusmentTime { get; set; }

        public decimal QualityPercent { get; set; } = 0;

        protected override async void OnInitialized()
        {
            var totalAdjusmentTime = (await OperationAdjustmentAppService.GetTotalAdjustmentTimeAsync(AppService.CurrentOperation.WorkOrderID));

            if(totalAdjusmentTime > 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(totalAdjusmentTime);

                TotalAdjusmentTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }


            QualityPercent = 1;


            ScrapQuantityCalculate();

            StartTimer();


        }

        private async void ProducedQuantityAdded()
        {
            //AppService.CurrentOperation.WorkOrder.ProducedQuantity = AppService.CurrentOperation.WorkOrder.ProducedQuantity + 1;

            //QualityPercent = 1 - (AppService.CurrentOperation.ScrapQuantity / AppService.CurrentOperation.WorkOrder.ProducedQuantity);

            await InvokeAsync(StateHasChanged);
        }

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

        System.Timers.Timer _timer = new System.Timers.Timer(1000);

        DateTime StartTime = DateTime.Now;

        void StartTimer()
        {
            StartTime = DateTime.Now;
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            DateTime currentTime = e.SignalTime;

            TotalOperationTime = currentTime.Subtract(StartTime).Hours + ":" + currentTime.Subtract(StartTime).Minutes + ":" + currentTime.Subtract(StartTime).Seconds;

            InvokeAsync(StateHasChanged);
        }

        #endregion

        public void StartAdjustment()
        {
            //Operasyon süresini durdurmak için M9 registerını resetliyoruz
            AppService.FatekCommunication.SetDis(Fatek.CommunicationCore.Base.MemoryType.M, 9, Fatek.CommunicationCore.Base.RunningCode.Reset);

            //Ayar süresini başlatmak için M8 registerını setliyoruz
            AppService.FatekCommunication.SetDis(Fatek.CommunicationCore.Base.MemoryType.M, 8, Fatek.CommunicationCore.Base.RunningCode.Set);

            AppService.AdjustmentState = Utilities.EnumUtilities.States.AdjustmentState.FromOperation;
            NavigationManager.NavigateTo("/operation-adjustment");
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Enabled = false;
                _timer.Dispose();
            }
        }
    }
}
