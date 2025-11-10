namespace THLHostForm
{
    partial class FrmPortManager
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
            this.cboPortSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.btnOpenPort = new System.Windows.Forms.Button();
            this.btnClosePort = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPortState = new System.Windows.Forms.Label();
            this.btnSendQuest = new System.Windows.Forms.Button();
            this.SerialPortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvPortName = new System.Windows.Forms.DataGridView();
            this.dgvBaudRate = new System.Windows.Forms.DataGridView();
            this.BaudRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.btnReadData = new System.Windows.Forms.Button();
            this.btnShowData = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.lblNetState = new System.Windows.Forms.Label();
            this.boxAdd = new System.Windows.Forms.TextBox();
            this.boxPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaudRate)).BeginInit();
            this.SuspendLayout();
            // 
            // cboPortSelect
            // 
            this.cboPortSelect.FormattingEnabled = true;
            this.cboPortSelect.Location = new System.Drawing.Point(125, 25);
            this.cboPortSelect.Name = "cboPortSelect";
            this.cboPortSelect.Size = new System.Drawing.Size(168, 23);
            this.cboPortSelect.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "串口号：";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(30, 686);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(97, 51);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "刷新串口";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(332, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "波特率选择：";
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Location = new System.Drawing.Point(435, 25);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(183, 23);
            this.cmbBaudRate.TabIndex = 5;
            // 
            // btnOpenPort
            // 
            this.btnOpenPort.Location = new System.Drawing.Point(185, 686);
            this.btnOpenPort.Name = "btnOpenPort";
            this.btnOpenPort.Size = new System.Drawing.Size(97, 51);
            this.btnOpenPort.TabIndex = 6;
            this.btnOpenPort.Text = "打开串口";
            this.btnOpenPort.UseVisualStyleBackColor = true;
            this.btnOpenPort.Click += new System.EventHandler(this.btnOpenPort_Click);
            // 
            // btnClosePort
            // 
            this.btnClosePort.Location = new System.Drawing.Point(332, 686);
            this.btnClosePort.Name = "btnClosePort";
            this.btnClosePort.Size = new System.Drawing.Size(97, 51);
            this.btnClosePort.TabIndex = 7;
            this.btnClosePort.Text = "关闭串口";
            this.btnClosePort.UseVisualStyleBackColor = true;
            this.btnClosePort.Click += new System.EventHandler(this.btnClosePort_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 30F);
            this.label3.Location = new System.Drawing.Point(564, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(372, 50);
            this.label3.TabIndex = 8;
            this.label3.Text = "当前串口状态：";
            // 
            // lblPortState
            // 
            this.lblPortState.AutoSize = true;
            this.lblPortState.Font = new System.Drawing.Font("宋体", 30F);
            this.lblPortState.Location = new System.Drawing.Point(981, 111);
            this.lblPortState.Name = "lblPortState";
            this.lblPortState.Size = new System.Drawing.Size(122, 50);
            this.lblPortState.TabIndex = 9;
            this.lblPortState.Text = "关闭";
            // 
            // btnSendQuest
            // 
            this.btnSendQuest.Location = new System.Drawing.Point(788, 686);
            this.btnSendQuest.Name = "btnSendQuest";
            this.btnSendQuest.Size = new System.Drawing.Size(121, 51);
            this.btnSendQuest.TabIndex = 12;
            this.btnSendQuest.Text = "发送测试指令";
            this.btnSendQuest.UseVisualStyleBackColor = true;
            this.btnSendQuest.Click += new System.EventHandler(this.ButtonReadTemp_Click);
            // 
            // SerialPortName
            // 
            this.SerialPortName.DataPropertyName = "SerialPortName";
            this.SerialPortName.HeaderText = "串口号";
            this.SerialPortName.MinimumWidth = 6;
            this.SerialPortName.Name = "SerialPortName";
            this.SerialPortName.Width = 125;
            // 
            // dgvPortName
            // 
            this.dgvPortName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPortName.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SerialPortName});
            this.dgvPortName.Location = new System.Drawing.Point(30, 111);
            this.dgvPortName.Name = "dgvPortName";
            this.dgvPortName.RowHeadersWidth = 51;
            this.dgvPortName.RowTemplate.Height = 27;
            this.dgvPortName.Size = new System.Drawing.Size(173, 543);
            this.dgvPortName.TabIndex = 2;
            this.dgvPortName.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPortName_CellDoubleClick);
            // 
            // dgvBaudRate
            // 
            this.dgvBaudRate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBaudRate.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BaudRate});
            this.dgvBaudRate.Location = new System.Drawing.Point(256, 111);
            this.dgvBaudRate.Name = "dgvBaudRate";
            this.dgvBaudRate.RowHeadersWidth = 51;
            this.dgvBaudRate.RowTemplate.Height = 27;
            this.dgvBaudRate.Size = new System.Drawing.Size(261, 543);
            this.dgvBaudRate.TabIndex = 13;
            this.dgvBaudRate.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBaudRate_CellDoubleClick);
            // 
            // BaudRate
            // 
            this.BaudRate.DataPropertyName = "BaudRate";
            this.BaudRate.HeaderText = "波特率";
            this.BaudRate.MinimumWidth = 6;
            this.BaudRate.Name = "BaudRate";
            this.BaudRate.Width = 125;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(570, 221);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 15);
            this.label5.TabIndex = 14;
            // 
            // btnReadData
            // 
            this.btnReadData.Location = new System.Drawing.Point(616, 686);
            this.btnReadData.Name = "btnReadData";
            this.btnReadData.Size = new System.Drawing.Size(120, 51);
            this.btnReadData.TabIndex = 15;
            this.btnReadData.Text = "数据读取";
            this.btnReadData.UseVisualStyleBackColor = true;
            this.btnReadData.Click += new System.EventHandler(this.btnReadData_Click);
            // 
            // btnShowData
            // 
            this.btnShowData.Location = new System.Drawing.Point(463, 686);
            this.btnShowData.Name = "btnShowData";
            this.btnShowData.Size = new System.Drawing.Size(120, 51);
            this.btnShowData.TabIndex = 16;
            this.btnShowData.Text = "数据查看";
            this.btnShowData.UseVisualStyleBackColor = true;
            this.btnShowData.Click += new System.EventHandler(this.btnShowData_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 30F);
            this.label8.Location = new System.Drawing.Point(564, 221);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(422, 50);
            this.label8.TabIndex = 17;
            this.label8.Text = "当前服务器状态：";
            // 
            // lblNetState
            // 
            this.lblNetState.AutoSize = true;
            this.lblNetState.Font = new System.Drawing.Font("宋体", 30F);
            this.lblNetState.Location = new System.Drawing.Point(981, 221);
            this.lblNetState.Name = "lblNetState";
            this.lblNetState.Size = new System.Drawing.Size(122, 50);
            this.lblNetState.TabIndex = 18;
            this.lblNetState.Text = "关闭";
            // 
            // boxAdd
            // 
            this.boxAdd.Font = new System.Drawing.Font("宋体", 15F);
            this.boxAdd.Location = new System.Drawing.Point(778, 338);
            this.boxAdd.Name = "boxAdd";
            this.boxAdd.Size = new System.Drawing.Size(295, 36);
            this.boxAdd.TabIndex = 19;
            this.boxAdd.Text = "127.0.0.1";
            // 
            // boxPort
            // 
            this.boxPort.Font = new System.Drawing.Font("宋体", 15F);
            this.boxPort.Location = new System.Drawing.Point(778, 435);
            this.boxPort.Name = "boxPort";
            this.boxPort.Size = new System.Drawing.Size(295, 36);
            this.boxPort.TabIndex = 20;
            this.boxPort.Text = "43195";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 30F);
            this.label6.Location = new System.Drawing.Point(564, 324);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(172, 50);
            this.label6.TabIndex = 21;
            this.label6.Text = "地址：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 30F);
            this.label7.Location = new System.Drawing.Point(564, 421);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(172, 50);
            this.label7.TabIndex = 22;
            this.label7.Text = "端口：";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(789, 528);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(120, 51);
            this.btnConnect.TabIndex = 23;
            this.btnConnect.Text = "连接服务器";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // FrmPortManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1231, 799);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.boxPort);
            this.Controls.Add(this.boxAdd);
            this.Controls.Add(this.lblNetState);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnShowData);
            this.Controls.Add(this.btnReadData);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dgvBaudRate);
            this.Controls.Add(this.btnSendQuest);
            this.Controls.Add(this.lblPortState);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClosePort);
            this.Controls.Add(this.btnOpenPort);
            this.Controls.Add(this.cmbBaudRate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.dgvPortName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboPortSelect);
            this.Name = "FrmPortManager";
            this.Text = "FrmPortManager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPortManager_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPortName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBaudRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboPortSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbBaudRate;
        private System.Windows.Forms.Button btnOpenPort;
        private System.Windows.Forms.Button btnClosePort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPortState;
        private System.Windows.Forms.Button btnSendQuest;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialPortName;
        private System.Windows.Forms.DataGridView dgvPortName;
        private System.Windows.Forms.DataGridView dgvBaudRate;
        private System.Windows.Forms.DataGridViewTextBoxColumn BaudRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnReadData;
        private System.Windows.Forms.Button btnShowData;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblNetState;
        private System.Windows.Forms.TextBox boxAdd;
        private System.Windows.Forms.TextBox boxPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnConnect;
    }
}