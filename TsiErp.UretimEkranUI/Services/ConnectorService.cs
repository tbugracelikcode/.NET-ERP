using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TsiErp.UretimEkranUI.Services
{
    public class ConnectorService
    {
        public static void SendAndRead(string data, out string returnData, string ipAdres, int veriGondermePortu, int veriAlmaPortu)
        {
            TcpClient client = new TcpClient(ipAdres, veriGondermePortu);

            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.WriteLine(data);
            writer.Flush();

            TcpListener listener = new TcpListener(IPAddress.Parse(ipAdres), veriAlmaPortu);
            listener.Start();

            client = listener.AcceptTcpClient();

            StreamReader reader = new StreamReader(client.GetStream());
            returnData = reader.ReadLine();

            listener.Stop();
            client.Close();
        }

    }
}
