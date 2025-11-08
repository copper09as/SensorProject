using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ModbusDAL;
using Models;
using Newtonsoft.Json;

namespace THLHostForm
{
    public partial class FrmMain : Form
    {
        private ModbusService objModbusService;
        private CancellationTokenSource _netCts;
        public FrmMain(ModbusService objModbusService)
        {
            InitializeComponent();
            this.objModbusService = objModbusService;
            OpenForm(new FrmPortManager(objModbusService));
            adminName.Text = Program.AdminName;
            Task.Run(() => NetLoop(_netCts.Token));
        }

        private async Task NetLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    NetManager.Update();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("[NetLoop] " + ex.Message);
                }
                await Task.Delay(10);
            }
        }


        private void CloseForm()
        {
            foreach (Control item in splitContainer.Panel2.Controls)
            {
                if (item is Form)
                {
                    Form form = (Form)item;
                    form.Close();
                }
            }
        }
        private void OpenForm(Form objFrm)
        {
            CloseForm();
            objFrm.TopLevel = false;
            objFrm.FormBorderStyle = FormBorderStyle.None;
            objFrm.Parent = splitContainer.Panel2;
            objFrm.Dock = DockStyle.Fill;
            objFrm.Show();
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            var result = MessageBox.Show("确认退出吗？", "退出询问",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result != DialogResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
