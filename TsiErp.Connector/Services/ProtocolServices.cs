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
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
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

                            result = result.Substring(5);
                            listener.Stop();

                            if (result == "0")
                            {
                                result = ProtocolErrors.ErrorGeneral;
                            }
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M001W(string ipAddress, string data)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M001W + data);
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

                            result = result.Substring(5);
                            listener.Stop();

                            if (result != "0")
                            {
                                result = ProtocolErrors.ErrorGeneral;
                            }
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M002R(string ipAddress)
        {

            string result;

            try
            {
                TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.WriteLine(ProtocolHeaders.M002R);
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

                        result = result.Substring(5);
                        listener.Stop();
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorResultNull;
                        listener.Stop();
                    }
                }
                else
                {
                    result = ProtocolErrors.ErrorTcpClientNull;
                    listener.Stop();
                }
            }
            catch (Exception exp)
            {
                result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
            }

            return result;
        }

        public string M003R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M003R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M004R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M004R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M008W(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M008W);
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

                            result = result.Substring(5);
                            listener.Stop();

                            if (result != "0")
                            {
                                result = ProtocolErrors.ErrorGeneral;
                            }
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M010R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M010R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M011W(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M011W);
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

                            result = result.Substring(5);
                            listener.Stop();

                            if (result != "0")
                            {
                                result = ProtocolErrors.ErrorGeneral;
                            }
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M012R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M012R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M013R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M013R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M014W(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M014W);
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

                            result = result.Substring(5);
                            listener.Stop();

                            if (result != "0")
                            {
                                result = ProtocolErrors.ErrorGeneral;
                            }
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M015R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M015R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M016W(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M016W);
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

                            result = result.Substring(5);
                            listener.Stop();

                            if (result != "0")
                            {
                                result = ProtocolErrors.ErrorGeneral;
                            }
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }


            return result;
        }

        public string M017W(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M017W);
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

                            result = result.Substring(5);
                            listener.Stop();

                            if (result != "0")
                            {
                                result = ProtocolErrors.ErrorGeneral;
                            }
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }

            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M018R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M018R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M019R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M019R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M020R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M020R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }


            return result;
        }

        public string M021R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M021R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M026R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M026R);
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

                            result = result.Substring(5);
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M026W(string ipAddress, string data)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M026W + data);
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

                            result = result.Substring(5);
                            listener.Stop();

                            if (result != "0")
                            {
                                result = ProtocolErrors.ErrorGeneral;
                            }
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M028R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {
                try
                {

                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M028R);
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
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }





                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }

        public string M029R(string ipAddress)
        {
            string result;
            string plcStatus = M002R(ipAddress);

            if (plcStatus == "1")
            {

                try
                {
                    TcpClient client = new TcpClient(ipAddress, ProtocolPorts.DataSendingPort);

                    StreamWriter writer = new StreamWriter(client.GetStream());
                    writer.WriteLine(ProtocolHeaders.M029R);
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

                            result = result.Substring(5);

                            string attachtime = result.Substring(0, 7);
                            string operationtime = result.Substring(8, 15);

                            result = attachtime + "-" + operationtime;
                            listener.Stop();
                        }
                        else
                        {
                            result = ProtocolErrors.ErrorResultNull;
                            listener.Stop();
                        }
                    }
                    else
                    {
                        result = ProtocolErrors.ErrorTcpClientNull;
                        listener.Stop();
                    }
                }
                catch (Exception exp)
                {
                    result = string.IsNullOrEmpty(exp.InnerException.Message) ? exp.Message : exp.Message + " - " + exp.InnerException.Message;
                }
            }
            else
            {
                result = ProtocolErrors.ErrorAutomatic;
            }

            return result;
        }
    }
}
