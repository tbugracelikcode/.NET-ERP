using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TsiErp.Entities.Entities.ProductionManagement.WorkOrder.Dtos;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class WorkOrderOperationDetailPage
    {
        [Parameter] public Guid WorkOrderId { get; set; }

        public string ElapsedTime { get; set; } = "0:0:0";
        public string ElapsedTime1 { get; set; } = "0:0:0";
        public string ElapsedTime2 { get; set; } = "0:0:0";

        System.Timers.Timer _timer = new System.Timers.Timer(1);
        System.Timers.Timer _timer1 = new System.Timers.Timer(1);
        System.Timers.Timer _timer2 = new System.Timers.Timer(1);

        DateTime StartTime = DateTime.Now;

        SelectWorkOrdersDto DataSource = new SelectWorkOrdersDto();

        public decimal ScrapQuantity { get; set; } = 0;
        public decimal QualityPercent { get; set; } = 0;

        protected override async void OnInitialized()
        {
            await GetWorkOrderDetail();
            //StartTimer();
        }

        private async Task GetWorkOrderDetail()
        {
            DataSource = (await WorkOrdersAppService.GetAsync(WorkOrderId)).Data;

            if (DataSource.Id != Guid.Empty)
            {
                QualityPercent = 1;
                DataSource.PlannedQuantity = 50;
            }
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            DateTime currentTime = e.SignalTime;

            ElapsedTime = currentTime.Subtract(StartTime).Hours + ":" + currentTime.Subtract(StartTime).Minutes + ":" + currentTime.Subtract(StartTime).Seconds;

            ElapsedTime1 = currentTime.Subtract(StartTime).Hours + ":" + currentTime.Subtract(StartTime).Minutes + ":" + currentTime.Subtract(StartTime).Seconds;

            ElapsedTime2 = currentTime.Subtract(StartTime).Hours + ":" + currentTime.Subtract(StartTime).Minutes + ":" + currentTime.Subtract(StartTime).Seconds;

            InvokeAsync(StateHasChanged);
        }

        void StartTimer()
        {
            StartTime = DateTime.Now;
            _timer = new System.Timers.Timer(1);
            _timer.Elapsed += OnTimedEvent;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer1 = new System.Timers.Timer(1);
            _timer1.Elapsed += OnTimedEvent;
            _timer1.AutoReset = true;
            _timer1.Enabled = true;
            _timer2 = new System.Timers.Timer(1);
            _timer2.Elapsed += OnTimedEvent;
            _timer2.AutoReset = true;
            _timer2.Enabled = true;
        }

        private async void ProducedQuantityAdded()
        {
            DataSource.ProducedQuantity = DataSource.ProducedQuantity + 1;

            QualityPercent = 1 - (ScrapQuantity / DataSource.ProducedQuantity);

            await InvokeAsync(StateHasChanged);
        }

        private async void ScrapQuantityAdded()
        {
            if (ScrapQuantity <= DataSource.ProducedQuantity)
            {
                ScrapQuantity = ScrapQuantity + 1;

                QualityPercent = 1 - (ScrapQuantity / DataSource.ProducedQuantity);

                await InvokeAsync(StateHasChanged);
            }

        }
    }
}
