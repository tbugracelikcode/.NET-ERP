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
    public partial class WorkOrderOperationDetailPage
    {

        public string ElapsedTime { get; set; } = "0:0:0";

        System.Timers.Timer _timer = new System.Timers.Timer(1);

        DateTime StartTime = DateTime.Now;


        public decimal ScrapQuantity { get; set; } = 0;
        public decimal QualityPercent { get; set; } = 0;

        protected override async void OnInitialized()
        {
            QualityPercent = 1;
            //StartTimer();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            DateTime currentTime = e.SignalTime;
            
            ElapsedTime = currentTime.Subtract(StartTime).Hours + ":" + currentTime.Subtract(StartTime).Minutes + ":" + currentTime.Subtract(StartTime).Seconds;

            InvokeAsync(StateHasChanged);
        }

        void StartTimer()
        {
            StartTime = DateTime.Now;
            _timer = new System.Timers.Timer(1);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private async void ProducedQuantityAdded()
        {
            AppService.CurrentOperation.WorkOrder.ProducedQuantity = AppService.CurrentOperation.WorkOrder.ProducedQuantity + 1;

            QualityPercent = 1 - (ScrapQuantity / AppService.CurrentOperation.WorkOrder.ProducedQuantity);

            await InvokeAsync(StateHasChanged);
        }

        private async void ScrapQuantityAdded()
        {
            if (ScrapQuantity <= AppService.CurrentOperation.WorkOrder.ProducedQuantity)
            {
                ScrapQuantity = ScrapQuantity + 1;

                QualityPercent = 1 - (ScrapQuantity / AppService.CurrentOperation.WorkOrder.ProducedQuantity);

                await InvokeAsync(StateHasChanged);
            }

        }
    }
}
