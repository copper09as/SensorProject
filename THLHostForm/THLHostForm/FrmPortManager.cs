using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModbusDAL;


namespace THLHostForm
{
    public partial class FrmPortManager : Form
    {
        private ModbusService objModbusService;
        public FrmPortManager()
        {
            InitializeComponent();
            RefreshPort();
            if(NetManager.socket!=null)
            {
                lblNetState.Text = NetManager.socket.Connected ? "开启" : "关闭";
            }
            for (int i = 1; i < 50; i++)
            {
                cmbBaudRate.Items.Add(600 * i);
                dgvBaudRate.Rows.Add((i*600).ToString());
            }
            NetManager.AddEventListener(NetEvent.Close, TCPClose);
            NetManager.AddEventListener(NetEvent.ConnectSucc, TCPConnect);
            NetManager.AddEventListener(NetEvent.ConnectFail, TCPConnectFail);
        }

        private void TCPConnectFail(string err)
        {
            MessageBox.Show(err);
            lblNetState.Text = "关闭";
        }

        private void TCPConnect(string err)
        {
            MessageBox.Show(err);
            lblNetState.Text = "开启";
        }

        private void TCPClose(string err)
        {
            MessageBox.Show("与服务器连接失败，进入离线模式！", "提示信息");
            lblNetState.Text ="关闭";
        }

        public FrmPortManager(ModbusService objModbusService) : this()
        {
            this.objModbusService = objModbusService;
            lblPortState.Text = !objModbusService.isOpen() ? "关闭" : "开启";
        }


        private void dgvPortName_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = dgvPortName.CurrentRow.Cells["SerialPortName"];
            if (cell != null && cell.Value != null) // 增加空值判断
            {
                string portName = cell.Value.ToString();
                cboPortSelect.Text = portName;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dgvPortName.Rows.Clear();
            cboPortSelect.Items.Clear();
            cboPortSelect.Text = "";
            RefreshPort();
        }

        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            if (cboPortSelect.Text.Trim().Length == 0)
            {
                MessageBox.Show("未选择串口号！", "提示信息");
                return;
            }
            if (cmbBaudRate.Text.Trim().Length == 0)
            {
                MessageBox.Show("未选择波特率！", "提示信息");
                return;
            }
            try
            {
                if (objModbusService.OpenPort(cboPortSelect.Text.Trim(),
                    Convert.ToInt32(cmbBaudRate.Text.Trim())))
                {
                    MessageBox.Show("串口打开成功！", "提示信息");
                    lblPortState.Text = "开启";
                }
                else
                {
                    MessageBox.Show("串口打开失败！", "提示信息");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("串口打开失败！" + ex.Message, "提示信息");
            }
        }

        private void btnClosePort_Click(object sender, EventArgs e)
        {
            try
            {
                if (objModbusService.ClosePort())
                {
                    MessageBox.Show("串口关闭成功！", "提示信息");
                    lblPortState.Text = "关闭";
                }
                else
                {
                    MessageBox.Show("串口关闭失败！", "提示信息");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("串口关闭失败！" + ex.Message, "提示信息");
            }
        }
        private void RefreshPort()
        {
            foreach (var i in SerialPort.GetPortNames())
            {
                cboPortSelect.Items.Add(i);
                dgvPortName.Rows.Add(i);
            }
        }
        private void btnSendQuest_Click(object sender, EventArgs e)
        {
            MessageBox.Show(objModbusService.GetTemperature(0x02).Result[0].ToString());
        }
        private async void ButtonReadTemp_Click(object sender, EventArgs e)
        {
            ushort[] result = await objModbusService.GetTemperature(0x02);
            MessageBox.Show(result.Length > 0 ? (result[0]/10f).ToString() : "读取失败");
        }
        private void dgvBaudRate_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var cell = dgvBaudRate.CurrentRow.Cells["BaudRate"];
            if (cell != null && cell.Value != null)
            {
                string baudRate = cell.Value.ToString();
                cmbBaudRate.Text = baudRate;
            }
        }

        private void FrmPortManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnClosePort_Click(null, null);
        }


        private void btnReadData_Click(object sender, EventArgs e)
        {
            if(!objModbusService.isOpen())
            {
                MessageBox.Show("串口未开启，请开启串口后再进行数据读取！");
                return;
            }
            FrmTHLRead form = new FrmTHLRead(objModbusService);
            form.Show();
        }

        private void btnShowData_Click(object sender, EventArgs e)
        {
            FrmDataShow frmDataShow = new FrmDataShow();
            frmDataShow.Show();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                NetManager.Connect(boxAdd.Text, Convert.ToInt32(boxPort.Text));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
