using System.Timers;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;

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
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t=>t.IsOperator== true && t.IsIncidentalHalt == false).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsMachine()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t=>t.IsMachine== true && t.IsIncidentalHalt == false).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsManagement()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t=>t.IsManagement==true && t.IsIncidentalHalt == false).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsIncidental()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsIncidentalHalt == true).ToList();

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
