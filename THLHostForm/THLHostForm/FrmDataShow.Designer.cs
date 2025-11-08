namespace THLHostForm
{
    partial class FrmDataShow
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart3 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvTH = new System.Windows.Forms.DataGridView();
            this.Temperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.humidity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Light = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.startDtp = new System.Windows.Forms.DateTimePicker();
            this.endDtp = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.showDataBtn = new System.Windows.Forms.Button();
            this.btnToExcel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart3
            // 
            chartArea4.Name = "ChartArea1";
            this.chart3.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart3.Legends.Add(legend4);
            this.chart3.Location = new System.Drawing.Point(34, 507);
            this.chart3.Name = "chart3";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.IsXValueIndexed = true;
            series4.Legend = "Legend1";
            series4.Name = "Light";
            series4.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            series4.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart3.Series.Add(series4);
            this.chart3.Size = new System.Drawing.Size(625, 449);
            this.chart3.TabIndex = 15;
            this.chart3.Text = "chart3";
            // 
            // chart2
            // 
            chartArea5.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.chart2.Legends.Add(legend5);
            this.chart2.Location = new System.Drawing.Point(759, 32);
            this.chart2.Name = "chart2";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series5.IsXValueIndexed = true;
            series5.Legend = "Legend1";
            series5.Name = "Humidity";
            series5.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series5.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart2.Series.Add(series5);
            this.chart2.Size = new System.Drawing.Size(625, 449);
            this.chart2.TabIndex = 14;
            this.chart2.Text = "chart2";
            // 
            // dgvTH
            // 
            this.dgvTH.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTH.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Temperature,
            this.humidity,
            this.Light,
            this.DTime});
            this.dgvTH.Location = new System.Drawing.Point(759, 507);
            this.dgvTH.Name = "dgvTH";
            this.dgvTH.RowHeadersWidth = 51;
            this.dgvTH.RowTemplate.Height = 27;
            this.dgvTH.Size = new System.Drawing.Size(736, 449);
            this.dgvTH.TabIndex = 13;
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
            // chart1
            // 
            chartArea6.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.chart1.Legends.Add(legend6);
            this.chart1.Location = new System.Drawing.Point(34, 32);
            this.chart1.Name = "chart1";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series6.IsXValueIndexed = true;
            series6.Legend = "Legend1";
            series6.Name = "Temperature";
            series6.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series6.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chart1.Series.Add(series6);
            this.chart1.Size = new System.Drawing.Size(672, 449);
            this.chart1.TabIndex = 12;
            this.chart1.Text = "chart1";
            // 
            // startDtp
            // 
            this.startDtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDtp.Location = new System.Drawing.Point(1415, 90);
            this.startDtp.Name = "startDtp";
            this.startDtp.ShowUpDown = true;
            this.startDtp.Size = new System.Drawing.Size(198, 25);
            this.startDtp.TabIndex = 16;
            // 
            // endDtp
            // 
            this.endDtp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDtp.Location = new System.Drawing.Point(1661, 90);
            this.endDtp.Name = "endDtp";
            this.endDtp.ShowUpDown = true;
            this.endDtp.Size = new System.Drawing.Size(207, 25);
            this.endDtp.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F);
            this.label1.Location = new System.Drawing.Point(1463, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 25);
            this.label1.TabIndex = 18;
            this.label1.Text = "开始时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15F);
            this.label2.Location = new System.Drawing.Point(1700, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 25);
            this.label2.TabIndex = 19;
            this.label2.Text = "结束时间";
            // 
            // showDataBtn
            // 
            this.showDataBtn.Location = new System.Drawing.Point(1560, 156);
            this.showDataBtn.Name = "showDataBtn";
            this.showDataBtn.Size = new System.Drawing.Size(127, 34);
            this.showDataBtn.TabIndex = 20;
            this.showDataBtn.Text = "查看数据";
            this.showDataBtn.UseVisualStyleBackColor = true;
            this.showDataBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(1560, 252);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(127, 34);
            this.btnToExcel.TabIndex = 21;
            this.btnToExcel.Text = "打印数据";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.btnToExcel_Click);
            // 
            // FrmDataShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1880, 982);
            this.Controls.Add(this.btnToExcel);
            this.Controls.Add(this.showDataBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.endDtp);
            this.Controls.Add(this.startDtp);
            this.Controls.Add(this.chart3);
            this.Controls.Add(this.chart2);
            this.Controls.Add(this.dgvTH);
            this.Controls.Add(this.chart1);
            this.MaximizeBox = false;
            this.Name = "FrmDataShow";
            this.Text = "FrmDataShow";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.chart3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.DataGridView dgvTH;
        private System.Windows.Forms.DataGridViewTextBoxColumn Temperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn humidity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Light;
        private System.Windows.Forms.DataGridViewTextBoxColumn DTime;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.DateTimePicker startDtp;
        private System.Windows.Forms.DateTimePicker endDtp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button showDataBtn;
        private System.Windows.Forms.Button btnToExcel;
    }
}