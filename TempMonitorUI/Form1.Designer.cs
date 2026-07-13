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
            lblTemp = new Label();
            rtbLog = new RichTextBox();
            SuspendLayout();
            // 
            // btnStart
            // 
            btnStart.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            btnStart.Location = new Point(687, 372);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(254, 71);
            btnStart.TabIndex = 0;
            btnStart.Text = "启动加热器";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // lblTemp
            // 
            lblTemp.AutoSize = true;
            lblTemp.Font = new Font("Microsoft YaHei UI", 24F, FontStyle.Regular, GraphicsUnit.Point, 134);
            lblTemp.Location = new Point(973, 376);
            lblTemp.Name = "lblTemp";
            lblTemp.RightToLeft = RightToLeft.No;
            lblTemp.Size = new Size(157, 62);
            lblTemp.TabIndex = 1;
            lblTemp.Text = "0.0 ℃";
            lblTemp.Click += lblTemp_Click;
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1463, 892);
            Controls.Add(rtbLog);
            Controls.Add(lblTemp);
            Controls.Add(btnStart);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        //隐藏代码块,起始#region 隐藏代码块名称,结束#endregion,编译时正常编译


        private Button btnStart;
        private Label lblTemp;
        private RichTextBox rtbLog;
    }
}
