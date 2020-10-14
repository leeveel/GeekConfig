namespace ExcelConverter
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ServerAll = new System.Windows.Forms.Button();
            this.configListBox = new System.Windows.Forms.CheckedListBox();
            this.logTb = new System.Windows.Forms.TextBox();
            this.ServerSingle = new System.Windows.Forms.Button();
            this.ClientAll = new System.Windows.Forms.Button();
            this.ClientSingle = new System.Windows.Forms.Button();
            this.errLogTb = new System.Windows.Forms.TextBox();
            this.clearLogBtn = new System.Windows.Forms.Button();
            this.clearErrLogBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ServerAll
            // 
            this.ServerAll.Location = new System.Drawing.Point(11, 333);
            this.ServerAll.Name = "ServerAll";
            this.ServerAll.Size = new System.Drawing.Size(76, 33);
            this.ServerAll.TabIndex = 0;
            this.ServerAll.Text = "服务器-ALL";
            this.ServerAll.UseVisualStyleBackColor = true;
            this.ServerAll.Click += new System.EventHandler(this.ServerAll_Click);
            // 
            // configListBox
            // 
            this.configListBox.FormattingEnabled = true;
            this.configListBox.Location = new System.Drawing.Point(3, 3);
            this.configListBox.Name = "configListBox";
            this.configListBox.Size = new System.Drawing.Size(631, 324);
            this.configListBox.TabIndex = 1;
            // 
            // logTb
            // 
            this.logTb.BackColor = System.Drawing.Color.White;
            this.logTb.Location = new System.Drawing.Point(3, 372);
            this.logTb.Multiline = true;
            this.logTb.Name = "logTb";
            this.logTb.ReadOnly = true;
            this.logTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTb.Size = new System.Drawing.Size(234, 178);
            this.logTb.TabIndex = 2;
            // 
            // ServerSingle
            // 
            this.ServerSingle.Location = new System.Drawing.Point(93, 333);
            this.ServerSingle.Name = "ServerSingle";
            this.ServerSingle.Size = new System.Drawing.Size(86, 33);
            this.ServerSingle.TabIndex = 0;
            this.ServerSingle.Text = "服务器-单选";
            this.ServerSingle.UseVisualStyleBackColor = true;
            this.ServerSingle.Click += new System.EventHandler(this.ServerSingle_Click);
            // 
            // ClientAll
            // 
            this.ClientAll.Location = new System.Drawing.Point(353, 333);
            this.ClientAll.Name = "ClientAll";
            this.ClientAll.Size = new System.Drawing.Size(87, 33);
            this.ClientAll.TabIndex = 0;
            this.ClientAll.Text = "客户端-ALL";
            this.ClientAll.UseVisualStyleBackColor = true;
            this.ClientAll.Click += new System.EventHandler(this.ClientAll_Click);
            // 
            // ClientSingle
            // 
            this.ClientSingle.Location = new System.Drawing.Point(446, 333);
            this.ClientSingle.Name = "ClientSingle";
            this.ClientSingle.Size = new System.Drawing.Size(90, 33);
            this.ClientSingle.TabIndex = 0;
            this.ClientSingle.Text = "客户端-单选";
            this.ClientSingle.UseVisualStyleBackColor = true;
            this.ClientSingle.Click += new System.EventHandler(this.ClientSingle_Click);
            // 
            // errLogTb
            // 
            this.errLogTb.BackColor = System.Drawing.Color.White;
            this.errLogTb.ForeColor = System.Drawing.Color.Crimson;
            this.errLogTb.Location = new System.Drawing.Point(243, 372);
            this.errLogTb.Multiline = true;
            this.errLogTb.Name = "errLogTb";
            this.errLogTb.ReadOnly = true;
            this.errLogTb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.errLogTb.Size = new System.Drawing.Size(398, 178);
            this.errLogTb.TabIndex = 2;
            // 
            // clearLogBtn
            // 
            this.clearLogBtn.Location = new System.Drawing.Point(185, 333);
            this.clearLogBtn.Name = "clearLogBtn";
            this.clearLogBtn.Size = new System.Drawing.Size(86, 33);
            this.clearLogBtn.TabIndex = 0;
            this.clearLogBtn.Text = "清除日志";
            this.clearLogBtn.UseVisualStyleBackColor = true;
            this.clearLogBtn.Click += new System.EventHandler(this.ClearLogBtn_Click);
            // 
            // clearErrLogBtn
            // 
            this.clearErrLogBtn.Location = new System.Drawing.Point(542, 333);
            this.clearErrLogBtn.Name = "clearErrLogBtn";
            this.clearErrLogBtn.Size = new System.Drawing.Size(90, 33);
            this.clearErrLogBtn.TabIndex = 0;
            this.clearErrLogBtn.Text = "清除日志";
            this.clearErrLogBtn.UseVisualStyleBackColor = true;
            this.clearErrLogBtn.Click += new System.EventHandler(this.ClearErrLogBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 555);
            this.Controls.Add(this.errLogTb);
            this.Controls.Add(this.logTb);
            this.Controls.Add(this.configListBox);
            this.Controls.Add(this.clearErrLogBtn);
            this.Controls.Add(this.ClientSingle);
            this.Controls.Add(this.ClientAll);
            this.Controls.Add(this.clearLogBtn);
            this.Controls.Add(this.ServerSingle);
            this.Controls.Add(this.ServerAll);
            this.Name = "Form1";
            this.Text = "导表工具";
            this.Load += new System.EventHandler(this.OnFormLoaded);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ServerAll;
        private System.Windows.Forms.CheckedListBox configListBox;
        private System.Windows.Forms.TextBox logTb;
        private System.Windows.Forms.Button ServerSingle;
        private System.Windows.Forms.Button ClientAll;
        private System.Windows.Forms.Button ClientSingle;
        private System.Windows.Forms.TextBox errLogTb;
        private System.Windows.Forms.Button clearLogBtn;
        private System.Windows.Forms.Button clearErrLogBtn;
    }
}

