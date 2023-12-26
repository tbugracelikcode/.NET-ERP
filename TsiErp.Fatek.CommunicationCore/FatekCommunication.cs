using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using TsiErp.Fatek.CommunicationCore.Base;

namespace TsiErp.Fatek.CommunicationCore
{
    public class FatekCommunication : BaseFatekCommunication
    {
        private SerialPort objSerialPort = null;

        /// <summary>
        /// Contructor FATEK Commnucation.
        /// </summary>
        /// <param name="serialPort">Serial Port</param>
        public FatekCommunication(SerialPort serialPort)
        {
            try
            {
                this.objSerialPort = serialPort;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Contructor FATEK Commnucation.
        /// </summary>
        /// <param name="port">Port name</param>
        /// <param name="baudRate">Baud rate</param>
        /// <param name="parity">Parity</param>
        /// <param name="dataBits">DataBits</param>
        /// <param name="stopBits">StopBits</param>
        public FatekCommunication(string port, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            try
            {
                objSerialPort = new SerialPort(port, baudRate, parity, dataBits, stopBits);
                objSerialPort.ReadTimeout = 2000;
                objSerialPort.WriteTimeout = 2000;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// PC connect with PLC FATEK.
        /// </summary>
        public bool Connect()
        {
            try
            {
                if (objSerialPort.IsOpen) objSerialPort.Close();
                objSerialPort.Open();

                return objSerialPort.IsOpen;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// PC disconnect with PLC FATEK.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (objSerialPort.IsOpen) objSerialPort.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool GetDis(MemoryType mType, int startRegister)
        {
            try
            {
                bool[] result = ReadDiscretes(1, (ushort)1, mType, (ushort)startRegister, DataType.BOOL);
                return Convert.ToBoolean(result[0]);
            }
            catch (Exception)
            {
                bool[] result = ReadDiscretes(1, (ushort)1, mType, (ushort)startRegister, DataType.BOOL);
                return Convert.ToBoolean(result[0]);
            }

        }

        public void SetDis(MemoryType mType, int startRegister, RunningCode value)
        {
            try
            {
                WriteSingleDiscrete(1, CommandCode.SINGLE_DISCRETE_CONTROL, value, mType, (ushort)startRegister, DataType.BOOL);
            }
            catch (Exception)
            {
                WriteSingleDiscrete(1, CommandCode.SINGLE_DISCRETE_CONTROL, value, mType, (ushort)startRegister, DataType.BOOL);
            }
        }

        /// <summary>
        ///  Command code 42:  Single discrete control.
        /// </summary>
        /// <param name="slaveStationNo">Slave Station No</param>
        /// <param name="cmmdCode">Command Code</param>
        /// <param name="runCode">Running Code</param>
        /// <param name="mType">Memory Type</param>
        /// <param name="discreteNo">Discrete No</param>
        /// <param name="dType">Data Type</param>
        public void WriteSingleDiscrete(int slaveStationNo, CommandCode cmmdCode, RunningCode runCode, MemoryType mType, ushort discreteNo, DataType dType)
        {
            try
            {
                string frame = this.GetMessageWriteSingleDiscrete(slaveStationNo, cmmdCode, runCode, mType, discreteNo, dType);
                objSerialPort.WriteLine(frame);
                System.Threading.Thread.Sleep(250);

                int sizeOfMessageReceiver = 9;
                string buffReceiver = string.Empty;
                DateTime startDateTime = DateTime.Now;
                do
                {
                    this.CheckBufferReceiver(buffReceiver);
                    buffReceiver = objSerialPort.ReadExisting();
                    System.Threading.Thread.Sleep(100);
                } while (buffReceiver.Length < sizeOfMessageReceiver && (DateTime.Now.Subtract(startDateTime).TotalMilliseconds < objSerialPort.ReadTimeout));
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Discrete PLC'ye yazılamadı. " + mType.ToString() + discreteNo.ToString(), "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        /// <summary>
        /// Command code 44: The status reading of continuous discrete.
        /// </summary>
        /// <param name="slaveStationNo">Slave Station No</param>
        /// <param name="numberOfPoints">Number Of Points.</param>
        /// <param name="mType">Memory Type</param>
        /// <param name="startNo">Start No</param>
        /// <param name="dType">Data Type</param>
        /// <returns>bool[]</returns>
        public bool[] ReadDiscretes(int slaveStationNo, ushort numberOfPoints, MemoryType mType, ushort startNo, DataType dType)
        {
            List<bool> result = new List<bool>();
            try
            {
                string frame = this.GetMessageReadDiscretes(slaveStationNo, CommandCode.THE_STATUS_READING_OF_CONTINUOUS_DISCRETE, numberOfPoints, mType, startNo, dType);
                objSerialPort.WriteLine(frame);
                System.Threading.Thread.Sleep(250);

                int sizeOfMessageReceiver = numberOfPoints + 9;
                string buffReceiver = string.Empty;
                DateTime startDateTime = DateTime.Now;
                do
                {
                    this.CheckBufferReceiver(buffReceiver);
                    buffReceiver = objSerialPort.ReadExisting();
                    System.Threading.Thread.Sleep(250);
                } while (buffReceiver.Length < sizeOfMessageReceiver && (DateTime.Now.Subtract(startDateTime).TotalMilliseconds < objSerialPort.ReadTimeout));

                string dataReceiver = buffReceiver.Substring(6, numberOfPoints);
                foreach (char item in dataReceiver)
                {
                    result.Add(!"0".Equals(item.ToString()) ? true : false);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Discrete PLC'den okunamadı. " + mType.ToString() + startNo.ToString(), "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return result.ToArray();
        }

        //public Operasyon GetAllRegisters(Operasyon operasyon)
        //{
        //    try
        //    {
        //        string OprBaslangicTRH = "";

        //        operasyon.ID = GetItem(MemoryType.DR, 304);
        //        operasyon.IsEmriID = GetItem(MemoryType.DR, 312);
        //        operasyon.UretimEmriID = GetItem(MemoryType.DR, 314);
        //        operasyon.SiparisID = GetItem(MemoryType.DR, 316);
        //        operasyon.StokID = GetItem(MemoryType.DR, 320);
        //        operasyon.VaryantID = GetItem(MemoryType.DR, 322);
        //        operasyon.RotaID = GetItem(MemoryType.DR, 324);
        //        operasyon.OperasyonID = GetItem(MemoryType.DR, 302);
        //        operasyon.PlanlananAdet = GetItem(MemoryType.DR, 116);
        //        operasyon.GerceklesenAdet = GetItem(MemoryType.DR, 150);
        //        operasyon.KalanAdet = GetItem(MemoryType.DR, 118);

        //        operasyon.operasyonSatir.ID = GetItem(MemoryType.DR, 306);
        //        operasyon.operasyonSatir.HurdaAdet = GetItem(MemoryType.DR, 300);
        //        operasyon.operasyonSatir.CalisanID = GetItem(MemoryType.DR, 122);
        //        operasyon.operasyonSatir.VardiyaID = GetItem(MemoryType.DR, 124);
        //        operasyon.operasyonSatir.Kalite = GetItem(MemoryType.DR, 310);
        //        operasyon.operasyonSatir.UretilenAdet = GetItem(MemoryType.DR, 106);
        //        operasyon.operasyonSatir.AtılSure = GetItem(MemoryType.DR, 328);
        //        operasyon.operasyonSatir.OperasyonID = GetItem(MemoryType.DR, 304);
        //        operasyon.operasyonSatir.Agirlik = GetItem(MemoryType.DR, 326);
        //        operasyon.operasyonSatir.IsEmriID = operasyon.IsEmriID;
        //        operasyon.operasyonSatir.StokID = operasyon.StokID;
        //        operasyon.operasyonSatir.Mesai = GetItem(MemoryType.DR, 126);

        //        OprBaslangicTRH = GetItem(MemoryType.D, 22).ToString() + ".";
        //        OprBaslangicTRH = OprBaslangicTRH + GetItem(MemoryType.D, 21).ToString() + ".";
        //        OprBaslangicTRH = OprBaslangicTRH + GetItem(MemoryType.D, 20).ToString() + " ";

        //        OprBaslangicTRH = OprBaslangicTRH + GetItem(MemoryType.D, 23).ToString() + ":";
        //        OprBaslangicTRH = OprBaslangicTRH + GetItem(MemoryType.D, 24).ToString() + ":";
        //        OprBaslangicTRH = OprBaslangicTRH + GetItem(MemoryType.D, 25).ToString();

        //        operasyon.operasyonSatir.OprBaslangicTRH = Convert.ToDateTime(OprBaslangicTRH);

        //        return operasyon;
        //    }
        //    catch (Exception exp)
        //    {
        //        //MessageBox.Show(exp.Message, "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return null;
        //    }
        //}

        public int GetItem(MemoryType mType, int startRegister)
        {
            try
            {
                int back = 0;
                object[] result = ReadRegisters(1, (ushort)1, mType, (ushort)startRegister, DataType.WORD);
                back = Convert.ToInt32(result[0]);
                return back;
            }
            catch (Exception)
            {
                int back = 0;
                object[] result = ReadRegisters(1, (ushort)1, mType, (ushort)startRegister, DataType.WORD);
                back = Convert.ToInt32(result[0]);
                return back;
            }
        }

        public int GetSignedItem(MemoryType mType, int startRegister)
        {
            try
            {
                int back = 0;
                object[] result = ReadSignedRegisters(1, (ushort)1, mType, (ushort)startRegister, DataType.WORD);
                back = Convert.ToInt32(result[0]);
                return back;
            }
            catch (Exception)
            {
                int back = 0;
                object[] result = ReadSignedRegisters(1, (ushort)1, mType, (ushort)startRegister, DataType.WORD);
                back = Convert.ToInt32(result[0]);
                return back;
            }
        }

        public void SetItem(MemoryType mType, int startRegister, int item)
        {
            try
            {
                WriteMultipeRegisters(1, (ushort)1, mType, (ushort)startRegister, DataType.WORD, item);
                if (item != GetItem(mType, startRegister))
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                WriteMultipeRegisters(1, (ushort)1, mType, (ushort)startRegister, DataType.WORD, item);
            }
        }

        /// <summary>
        /// Command code 46:  Read the data from continuous registers.
        /// </summary>
        /// <param name="slaveStationNo">Slave Station No</param>        
        /// <param name="numberOfPoints">Number Of Points.</param>
        /// <param name="mType">Memory Type.</param>
        /// <param name="startRegister">Start register No(6 or 7 words)</param>
        /// <param name="dType">Data Type.</param>
        /// <returns>object[]</returns>
        public object[] ReadRegisters(int slaveStationNo, ushort numberOfPoints, MemoryType mType, ushort startRegister, DataType dType)
        {
            List<object> result = new List<object>();
            try
            {

                string frame = this.GetMessageReadRegisters(slaveStationNo, CommandCode.READ_THE_DATA_FROM_CONTINUOUS_REGISTERS, numberOfPoints, mType, startRegister, dType);
                objSerialPort.WriteLine(frame);
                System.Threading.Thread.Sleep(250);


                // Read SerialPort
                int CONST = 4;
                switch (dType)
                {
                    case DataType.INT:
                    case DataType.WORD:
                        CONST = 4;
                        break;
                    case DataType.DINT:
                    case DataType.DWORD:
                    case DataType.REAL:
                        CONST = 8;
                        break;
                    default:
                        throw new InvalidOperationException("Data type is invalid");
                }
                int sizeOfMessageReceiver = numberOfPoints * CONST + 9;
                string buffReceiver = string.Empty;
                DateTime startDateTime = DateTime.Now;
                do
                {
                    this.CheckBufferReceiver(buffReceiver);
                    System.Threading.Thread.Sleep(100);
                    buffReceiver = objSerialPort.ReadExisting();
                } while (buffReceiver.Length < sizeOfMessageReceiver && (DateTime.Now.Subtract(startDateTime).TotalMilliseconds < objSerialPort.ReadTimeout));

                string dataReceiver = string.Empty;
                string valueString = string.Empty;
                switch (mType)
                {
                    case MemoryType.R:
                        dataReceiver = buffReceiver.Substring(6, 4).PadLeft(8, '0');
                        result.Add(Conversion.HEXToDINT(dataReceiver));
                        break;
                    case MemoryType.D:
                        dataReceiver = buffReceiver.Substring(6, 4).PadLeft(8, '0');
                        result.Add(Conversion.HEXToDINT(dataReceiver));
                        break;
                    case MemoryType.DR:
                        dataReceiver = buffReceiver.Substring(6, 8);
                        result.Add(Conversion.HEXToDINT(dataReceiver));
                        break;
                    case MemoryType.DD:
                        dataReceiver = buffReceiver.Substring(6, 8);
                        result.Add(Conversion.HEXToDINT(dataReceiver));
                        break;
                    case MemoryType.T:
                        dataReceiver = buffReceiver.Substring(6, 4).PadLeft(8, '0');
                        result.Add(Conversion.HEXToDINT(dataReceiver));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Register PLC'den okunamadı. " + mType.ToString() + startRegister.ToString(), "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            return result.ToArray();
        }

        public object[] ReadSignedRegisters(int slaveStationNo, ushort numberOfPoints, MemoryType mType, ushort startRegister, DataType dType)
        {
            List<object> result = new List<object>();
            try
            {

                string frame = this.GetMessageReadRegisters(slaveStationNo, CommandCode.READ_THE_DATA_FROM_CONTINUOUS_REGISTERS, numberOfPoints, mType, startRegister, dType);
                objSerialPort.WriteLine(frame);
                System.Threading.Thread.Sleep(250);


                // Read SerialPort
                int CONST = 4;
                switch (dType)
                {
                    case DataType.INT:
                    case DataType.WORD:
                        CONST = 4;
                        break;
                    case DataType.DINT:
                    case DataType.DWORD:
                    case DataType.REAL:
                        CONST = 8;
                        break;
                    default:
                        throw new InvalidOperationException("Data type is invalid");
                }
                int sizeOfMessageReceiver = numberOfPoints * CONST + 9;
                string buffReceiver = string.Empty;
                DateTime startDateTime = DateTime.Now;
                do
                {
                    this.CheckBufferReceiver(buffReceiver);
                    buffReceiver = objSerialPort.ReadExisting();
                    System.Threading.Thread.Sleep(100);
                } while (buffReceiver.Length < sizeOfMessageReceiver && (DateTime.Now.Subtract(startDateTime).TotalMilliseconds < objSerialPort.ReadTimeout));

                string dataReceiver = string.Empty;
                string valueString = string.Empty;
                switch (mType)
                {
                    case MemoryType.R:
                        dataReceiver = buffReceiver.Substring(6, 4);
                        string hhh = Convert.ToString(Convert.ToInt32(dataReceiver, 16), 2);
                        var newttt = ~(Convert.ToInt16(hhh, 2)) + 1;
                        int asd = -newttt;
                        result.Add(asd);
                        break;
                    case MemoryType.D:
                        dataReceiver = buffReceiver.Substring(6, 4).PadLeft(8, '0');
                        result.Add(Conversion.HEXToDINT(dataReceiver));
                        break;
                    case MemoryType.DR:
                        dataReceiver = buffReceiver.Substring(6, 8);
                        result.Add(Conversion.HEXToDINT(dataReceiver));
                        break;
                    case MemoryType.DD:
                        dataReceiver = buffReceiver.Substring(6, 8);
                        result.Add(Conversion.HEXToDINT(dataReceiver));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Register PLC'den okunamadı. " + mType.ToString() + startRegister.ToString(), "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            return result.ToArray();
        }

        /// <summary>
        /// Command code 47: Write to continuous registers.
        /// </summary>
        /// <param name="slaveStationNo">Slave Station No</param>
        /// <param name="numberOfPoints">Number Of Points.</param>
        /// <param name="mType">Memory Type</param>
        ///  <param name="startRegister">Start register No(6 or 7 words)</param>
        ///  <param name="dType">Data Type</param>
        /// <param name="data">Data of registers</param>
        public void WriteMultipeRegisters(int slaveStationNo, ushort numberOfPoints, MemoryType mType, ushort startRegister, DataType dType, int data)
        {
            try
            {
                string frame = this.GetMessageWriteMultipeRegisters(slaveStationNo, CommandCode.WRITE_TO_CONTINUOUS_REGISTERS, numberOfPoints, mType, startRegister, dType, data);
                objSerialPort.WriteLine(frame);
                System.Threading.Thread.Sleep(250);

                int sizeOfMessageReceiver = 9;
                string buffReceiver = string.Empty;
                DateTime startDateTime = DateTime.Now;
                do
                {
                    this.CheckBufferReceiver(buffReceiver);
                    buffReceiver = objSerialPort.ReadExisting();
                    System.Threading.Thread.Sleep(100);
                } while (buffReceiver.Length < sizeOfMessageReceiver && (DateTime.Now.Subtract(startDateTime).TotalMilliseconds < objSerialPort.ReadTimeout));

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Register PLC'ye yazılamadı. " + mType.ToString() + startRegister.ToString(), "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        #region İleride LAzım Olabilecek Fonksiyonlar

        ///// <summary>
        ///// Command code 48:  Mixed read the random discrete status or register data.
        ///// </summary>
        ///// <param name="slaveStationNo">Slave Station No</param>
        ///// <param name="numberOfPoints">Number Of Points.</param>
        ///// <param name="components">List<RandomRegister></param>
        ///// <returns>string</returns>
        //public RandomRegister[] ReadRadomDiscreteOrRegisters(int slaveStationNo, ushort numberOfPoints, List<RandomRegister> components)
        //{
        //    List<RandomRegister> result = new List<RandomRegister>();
        //    try
        //    {
        //        string frame = this.GetMessageReadRadomDiscreteOrRegisters(slaveStationNo, CommandCode.MIXED_READ_THE_RADOM_DISCRETE_STATUS_OF_REGISTER_DATA, numberOfPoints, components);
        //        objSerialPort.WriteLine(frame);


        //        // Read SerialPort
        //        int CONST = 0;
        //        foreach (RandomRegister item in components)
        //        {
        //            switch (item.DataType)
        //            {
        //                case DataType.BOOL:
        //                    CONST += 1;
        //                    break;
        //                case DataType.INT:
        //                case DataType.WORD:
        //                    CONST += 4;
        //                    break;
        //                case DataType.DINT:
        //                case DataType.DWORD:
        //                case DataType.REAL:
        //                    CONST += 8;
        //                    break;
        //                default:
        //                    throw new InvalidOperationException("Data type is invalid");
        //            }
        //        }

        //        int sizeOfMessageReceiver = CONST + 9;
        //        string buffReceiver = string.Empty;
        //        DateTime startDateTime = DateTime.Now;
        //        do
        //        {
        //            this.CheckBufferReceiver(buffReceiver);
        //            buffReceiver = objSerialPort.ReadExisting();
        //            System.Threading.Thread.Sleep(100);
        //        } while (buffReceiver.Length < sizeOfMessageReceiver && (DateTime.Now.Subtract(startDateTime).TotalMilliseconds < objSerialPort.ReadTimeout));
        //        //Console.WriteLine("ReadTimeout: {0}", DateTime.Now.Subtract(startDateTime).TotalMilliseconds);
        //        //string errorMsg = this.GetException(buffReceiver);
        //        //if (!string.IsNullOrEmpty(errorMsg) || !string.IsNullOrWhiteSpace(errorMsg))
        //        //    MessageBox.Show(errorMsg);
        //        //throw new Exception(errorMsg);

        //        int index = 0;
        //        string dataReceiver = string.Empty;
        //        string valueString = string.Empty;

        //        dataReceiver = buffReceiver.Substring(6, CONST); // data.

        //        foreach (RandomRegister item in components)
        //        {
        //            switch (item.DataType)
        //            {
        //                case DataType.BOOL:
        //                    valueString = dataReceiver.Substring(index, 1);
        //                    item.Value = !"0".Equals(valueString) ? true : false;
        //                    index += 1;
        //                    break;
        //                case DataType.INT:
        //                    valueString = dataReceiver.Substring(index, 4);
        //                    item.Value = Conversion.HEXToINT(valueString);
        //                    index += 4;
        //                    break;
        //                case DataType.WORD:
        //                    valueString = dataReceiver.Substring(index, 4);
        //                    item.Value = Conversion.HEXToWORD(valueString);
        //                    index += 4;
        //                    break;
        //                case DataType.DINT:
        //                    valueString = dataReceiver.Substring(index, 8);
        //                    item.Value = Conversion.HEXToDINT(valueString);
        //                    index += 8;
        //                    break;
        //                case DataType.DWORD:
        //                    valueString = dataReceiver.Substring(index, 8);
        //                    item.Value = Conversion.HEXToDWORD(valueString);
        //                    index += 8;
        //                    break;
        //                case DataType.REAL:
        //                    valueString = dataReceiver.Substring(index, 8);
        //                    item.Value = Conversion.HEXToREAL(valueString);
        //                    index += 8;
        //                    break;
        //                default:
        //                    throw new InvalidOperationException("Data type is invalid");
        //            }
        //            result.Add(item);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result.ToArray();
        //}

        ///// <summary>
        ///// FATEK PLC Information
        ///// </summary>
        ///// <param name="slaveStationNo">Slave Station No</param>
        ///// <returns>FATEK PLC Information</returns>
        //public string GetPLCInfo(int slaveStationNo)
        //{
        //    StringBuilder result = new StringBuilder();
        //    try
        //    {
        //        string frame = this.GetMessageOfPLCStatus(slaveStationNo, CommandCode.PLC_STATUS);
        //        objSerialPort.WriteLine(frame);

        //        // Read SerialPort
        //        int sizeOfMessageReceiver = 15;
        //        string buffReceiver = string.Empty;
        //        DateTime startDateTime = DateTime.Now;
        //        do
        //        {
        //            this.CheckBufferReceiver(buffReceiver);
        //            buffReceiver = objSerialPort.ReadExisting();
        //            System.Threading.Thread.Sleep(100);
        //        } while (buffReceiver.Length < sizeOfMessageReceiver && (DateTime.Now.Subtract(startDateTime).TotalMilliseconds < objSerialPort.ReadTimeout));
        //        //string errorMsg = this.GetException(buffReceiver);
        //        //if (!string.IsNullOrEmpty(errorMsg) || !string.IsNullOrWhiteSpace(errorMsg))
        //        //    MessageBox.Show(errorMsg);
        //        //throw new Exception(errorMsg);

        //        string dataReceiver = buffReceiver.Substring(6, 2); // STATUS 1.
        //        ushort plcStatus = Conversion.HEXToWORD(dataReceiver);

        //        result.AppendLine("FATEK PLC COMMUNICATION PROTOCOL VIA SERIALPORT");
        //        result.AppendLine("===============================================================");
        //        PLCMode objPLCMode = (PLCMode)plcStatus;
        //        result.AppendLine(string.Format("B0. PLC Mode: {0}", objPLCMode));

        //        LADDER_CHECKSUM ladder = (LADDER_CHECKSUM)((plcStatus & (ushort)Math.Pow(2, 2)) >> 2);
        //        result.AppendLine(string.Format("B2. Ladder Checksum: {0}", ladder));

        //        USE_ROM_PACK_OR_NOT_USE objUseRomPack = (USE_ROM_PACK_OR_NOT_USE)((plcStatus & (ushort)Math.Pow(2, 3)) >> 3);
        //        result.AppendLine(string.Format("B3. Use ROM Pack: {0}", objUseRomPack));

        //        WDT_TIMEOUT_OR_NORMAL objWDTTimeout = (WDT_TIMEOUT_OR_NORMAL)((plcStatus & (ushort)Math.Pow(2, 4)) >> 4);
        //        result.AppendLine(string.Format("B4. WDT Timeout: {0}", objWDTTimeout));

        //        SET_ID_OR_NOT_SET_ID objSetID = (SET_ID_OR_NOT_SET_ID)((plcStatus & (ushort)Math.Pow(2, 5)) >> 5);
        //        result.AppendLine(string.Format("B5. Set ID: {0}", objSetID));

        //        EMERGENCY_STOP_OR_NORMAL objEmergencyStop = (EMERGENCY_STOP_OR_NORMAL)((plcStatus & (ushort)Math.Pow(2, 6)) >> 6);
        //        result.AppendLine(string.Format("B6. Emergency Stop: {0}", objEmergencyStop));

        //        //RESERVE FOR FUTURE
        //        result.AppendLine("B7. Reserver For Future");
        //        result.AppendLine("===============================================================");
        //        result.AppendLine("Designed By Industrial Networks");
        //        result.AppendLine("Skype: katllu");
        //        result.AppendLine("Mobile: (+84) 909.886.483");
        //        result.AppendLine("E-mail: hoangluu.automation@gmail.com");
        //        result.AppendLine("Youtube: https://www.youtube.com/industrialnetworks");
        //        result.AppendLine();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result.ToString();
        //}

        ///// <summary>
        ///// Command code 40: The gist read the system status of PLC.
        ///// </summary>
        ///// <param name="slaveStationNo">Slave Station No</param>
        ///// <returns>bool</returns>
        //public PLCMode GetPLCStatus(int slaveStationNo)
        //{
        //    PLCMode result = PLCMode.STOP;
        //    try
        //    {
        //        string frame = this.GetMessageOfPLCStatus(slaveStationNo, CommandCode.PLC_STATUS);
        //        objSerialPort.WriteLine(frame);

        //        // Read SerialPort
        //        int sizeOfMessageReceiver = 15;
        //        string buffReceiver = string.Empty;
        //        DateTime startDateTime = DateTime.Now;
        //        do
        //        {
        //            this.CheckBufferReceiver(buffReceiver);
        //            buffReceiver = objSerialPort.ReadExisting();
        //            System.Threading.Thread.Sleep(100);
        //        } while (buffReceiver.Length < sizeOfMessageReceiver && (DateTime.Now.Subtract(startDateTime).TotalMilliseconds < objSerialPort.ReadTimeout));
        //        //string errorMsg = this.GetException(buffReceiver);
        //        //if (!string.IsNullOrEmpty(errorMsg) || !string.IsNullOrWhiteSpace(errorMsg))
        //        //    MessageBox.Show(errorMsg);
        //        //throw new Exception(errorMsg);

        //        string dataReceiver = buffReceiver.Substring(6, 2); // STATUS 1.
        //        ushort plcStatus = Conversion.HEXToWORD(dataReceiver);
        //        result = (PLCMode)plcStatus;
        //        //Console.WriteLine("PLC STATUS = " + dataReceiver);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Command code 41: Control RUN/STOP of PLC.
        ///// </summary>
        ///// <param name="slaveStationNo">Slave Station No</param>
        ///// <param name="status">Control Code, 1: RUN, 0: STOP</param>
        ///// <returns>string</returns>
        //public bool SetPLCMode(int slaveStationNo, PLCMode status)
        //{
        //    bool result = false;
        //    try
        //    {
        //        string frame = this.GetMessageOfControlPLC(slaveStationNo, CommandCode.CONTROL_RUN_STOP, status);
        //        objSerialPort.WriteLine(frame);

        //        // Read SerialPort
        //        int sizeOfMessageReceiver = 9;
        //        string buffReceiver = string.Empty;
        //        DateTime startDateTime = DateTime.Now;
        //        do
        //        {
        //            this.CheckBufferReceiver(buffReceiver);
        //            buffReceiver = objSerialPort.ReadExisting();
        //            System.Threading.Thread.Sleep(100);
        //        } while (buffReceiver.Length < sizeOfMessageReceiver && (DateTime.Now.Subtract(startDateTime).TotalMilliseconds < objSerialPort.ReadTimeout));
        //        //string errorMsg = this.GetException(buffReceiver);
        //        //if (!string.IsNullOrEmpty(errorMsg) || !string.IsNullOrWhiteSpace(errorMsg))
        //        //    MessageBox.Show(errorMsg);
        //        //throw new Exception(errorMsg);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Command code 43: The status reading of ENABLE/DISABLE of continuous discrete.
        ///// </summary>
        ///// <param name="slaveStationNo">Slave Station No</param>
        ///// <param name="numberOfPoints">Number Of Points.</param>
        ///// <param name="mType">Memory Type</param>
        ///// <param name="startNo">Start No</param>
        ///// <param name="dType">Data Type</param>
        //public void ReadEnableOrDisable(int slaveStationNo, CommandCode cmmdCode, ushort numberOfPoints, MemoryType mType, ushort startNo, DataType dType)
        //{
        //    try
        //    {
        //        string frame = this.GetMessageReadDiscretes(slaveStationNo, cmmdCode, numberOfPoints, mType, startNo, dType);
        //        objSerialPort.WriteLine(frame);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// Command code 45: Write the status to continuous discrete.
        ///// </summary>
        ///// <param name="slaveStationNo">Slave Station No</param>
        ///// <param name="numberOfPoints">Number Of Points.</param>
        ///// <param name="mType">Memory Type</param>
        ///// <param name="startNo">Start No</param>
        ///// <param name="dType">Data Type</param>
        ///// <param name="data">Status</param>    
        //public void WriteMultipeDiscretes(int slaveStationNo, ushort numberOfPoints, MemoryType mType, ushort startNo, DataType dType, bool[] data)
        //{
        //    try
        //    {
        //        string frame = this.GetMessageWriteMultipeDiscretes(slaveStationNo, CommandCode.WRITE_THE_STATUS_TO_CONTINUOUS_DISCRETE, numberOfPoints, mType, startNo, dType, data);
        //        objSerialPort.WriteLine(frame);

        //        int sizeOfMessageReceiver = 9;
        //        string buffReceiver = string.Empty;
        //        DateTime startDateTime = DateTime.Now;
        //        do
        //        {
        //            this.CheckBufferReceiver(buffReceiver);
        //            buffReceiver = objSerialPort.ReadExisting();
        //            System.Threading.Thread.Sleep(100);
        //        } while (buffReceiver.Length < sizeOfMessageReceiver && (DateTime.Now.Subtract(startDateTime).TotalMilliseconds < objSerialPort.ReadTimeout));
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public int GetSpecialItem(MemoryType mType, int startRegister)
        //{
        //    bool check = true;
        //    int back = 0;
        //    do
        //    {
        //        object[] result = ReadRegisters(1, (ushort)2, mType, (ushort)startRegister, DataType.WORD);
        //        if (result.Length != 0)
        //        {
        //            back = Convert.ToInt32(result[1]);
        //            check = false;
        //        }
        //    } while (check);
        //    return back;
        //}

        //public int GetSpecialRevItem(MemoryType mType, int startRegister)
        //{
        //    bool check = true;
        //    int back = 0;
        //    do
        //    {
        //        object[] result = ReadRegisters(1, (ushort)2, mType, (ushort)startRegister, DataType.WORD);
        //        if (result.Length != 0)
        //        {
        //            back = Convert.ToInt32(result[0]);
        //            check = false;
        //        }
        //    } while (check);
        //    return back;
        //}

        #endregion
    }
}
