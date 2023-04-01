namespace ExcelToCode
{
    partial class Form1
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
            configListBox = new CheckedListBox();
            logTb = new TextBox();
            errLogTb = new TextBox();
            ServerAll = new Button();
            ServerSingle = new Button();
            clearLogBtn = new Button();
            ClientAll = new Button();
            ClientSingle = new Button();
            clearErrLogBtn = new Button();
            label1 = new Label();
            label2 = new Label();
            coverList = new CheckedListBox();
            SuspendLayout();
            // 
            // configListBox
            // 
            configListBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            configListBox.FormattingEnabled = true;
            configListBox.Location = new Point(1, 36);
            configListBox.Name = "configListBox";
            configListBox.Size = new Size(379, 274);
            configListBox.TabIndex = 0;
            // 
            // logTb
            // 
            logTb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            logTb.Location = new Point(1, 357);
            logTb.Multiline = true;
            logTb.Name = "logTb";
            logTb.Size = new Size(283, 265);
            logTb.TabIndex = 1;
            // 
            // errLogTb
            // 
            errLogTb.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            errLogTb.ForeColor = Color.FromArgb(192, 0, 0);
            errLogTb.Location = new Point(250, 357);
            errLogTb.Multiline = true;
            errLogTb.Name = "errLogTb";
            errLogTb.Size = new Size(422, 265);
            errLogTb.TabIndex = 2;
            // 
            // ServerAll
            // 
            ServerAll.Location = new Point(18, 316);
            ServerAll.Name = "ServerAll";
            ServerAll.Size = new Size(82, 32);
            ServerAll.TabIndex = 3;
            ServerAll.Text = "服务器-ALL";
            ServerAll.UseVisualStyleBackColor = true;
            ServerAll.Click += ServerAll_Click;
            // 
            // ServerSingle
            // 
            ServerSingle.Location = new Point(114, 316);
            ServerSingle.Name = "ServerSingle";
            ServerSingle.Size = new Size(82, 32);
            ServerSingle.TabIndex = 3;
            ServerSingle.Text = "服务器-单选";
            ServerSingle.UseVisualStyleBackColor = true;
            ServerSingle.Click += ServerSingle_Click;
            // 
            // clearLogBtn
            // 
            clearLogBtn.Location = new Point(210, 316);
            clearLogBtn.Name = "clearLogBtn";
            clearLogBtn.Size = new Size(82, 32);
            clearLogBtn.TabIndex = 3;
            clearLogBtn.Text = "清理日志";
            clearLogBtn.UseVisualStyleBackColor = true;
            clearLogBtn.Click += ClearLogBtn_Click;
            // 
            // ClientAll
            // 
            ClientAll.Location = new Point(327, 317);
            ClientAll.Name = "ClientAll";
            ClientAll.Size = new Size(82, 32);
            ClientAll.TabIndex = 3;
            ClientAll.Text = "客户端-ALL";
            ClientAll.UseVisualStyleBackColor = true;
            ClientAll.Click += ClientAll_Click;
            // 
            // ClientSingle
            // 
            ClientSingle.Location = new Point(424, 317);
            ClientSingle.Name = "ClientSingle";
            ClientSingle.Size = new Size(82, 32);
            ClientSingle.TabIndex = 3;
            ClientSingle.Text = "客户端-单选";
            ClientSingle.UseVisualStyleBackColor = true;
            ClientSingle.Click += ClientSingle_Click;
            // 
            // clearErrLogBtn
            // 
            clearErrLogBtn.Location = new Point(521, 317);
            clearErrLogBtn.Name = "clearErrLogBtn";
            clearErrLogBtn.Size = new Size(82, 32);
            clearErrLogBtn.TabIndex = 3;
            clearErrLogBtn.Text = "清理日志";
            clearErrLogBtn.UseVisualStyleBackColor = true;
            clearErrLogBtn.Click += ClearErrLogBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(1, 9);
            label1.Name = "label1";
            label1.Size = new Size(59, 17);
            label1.TabIndex = 4;
            label1.Text = "文件列表:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(390, 9);
            label2.Name = "label2";
            label2.Size = new Size(260, 17);
            label2.TabIndex = 5;
            label2.Text = "目录列表：第一个是当前目录，后续是差异目录";
            // 
            // coverList
            // 
            coverList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            coverList.FormattingEnabled = true;
            coverList.Location = new Point(386, 36);
            coverList.Name = "coverList";
            coverList.Size = new Size(286, 274);
            coverList.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(673, 623);
            Controls.Add(coverList);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(clearErrLogBtn);
            Controls.Add(ClientSingle);
            Controls.Add(ClientAll);
            Controls.Add(clearLogBtn);
            Controls.Add(ServerSingle);
            Controls.Add(ServerAll);
            Controls.Add(errLogTb);
            Controls.Add(logTb);
            Controls.Add(configListBox);
            Name = "Form1";
            Text = "导表工具";
            Load += OnFormLoaded;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckedListBox configListBox;
        private TextBox logTb;
        private TextBox errLogTb;
        private Button ServerAll;
        private Button ServerSingle;
        private Button clearLogBtn;
        private Button ClientAll;
        private Button ClientSingle;
        private Button clearErrLogBtn;
        private Label label1;
        private Label label2;
        private CheckedListBox coverList;
    }
}