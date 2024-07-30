using System.Timers;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class SelectHaltReasonsListPage : IDisposable
    {
        public bool EndHaltReasonButtonDisable { get; set; } = true;

        WorkOrderOperationDetailPage WorkOrderOperationDetailPages;

        WorkOrderOperationDetailPage workOrderOperationDetailPage;

        protected override async void OnInitialized()
        {

            workOrderOperationDetailPage = (WorkOrderOperationDetailPage)OperationDetailPage["OperationDetailPage"];
            
            StartTimer();

            await InvokeAsync(() => StateHasChanged());
        }

        #region Halt Reasons

        List<ListHaltReasonsDto> HaltReasonsList = new List<ListHaltReasonsDto>();

        ListHaltReasonsDto SelectedHaltReason = new ListHaltReasonsDto();

        private async Task GetHaltReasonsIsOperator()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t=>t.IsOperator==true).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsMachine()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t=>t.IsMachine==true).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsManagement()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t=>t.IsManagement==true).ToList();

            await Task.CompletedTask;
        }

        private async void OnSelectHaltReason(ListHaltReasonsDto haltReason)
        {
            if(!string.IsNullOrEmpty(haltReason.Name))
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

        #endregion

        private async void EndHaltReasonButtonClick()
        {
            if(workOrderOperationDetailPage!=null)
            {

                //workOrderOperationDetailPage.OperationStartButtonClicked();

                //Operasyonun devamıyla alakalı metod yazılacak

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
