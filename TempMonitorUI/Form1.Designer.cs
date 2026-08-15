namespace TempMonitorUI
{
    partial class Form1
        //没有像Form1.cs中声明public,那么默认为internal,internal限权使得当前类只在当前项目可访问
        //partial会采用最宽松声明的限权,也就是Form1.cs中声明的public
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnStart = new Button();
            rtbLog = new RichTextBox();
            btnStop = new Button();
            numTargetTemp = new NumericUpDown();
            lblDeviationDisplay = new Label();
            lblTargetTempDisplay = new Label();
            lblTemp = new Label();
            label1 = new Label();
            numThreshold = new NumericUpDown();
            label2 = new Label();
            label3 = new Label();
            btnLoadHistory = new Button();
            btnVision = new Button();
            lblVisionResult = new Label();
            btnScpiServer = new Button();
            btnTestCppDll = new Button();
            ((System.ComponentModel.ISupportInitialize)numTargetTemp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numThreshold).BeginInit();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnStart.Location = new Point(670, 197);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(254, 71);
            btnStart.TabIndex = 0;
            btnStart.Text = "启动加热器";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // rtbLog
            // 
            rtbLog.Dock = DockStyle.Bottom;
            rtbLog.Location = new Point(0, 748);
            rtbLog.Name = "rtbLog";
            rtbLog.Size = new Size(1463, 144);
            rtbLog.TabIndex = 2;
            rtbLog.Text = "";
            rtbLog.TextChanged += rtbLog_TextChanged;
            // 
            // btnStop
            // 
            btnStop.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnStop.Location = new Point(670, 282);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(254, 71);
            btnStop.TabIndex = 3;
            btnStop.Text = "停止加热器";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // numTargetTemp
            // 
            numTargetTemp.DecimalPlaces = 1;
            numTargetTemp.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            numTargetTemp.Location = new Point(1210, 195);
            numTargetTemp.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
            numTargetTemp.Name = "numTargetTemp";
            numTargetTemp.Size = new Size(150, 68);
            numTargetTemp.TabIndex = 4;
            numTargetTemp.Value = new decimal(new int[] { 25, 0, 0, 0 });
            numTargetTemp.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // lblDeviationDisplay
            // 
            lblDeviationDisplay.AutoSize = true;
            lblDeviationDisplay.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblDeviationDisplay.Location = new Point(973, 282);
            lblDeviationDisplay.Name = "lblDeviationDisplay";
            lblDeviationDisplay.RightToLeft = RightToLeft.No;
            lblDeviationDisplay.Size = new Size(301, 62);
            lblDeviationDisplay.TabIndex = 1;
            lblDeviationDisplay.Text = "偏差：0.0 ℃";
            lblDeviationDisplay.Click += lblTemp_Click;
            // 
            // lblTargetTempDisplay
            // 
            lblTargetTempDisplay.AutoSize = true;
            lblTargetTempDisplay.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblTargetTempDisplay.Location = new Point(973, 197);
            lblTargetTempDisplay.Name = "lblTargetTempDisplay";
            lblTargetTempDisplay.RightToLeft = RightToLeft.No;
            lblTargetTempDisplay.Size = new Size(231, 62);
            lblTargetTempDisplay.TabIndex = 1;
            lblTargetTempDisplay.Text = "目标温度:";
            lblTargetTempDisplay.Click += lblTemp_Click;
            // 
            // lblTemp
            // 
            lblTemp.AutoSize = true;
            lblTemp.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblTemp.Location = new Point(973, 109);
            lblTemp.Name = "lblTemp";
            lblTemp.RightToLeft = RightToLeft.No;
            lblTemp.Size = new Size(425, 62);
            lblTemp.TabIndex = 1;
            lblTemp.Text = "当前温度：25.0 ℃";
            lblTemp.Click += lblTemp_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label1.Location = new Point(1338, 197);
            label1.Name = "label1";
            label1.RightToLeft = RightToLeft.No;
            label1.Size = new Size(89, 62);
            label1.TabIndex = 1;
            label1.Text = " ℃";
            label1.Click += lblTemp_Click;
            // 
            // numThreshold
            // 
            numThreshold.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            numThreshold.Location = new Point(1194, 418);
            numThreshold.Maximum = new decimal(new int[] { 120, 0, 0, 0 });
            numThreshold.Name = "numThreshold";
            numThreshold.Size = new Size(109, 68);
            numThreshold.TabIndex = 4;
            numThreshold.Value = new decimal(new int[] { 80, 0, 0, 0 });
            numThreshold.ValueChanged += numericUpDown1_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label2.Location = new Point(1291, 420);
            label2.Name = "label2";
            label2.RightToLeft = RightToLeft.No;
            label2.Size = new Size(89, 62);
            label2.TabIndex = 1;
            label2.Text = " ℃";
            label2.Click += lblTemp_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            label3.Location = new Point(973, 418);
            label3.Name = "label3";
            label3.RightToLeft = RightToLeft.No;
            label3.Size = new Size(231, 62);
            label3.TabIndex = 1;
            label3.Text = "报警温度:";
            label3.Click += lblTemp_Click;
            // 
            // btnLoadHistory
            // 
            btnLoadHistory.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnLoadHistory.Location = new Point(1173, 658);
            btnLoadHistory.Name = "btnLoadHistory";
            btnLoadHistory.Size = new Size(254, 71);
            btnLoadHistory.TabIndex = 5;
            btnLoadHistory.Text = "加载历史";
            btnLoadHistory.UseVisualStyleBackColor = true;
            btnLoadHistory.Click += btnLoadHistory_Click;
            // 
            // btnVision
            // 
            btnVision.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnVision.Location = new Point(384, 512);
            btnVision.Name = "btnVision";
            btnVision.Size = new Size(254, 71);
            btnVision.TabIndex = 6;
            btnVision.Text = "视觉定位";
            btnVision.UseVisualStyleBackColor = true;
            btnVision.Click += btnVision_Click;
            // 
            // lblVisionResult
            // 
            lblVisionResult.AutoSize = true;
            lblVisionResult.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblVisionResult.Location = new Point(670, 512);
            lblVisionResult.Name = "lblVisionResult";
            lblVisionResult.RightToLeft = RightToLeft.No;
            lblVisionResult.Size = new Size(255, 62);
            lblVisionResult.TabIndex = 7;
            lblVisionResult.Text = "等待定位...";
            // 
            // btnScpiServer
            // 
            btnScpiServer.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnScpiServer.Location = new Point(205, 347);
            btnScpiServer.Name = "btnScpiServer";
            btnScpiServer.Size = new Size(420, 71);
            btnScpiServer.TabIndex = 8;
            btnScpiServer.Text = "启动 SCPI 服务器";
            btnScpiServer.UseVisualStyleBackColor = true;
            btnScpiServer.Click += btnScpiServer_Click;
            // 
            // btnTestCppDll
            // 
            btnTestCppDll.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnTestCppDll.Location = new Point(205, 255);
            btnTestCppDll.Name = "btnTestCppDll";
            btnTestCppDll.Size = new Size(364, 71);
            btnTestCppDll.TabIndex = 9;
            btnTestCppDll.Text = "测试 C++ DLL";
            btnTestCppDll.UseVisualStyleBackColor = true;
            btnTestCppDll.Click += btnTestCppDll_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1463, 892);
            Controls.Add(btnTestCppDll);
            Controls.Add(btnScpiServer);
            Controls.Add(lblVisionResult);
            Controls.Add(btnVision);
            Controls.Add(btnLoadHistory);
            Controls.Add(numThreshold);
            Controls.Add(numTargetTemp);
            Controls.Add(btnStop);
            Controls.Add(rtbLog);
            Controls.Add(label3);
            Controls.Add(lblTargetTempDisplay);
            Controls.Add(lblDeviationDisplay);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblTemp);
            Controls.Add(btnStart);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load_1;
            ((System.ComponentModel.ISupportInitialize)numTargetTemp).EndInit();
            ((System.ComponentModel.ISupportInitialize)numThreshold).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        //隐藏代码块,起始#region 隐藏代码块名称,结束#endregion,编译时正常编译


        private Button btnStart;
        private RichTextBox rtbLog;
        private Button btnStop;
        private NumericUpDown numTargetTemp;
        private Label lblDeviationDisplay;
        private Label lblTargetTempDisplay;
        private Label lblTemp;
        private Label label1;
        private NumericUpDown numThreshold;
        private Label label2;
        private Label label3;
        private Button btnLoadHistory;
        private Button btnVision;
        private Label lblVisionResult;
        private Button btnScpiServer;
        private Button btnTestCppDll;
    }
}
