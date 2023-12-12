using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;
using TsiErp.UretimEkranUI.Services;

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

        public decimal QualityPercent { get; set; } = 0;

        protected override void OnInitialized()
        {
            QualityPercent = 1;

            ScrapQuantityCalculate();

            //StartTimer();
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

                if (AppService.CurrentOperation.ScrapQuantity <= AppService.CurrentOperation.WorkOrder.ProducedQuantity)
                {
                    //AppService.CurrentOperation.ScrapQuantity = AppService.CurrentOperation.ScrapQuantity + 1;

                    QualityPercent = 1 - (AppService.CurrentOperation.ScrapQuantity / AppService.CurrentOperation.WorkOrder.ProducedQuantity);

                    await InvokeAsync(StateHasChanged);
                }
            }
        }


        #region Operation Timer

        public string ElapsedTime { get; set; } = "0:0:0";

        System.Timers.Timer _timer = new System.Timers.Timer(1);

        DateTime StartTime = DateTime.Now;

        void StartTimer()
        {
            StartTime = DateTime.Now;
            _timer = new System.Timers.Timer(1);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            DateTime currentTime = e.SignalTime;

            ElapsedTime = currentTime.Subtract(StartTime).Hours + ":" + currentTime.Subtract(StartTime).Minutes + ":" + currentTime.Subtract(StartTime).Seconds;

            InvokeAsync(StateHasChanged);
        }

        #endregion

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
