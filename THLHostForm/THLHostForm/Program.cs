using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModbusDAL;

namespace THLHostForm
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SerialPort serialPort = new SerialPort();
            ModbusService objModbusService = new ModbusService(serialPort);

            var result = new FrmAdminLogin().ShowDialog();
            if (result == DialogResult.OK)
            {
                Application.Run(new FrmMain(objModbusService));
            }
            else
                Application.Exit();
          
        }
        public static string AdminName;
    }
}
