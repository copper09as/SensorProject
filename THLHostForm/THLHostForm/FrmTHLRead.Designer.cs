namespace THLHostForm
{
    partial class FrmTHLRead
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea10 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend10 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea11 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend11 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea12 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend12 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lblTemperature = new System.Windows.Forms.Label();
            this.dgvTH = new System.Windows.Forms.DataGridView();
            this.lblHum = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.Temperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.humidity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Light = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblLight = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea10.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea10);
            legend10.Name = "Legend1";
            this.chart1.Legends.Add(legend10);
            this.chart1.Location = new System.Drawing.Point(42, 71);
            this.chart1.Name = "chart1";
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series10.IsXValueIndexed = true;
            series10.Legend = "Legend1";
            series10.Name = "Temperature";
            series10.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series10.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart1.Series.Add(series10);
            this.chart1.Size = new System.Drawing.Size(625, 449);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 30F);
            this.label1.Location = new System.Drawing.Point(1413, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(272, 50);
            this.label1.TabIndex = 1;
            this.label1.Text = "当前温度：";
            // 
            // lblTemperature
            // 
            this.lblTemperature.AutoSize = true;
            this.lblTemperature.Font = new System.Drawing.Font("宋体", 30F);
            this.lblTemperature.Location = new System.Drawing.Point(1691, 150);
            this.lblTemperature.Name = "lblTemperature";
            this.lblTemperature.Size = new System.Drawing.Size(97, 50);
            this.lblTemperature.TabIndex = 2;
            this.lblTemperature.Text = "0°";
            // 
            // dgvTH
            // 
            this.dgvTH.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTH.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Temperature,
            this.humidity,
            this.Light,
            this.DTime});
            this.dgvTH.Location = new System.Drawing.Point(767, 546);
            this.dgvTH.Name = "dgvTH";
            this.dgvTH.RowHeadersWidth = 51;
            this.dgvTH.RowTemplate.Height = 27;
            this.dgvTH.Size = new System.Drawing.Size(736, 449);
            this.dgvTH.TabIndex = 3;
            // 
            // lblHum
            // 
            this.lblHum.AutoSize = true;
            this.lblHum.Font = new System.Drawing.Font("宋体", 30F);
            this.lblHum.Location = new System.Drawing.Point(1691, 71);
            this.lblHum.Name = "lblHum";
            this.lblHum.Size = new System.Drawing.Size(97, 50);
            this.lblHum.TabIndex = 4;
            this.lblHum.Text = "0°";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 30F);
            this.label2.Location = new System.Drawing.Point(1413, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(272, 50);
            this.label2.TabIndex = 5;
            this.label2.Text = "当前湿度：";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(1593, 329);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(113, 57);
            this.btnReset.TabIndex = 6;
            this.btnReset.Text = "开始监测";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // Temperature
            // 
            this.Temperature.HeaderText = "温度";
            this.Temperature.MinimumWidth = 6;
            this.Temperature.Name = "Temperature";
            this.Temperature.Width = 125;
            // 
            // humidity
            // 
            this.humidity.HeaderText = "湿度";
            this.humidity.MinimumWidth = 6;
            this.humidity.Name = "humidity";
            this.humidity.Width = 125;
            // 
            // Light
            // 
            this.Light.HeaderText = "亮度";
            this.Light.MinimumWidth = 6;
            this.Light.Name = "Light";
            this.Light.Width = 125;
            // 
            // DTime
            // 
            this.DTime.HeaderText = "时间";
            this.DTime.MinimumWidth = 6;
            this.DTime.Name = "DTime";
            this.DTime.Width = 125;
            // 
            // chart2
            // 
            chartArea11.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea11);
            legend11.Name = "Legend1";
            this.chart2.Legends.Add(legend11);
            this.chart2.Location = new System.Drawing.Point(767, 71);
            this.chart2.Name = "chart2";
            series11.ChartArea = "ChartArea1";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series11.IsXValueIndexed = true;
            series11.Legend = "Legend1";
            series11.Name = "Humidity";
            series11.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series11.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart2.Series.Add(series11);
            this.chart2.Size = new System.Drawing.Size(625, 449);
            this.chart2.TabIndex = 10;
            this.chart2.Text = "chart2";
            // 
            // lblLight
            // 
            this.lblLight.AutoSize = true;
            this.lblLight.Font = new System.Drawing.Font("宋体", 30F);
            this.lblLight.Location = new System.Drawing.Point(1691, 229);
            this.lblLight.Name = "lblLight";
            this.lblLight.Size = new System.Drawing.Size(172, 50);
            this.lblLight.TabIndex = 8;
            this.lblLight.Text = "0(Lux)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 30F);
            this.label3.Location = new System.Drawing.Point(1413, 229);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(272, 50);
            this.label3.TabIndex = 9;
            this.label3.Text = "当前亮度：";
            // 
            // chart3
            // 
            chartArea12.Name = "ChartArea1";
            this.chart3.ChartAreas.Add(chartArea12);
            legend12.Name = "Legend1";
            this.chart3.Legends.Add(legend12);
            this.chart3.Location = new System.Drawing.Point(42, 546);
            this.chart3.Name = "chart3";
            series12.ChartArea = "ChartArea1";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series12.IsXValueIndexed = true;
            series12.Legend = "Legend1";
            series12.Name = "Light";
            series12.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series12.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart3.Series.Add(series12);
            this.chart3.Size = new System.Drawing.Size(625, 449);
            this.chart3.TabIndex = 11;
            this.chart3.Text = "chart3";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(1593, 938);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 57);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "关闭窗口";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmTHLRead
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1898, 1029);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.chart3);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblLight);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblHum);
            this.Controls.Add(this.dgvTH);
            this.Controls.Add(this.lblTemperature);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chart1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "FrmTHLRead";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "温湿度光照读取界面";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTHLRead_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmTHLRead_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTemperature;
        private System.Windows.Forms.DataGridView dgvTH;
        private System.Windows.Forms.Label lblHum;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.DataGridViewTextBoxColumn Temperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn humidity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Light;
        private System.Windows.Forms.DataGridViewTextBoxColumn DTime;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.Label lblLight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
        private System.Windows.Forms.Button btnClose;
    }
}