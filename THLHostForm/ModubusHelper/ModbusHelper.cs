using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Modbus.Device;
namespace ModbusDAL
{
    public class ModbusHelper
    {
        private SerialPort objSerialPort;

        private IModbusMaster master;
        public ModbusHelper(SerialPort objSerialPort)
        {
            this.objSerialPort = objSerialPort;

        }
        public ushort[] SendRequest(byte slaveId,ushort startAddress, ushort numRegisters)
        {
            return master.ReadHoldingRegisters(slaveId, startAddress, numRegisters);
        }


        #region 串口读写关闭

        private bool Write(byte[] message)

        {
            if (!objSerialPort.IsOpen)
                return false;
            try
            {
                objSerialPort.Write(message, 0, message.Length);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private string Read()
        {
            if (!objSerialPort.IsOpen)
                return "false";
            try
            {
                return objSerialPort.ReadExisting();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool ClosePort()
        {
            try
            {
                if (objSerialPort.IsOpen)
                {
                    objSerialPort.Close();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool OpenPort(string portName,int baudRate)
        {
            
            try
            {
                if (!objSerialPort.IsOpen)
                {
                    objSerialPort.PortName = portName;
                    objSerialPort.BaudRate = baudRate;
                    objSerialPort.Parity = Parity.None;
                    objSerialPort.DataBits = 8;
                    objSerialPort.StopBits = StopBits.One;
                    objSerialPort.ReadTimeout = 1000;
                    objSerialPort.WriteTimeout = 1000; // 500ms 超时
                    master = ModbusSerialMaster.CreateRtu(objSerialPort);
                    master.Transport.ReadTimeout = 1000;
                    master.Transport.Retries = 1;
                    objSerialPort.Open();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        #endregion
    }
}
