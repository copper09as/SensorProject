using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ModbusDAL
{
    public class ModbusService
    {
        private SerialPort objSerialPort;
        private ModbusHelper objModusHelper;
        public ModbusService(SerialPort objSerialPort)
        {
            this.objSerialPort = objSerialPort;
            objModusHelper = new ModbusHelper(this.objSerialPort);
        }

        public bool OpenPort(string portName, int baudRate)
        {
            try
            {
                return objModusHelper.OpenPort(portName, baudRate);
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
                return objModusHelper.ClosePort();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool isOpen() => objSerialPort.IsOpen;
        public async Task<ushort[]> GetTemperature(byte slaveId)
        {
            return await Task.Run(() =>
            {
                return objModusHelper.SendRequest(slaveId,0,1);
            });
        }

        public async Task<ushort[]> GetTH(byte slaveId)
        {
            return await Task.Run(() =>
            {
                return objModusHelper.SendRequest(slaveId, 0, 2);
            });
        }
        public async Task<ushort[]> GetLight(byte slaveId)
        {
            return await Task.Run(() =>
            {
                return objModusHelper.SendRequest(slaveId, 2, 2);
            });
        }
        public bool ChangeAddress
            (byte originAddress,byte currentAddress,SerialDataReceivedEventHandler readBack)
        {
            var list = new List<byte>
            {
                0x00,0x64,0x00,currentAddress
            };
            try
            {
                objSerialPort.DataReceived += readBack;

                return true;
            }
            catch (Exception ex)
            {
                objSerialPort.DataReceived -= readBack;
                throw ex;
            }
        }
    }
}
