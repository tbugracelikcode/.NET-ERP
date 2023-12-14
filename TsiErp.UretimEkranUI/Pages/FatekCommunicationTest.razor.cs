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
            //try
            //{
            //    string[] ports = SerialPort.GetPortNames();

            //    SerialPort sp = new SerialPort("COM1", 9600, System.IO.Ports.Parity.Even, 7, System.IO.Ports.StopBits.One);
            //    sp.Handshake = Handshake.None;
            //    sp.DtrEnable = true;

            //    sp.Open();
            //}
            //catch (IOException exp)
            //{
            //}
        }
    }
}
