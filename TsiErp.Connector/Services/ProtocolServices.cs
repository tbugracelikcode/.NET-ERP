using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Connector.Helpers;

namespace TsiErp.Connector.Services
{
    public class ProtocolServices : IProtocolServices
    {
        public string M001R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M001W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M002R(string ipAddress)
        {
            string result = string.Empty;
            TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

            StreamWriter writer = new StreamWriter(client.GetStream());
            writer.WriteLine(ProtocolHeaders.M001R);
            writer.Flush();

            TcpListener listener = new TcpListener(IPAddress.Parse(ipAddress), ProtocolPorts.DataReceivingPort);
            listener.Start();

            client = listener.AcceptTcpClient();

            if (client != null)
            {
                if (client.Connected)
                {
                    StreamReader reader = new StreamReader(client.GetStream());
                    result = reader.ReadLine();

                    switch (result)
                    {
                        case "2":
                            result = ProtocolErrors.Error2;
                            break;
                        case "4":
                            result = ProtocolErrors.Error4;
                            break;
                        case "5":
                            result = ProtocolErrors.Error5;
                            break;
                        case "6":
                            result = ProtocolErrors.Error6;
                            break;
                        case "7":
                            result = ProtocolErrors.Error7;
                            break;
                        case "9":
                            result = ProtocolErrors.Error9;
                            break;
                        case "A":
                            result = ProtocolErrors.ErrorA;
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    result = ProtocolErrors.ErrorResultNull;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorTcpClientNull;
            }

            return result;
        }

        public string M003R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M004R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M005R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M006R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M007R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M008W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M009R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M009W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M010R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M011W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M012R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M013R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M014W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M015R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M016W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M017W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M018R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M019R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M020R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M021R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M022R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M022W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M023R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M023W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M024R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M024W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M025R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M025W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M026R(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M026W(string ipAddress)
        {
            throw new NotImplementedException();
        }

        public string M027R(string ipAddress)
        {
            throw new NotImplementedException();
        }
    }
}
