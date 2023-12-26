using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TsiErp.Fatek.CommunicationCore;
using TsiErp.Fatek.CommunicationCore.Base;
using TsiErp.UretimEkranUI.Models;
using TsiErp.UretimEkranUI.Services;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class Index : IDisposable
    {
        protected override async void OnInitialized()
        {
            FatekCommunication objFatekCommunication = new FatekCommunication("COM1", 9600, Parity.Even, 7, StopBits.One);
            bool isConnect = objFatekCommunication.Connect();
            AppService.FatekCommunication = objFatekCommunication;

            if (isConnect)
            {
                SetPLCTime();

                // Uygulama ayağa kalktığı anda boşta bekleme sayacı PLC tarafından start ediliyor. Operasyon dışındaki duruş süresini yakalamak için kullanılıyor.DR550 saymaya başladı.
                objFatekCommunication.SetDis(MemoryType.M, 14, RunningCode.Set);

                ParameterControl();

                #region Yarım kalan operasyon kontrol
                var workOrderControl = (await OperationDetailLocalDbService.GetListAsync()).ToList();

                if (workOrderControl.Count > 0)
                {
                    AppService.CurrentOperation = await OperationDetailLocalDbService.GetAsync(workOrderControl[0].Id);
                    NavigationManager.NavigateTo("/work-order-detail");
                }
                #endregion
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                HaltReasonTimer = new System.Timers.Timer(1000);
                HaltReasonTimer.Elapsed += HaltReasonTimerTimedEvent;
                HaltReasonTimer.Enabled = true;
            }
        }

        private async void ParameterControl()
        {
            int parameterId = await ParametersTableLocalDbService.ParameterControl();

            if (parameterId == 0)
            {
                var createdParameter = new ParametersTable
                {
                    HaltTriggerSecond = 0,
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

                // PLC'ye boşta bekleme parametresi tanımlanıyor.
                AppService.FatekCommunication.SetItem(MemoryType.R, 1000, AppService.ProgramParameters.HaltTriggerSecond);
            }
        }

        private async void SetPLCTime()
        {
            var currentDate = (await OperationDetailLocalDbService.GetCurrentTimeStamp());

            string[] TarihveSaat = currentDate.ToString().Split(' ');
            string[] Tarih = TarihveSaat[0].Split('-');
            int TarihGun = Convert.ToInt32(Tarih[2]);
            int TarihAY = Convert.ToInt32(Tarih[1]);
            int TarihYIL = Convert.ToInt32(Tarih[0].Substring(2, 2));

            string[] Zaman = TarihveSaat[1].Split(':');
            int Saat = Convert.ToInt32(Zaman[0]);
            int Dakika = Convert.ToInt32(Zaman[1]);
            int Saniye = Convert.ToInt32(Zaman[2]);

            AppService.FatekCommunication.SetItem(MemoryType.D, 2, TarihGun);
            AppService.FatekCommunication.SetItem(MemoryType.D, 1, TarihAY);
            AppService.FatekCommunication.SetItem(MemoryType.D, 0, TarihYIL);

            AppService.FatekCommunication.SetItem(MemoryType.D, 3, Saat);
            AppService.FatekCommunication.SetItem(MemoryType.D, 4, Dakika);
            AppService.FatekCommunication.SetItem(MemoryType.D, 5, Saniye);

            AppService.FatekCommunication.SetDis(MemoryType.M, 1952, RunningCode.Set);
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
