using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services;
using TsiErp.Business.Entities.Menu.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class SelectHaltReasonsListPage : IDisposable
    {
        public bool EndHaltReasonButtonDisable { get; set; } = true;

        protected override async void OnInitialized()
        {
            StartTimer();
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
            Navigation.NavigateTo("/home");
        }

        #region Timer

        public string TotalHaltReasonTime { get; set; } = "0:0:0";

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

            TotalHaltReasonTime = currentTime.Subtract(StartTime).Hours + ":" + currentTime.Subtract(StartTime).Minutes + ":" + currentTime.Subtract(StartTime).Seconds;

            InvokeAsync(StateHasChanged);
        }

        #endregion

        public void Dispose()
        {
        }
    }
}
