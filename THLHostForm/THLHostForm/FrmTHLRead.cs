using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using ModbusDAL;
using Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace THLHostForm
{
    public partial class FrmTHLRead : Form
    {
        private ModbusService objModbusService;
        private THLDataService objThlDataService = new THLDataService();
        private bool close = false;
        public FrmTHLRead(ModbusService objModbusService)
        {
            InitializeComponent();
            this.objModbusService = objModbusService;
        }



        private void UpdateUi(float temperature, float humidity, float light,DateTime time)
        {
            chart1.Series["Temperature"].Points.AddXY(time, temperature);
            chart2.Series["Humidity"].Points.AddXY(time,humidity);
            chart3.Series["Light"].Points.AddXY(time, light);
            lblTemperature.Text = temperature.ToString("F1") + "°";
            dgvTH.Rows.Add(temperature, humidity,light,time);
            lblHum.Text = humidity.ToString()+"%";
            lblLight.Text = light.ToString()+"(Lux)";
        }


        private async void timer1_Tick(object sender, EventArgs e)
        {
            //objModbusService.GetTH(0x02);
            ushort[] result = await objModbusService.GetTH(0x02);
            //MessageBox.Show(result.Length > 0 ? (result[0] / 10f).ToString() : "读取失败");
            float temperature = result[0] / 10f;
            float humidity = result[1] / 10f;
            ushort[] lightResult = await objModbusService.GetLight(0x01);
            ushort high = lightResult[0]; // 0x000C
            ushort low = lightResult[1]; // 0xF473
            uint value = ((uint)high << 16) | low;
            float light = value / 1000f;
            DateTime time = DateTime.Now;
            THLData data = new THLData
            {
                DTime = time,
                Temperature = temperature,
                Humidity = humidity,
                Light = light
            };
            MsgForward msgForward = new MsgForward();
            msgForward.ThlData = data;
            try
            {
                if(!close)
                    NetManager.Send(msgForward);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                btnReset.Text = "停止监测";
                timer1.Enabled = false;
                return;
            }
            try
            {
                objThlDataService.AddTHLData(data);
                UpdateUi(temperature, humidity, light, time);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                btnReset_Click(null, null);
                timer1.Enabled = false;
                return;
            }
        }

        private void FrmTHLRead_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            MsgSensorState msgSensorState = new MsgSensorState();
            if (timer1.Enabled)
            {
                btnReset.Text = "开始监测";

                msgSensorState.SensorState = 1;
            }
            else
            {
                btnReset.Text = "停止监测";
                msgSensorState.SensorState = 0;
            }
            NetManager.Send(msgSensorState);
            timer1.Enabled = !timer1.Enabled;
        }

        private void FrmTHLRead_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("确认退出吗？", "退出询问",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                timer1.Enabled = false;
                e.Cancel = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {   MsgSensorState msgSensorState = new MsgSensorState();
            msgSensorState.SensorState = 0;
            NetManager.Send(msgSensorState);
            Close();
        }
    }
}
