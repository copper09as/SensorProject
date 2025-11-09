using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModbusDAL;
using Newtonsoft.Json;

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
            MsgThlAnaly analy = new MsgThlAnaly
            {
                AnalyResult = "温湿度光照正常"
            };

            // 序列化为 JSON 字符串（格式化输出）
            string json = JsonConvert.SerializeObject(analy, Formatting.Indented);

            // 保存到文件
            string filePath = "C:\\Users\\49469\\Desktop\\analy.json";
            File.WriteAllText(filePath, json);

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
