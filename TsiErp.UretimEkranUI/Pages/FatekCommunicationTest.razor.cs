using System.IO.Ports;
using TsiErp.Fatek.CommunicationCore;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class FatekCommunicationTest
    {

        async void WriteToPLC()
        {
            FatekCommunication objFatekCommunication = new FatekCommunication("COM1", 9600, Parity.Even, 7, StopBits.One);
            objFatekCommunication.Connect();

            

            objFatekCommunication.SetItem(Fatek.CommunicationCore.Base.MemoryType.DR, 360, 6330);
            int a = objFatekCommunication.GetItem(Fatek.CommunicationCore.Base.MemoryType.DR, 360);

        }
    }
}
