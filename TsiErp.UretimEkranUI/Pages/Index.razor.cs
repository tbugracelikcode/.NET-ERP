using DevExpress.ClipboardSource.SpreadsheetML;
using Microsoft.AspNetCore.Components;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.UretimEkranUI.Models;
using TsiErp.UretimEkranUI.Services;
using TsiErp.UretimEkranUI.Shared;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class Index : IDisposable
    {

        protected override async void OnInitialized()
        {
            //ParameterControl();

            //#region Yarım kalan operasyon kontrol
            //var workOrderControl = (await OperationDetailLocalDbService.GetListAsync()).ToList();

            //if (workOrderControl.Count > 0)
            //{
            //    NavMenu menu = (NavMenu)mss["NavMenu"];
            //    menu.ChangeWorkOrderMenuEnabled(false);
            //    menu.ChangeLogoutMenuEnabled(false);
            //    menu.ChangeMainPageMenuEnabled(false);
            //    AppService.CurrentOperation = await OperationDetailLocalDbService.GetAsync(workOrderControl[0].Id);
            //}
            //#endregion

            //StartSystemIdleTimer();

            string data = "";

            ConnectorService.SendAndRead("M014W", out data, "127.0.0.1", 1644, 4416);

        }


        private async void ParameterControl()
        {
            int parameterId = await ParametersTableLocalDbService.ParameterControl();

            if (parameterId == 0)
            {
                var createdParameter = new ParametersTable
                {
                    HaltTriggerSecond = 120000,
                    IsLoadcell = false,
                    MidControlPeriod = 0,
                    StationID = Guid.Empty
                };

                await ParametersTableLocalDbService.InsertAsync(createdParameter);

                var insertedParameters = await ParametersTableLocalDbService.GetAsync(1);

                if (insertedParameters.Id > 0)
                {
                    AppService.ProgramParameters = insertedParameters;
                }

            }
            else
            {
                AppService.ProgramParameters = await ParametersTableLocalDbService.GetAsync(parameterId);
            }
        }


        private async void GoToCurrentWorkOrder()
        {
            Navigation.NavigateTo("/work-order-detail");
            await Task.CompletedTask;
        }

        private async void ShowModal()
        {
            await Task.CompletedTask;
        }

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
            SystemIdleTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                 0,
                 0,
                 0);

            HaltReasonModalVisible = false;
            
        }

        #endregion

        #region System Idle Time

        public int TotalSystemIdleTime { get; set; }

        public string SystemIdleTime { get; set; } = "00:00:00";

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

            SystemIdleTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                 time.Hours,
                 time.Minutes,
                 time.Seconds);

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

        private async Task ShowHaltReasonModal()
        {
            HaltReasonModalVisible = true;
            StartHaltReasonTimer();
            _systemIdleTimer.Stop();
            _systemIdleTimer.Enabled = false;
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {

        }
    }
}
