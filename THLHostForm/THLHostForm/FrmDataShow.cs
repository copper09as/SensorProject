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
using Models;
using Excel = Microsoft.Office.Interop.Excel;
namespace THLHostForm
{
    public partial class FrmDataShow : Form
    {
        private THLDataService objThlDataService = new THLDataService();
        private List<THLData> thlDataList = new List<THLData>();
        public FrmDataShow()
        {
            InitializeComponent();
            startDtp.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            endDtp.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            chart1.Series["Temperature"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";

            // 湿度图表
            chart2.Series["Humidity"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";

            // 光照图表
            chart3.Series["Light"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            chart3.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
        }
        private void UpdateUi(THLData thlData)
        {
            var time = thlData.DTime;
            var temperature = thlData.Temperature;
            var humidity = thlData.Humidity;
            var light = thlData.Light;
            chart1.Series["Temperature"].Points.AddXY(time, temperature);
            chart2.Series["Humidity"].Points.AddXY(time, humidity);
            chart3.Series["Light"].Points.AddXY(time, light);
            dgvTH.Rows.Add(temperature, humidity, light, time);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime startTime = Convert.ToDateTime(startDtp.Text);
            DateTime endTime = Convert.ToDateTime(endDtp.Text);
            chart1.Series["Temperature"].Points.Clear();
            chart2.Series["Humidity"].Points.Clear();
            chart3.Series["Light"].Points.Clear();
            dgvTH.Rows.Clear();
            try
            {
                thlDataList = objThlDataService.ShowThlData(startTime, endTime);
                foreach(var i in thlDataList)
                {
                    UpdateUi(i);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void btnToExcel_Click(object sender, EventArgs e)
        {
            if (thlDataList == null || thlDataList.Count == 0)
            {
                MessageBox.Show("没有数据可导出！");
                return;
            }

            try
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false; // 不显示界面，速度更快
                var workbook = excelApp.Workbooks.Add(Type.Missing);
                var worksheet = (Excel.Worksheet)workbook.Sheets[1];
                worksheet.Name = "THLData";

                // 写入表头
                string[] headers = { "Temperature", "Humidity", "Light", "DTime" };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1] = headers[i];
                }

                // 准备二维数组，一次性写入
                object[,] dataArr = new object[thlDataList.Count, headers.Length];
                for (int i = 0; i < thlDataList.Count; i++)
                {
                    dataArr[i, 0] = thlDataList[i].Temperature;
                    dataArr[i, 1] = thlDataList[i].Humidity;
                    dataArr[i, 2] = thlDataList[i].Light;
                    dataArr[i, 3] = thlDataList[i].DTime.ToString();
                }

                // 写入 Excel
                Excel.Range startCell = (Excel.Range)worksheet.Cells[2, 1];
                Excel.Range endCell = (Excel.Range)worksheet.Cells[thlDataList.Count + 1, headers.Length];
                Excel.Range writeRange = worksheet.Range[startCell, endCell];
                writeRange.Value2 = dataArr;

                // 自动调整列宽
                worksheet.Columns.AutoFit();

                // 保存文件
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\THLData.xlsx";
                workbook.SaveAs(filePath);
                workbook.Close();
                excelApp.Quit();

                MessageBox.Show($"导出成功！文件路径：{filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败: " + ex.Message);
            }
        }

    }
}
