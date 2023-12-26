using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TsiErp.Fatek.CommunicationCore.Base;
using TsiErp.UretimEkranUI.Services;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class BeforeOperationPage : IDisposable
    {

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                HaltReasonTimer = new System.Timers.Timer(1000);
                HaltReasonTimer.Elapsed += HaltReasonTimerTimedEvent;
                HaltReasonTimer.Enabled = true;
            }
        }

        #region Duruş Kontrol Timer

        System.Timers.Timer HaltReasonTimer;

        public bool HaltReasonModalVisible { get; set; } = false;

        private void HaltReasonTimerTimedEvent(object source, ElapsedEventArgs e)
        {
            if (Convert.ToBoolean(AppService.FatekCommunication.GetDis(MemoryType.M, 16)) == true)
            {
                if (!HaltReasonModalVisible)
                {
                    HaltReasonModalVisible = true;
                    //Duruş bilgisi modalı çıkacak. DR600 deki değer okunacak. Bu değer duruş süresi bilgisidir. Duruş bilgisi modal kapandıktan sonra HaltReasonModalVisible false yapılacak. DR600 0 set edilecek.
                }
            }

            InvokeAsync(StateHasChanged);
        }

        #endregion

        public void StartAdjustment()
        {
            //Ayar süresini için M8 registerını setliyoruz
            AppService.FatekCommunication.SetDis(MemoryType.M, 8, RunningCode.Set);

            AppService.AdjustmentState = Utilities.EnumUtilities.States.AdjustmentState.FromNonOperation;
            NavigationManager.NavigateTo("/operation-adjustment");
        }

        public void StartOperation()
        {
            //PLC de üretim başlat register ı set edildi.
            AppService.FatekCommunication.SetDis(MemoryType.M,3,RunningCode.Set);

            //PLC de üretim süresini başlat register ı set edildi.
            AppService.FatekCommunication.SetDis(MemoryType.M,9,RunningCode.Set);

            NavigationManager.NavigateTo("/work-order-detail");
        }

        public void Dispose()
        {
            if (HaltReasonTimer != null)
            {
                HaltReasonTimer.Enabled = false;
                HaltReasonTimer.Stop();
                HaltReasonTimer.Dispose();
            }
        }
    }
}
