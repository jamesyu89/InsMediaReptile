namespace InstagramPhotos.CodeGender
{
    partial class frmGeCodeFiles
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
            this.cklTables = new System.Windows.Forms.CheckedListBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.ckGroupByPrefix = new System.Windows.Forms.CheckBox();
            this.btnOutputPath = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.ckbCode = new System.Windows.Forms.CheckBox();
            this.ckInterface = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cklTables
            // 
            this.cklTables.CheckOnClick = true;
            this.cklTables.FormattingEnabled = true;
            this.cklTables.Location = new System.Drawing.Point(12, 39);
            this.cklTables.Name = "cklTables";
            this.cklTables.Size = new System.Drawing.Size(519, 228);
            this.cklTables.TabIndex = 0;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(50, 323);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(178, 25);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "生成";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "请选择要生成的表";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(234, 323);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(178, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "算了";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(123, 14);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(48, 16);
            this.chkAll.TabIndex = 3;
            this.chkAll.Text = "全选";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // ckGroupByPrefix
            // 
            this.ckGroupByPrefix.AutoSize = true;
            this.ckGroupByPrefix.Checked = true;
            this.ckGroupByPrefix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckGroupByPrefix.Location = new System.Drawing.Point(258, 13);
            this.ckGroupByPrefix.Name = "ckGroupByPrefix";
            this.ckGroupByPrefix.Size = new System.Drawing.Size(96, 16);
            this.ckGroupByPrefix.TabIndex = 3;
            this.ckGroupByPrefix.Text = "根据前缀分组";
            this.ckGroupByPrefix.UseVisualStyleBackColor = true;
            // 
            // btnOutputPath
            // 
            this.btnOutputPath.Location = new System.Drawing.Point(17, 284);
            this.btnOutputPath.Name = "btnOutputPath";
            this.btnOutputPath.Size = new System.Drawing.Size(75, 21);
            this.btnOutputPath.TabIndex = 4;
            this.btnOutputPath.Text = "输出路径";
            this.btnOutputPath.UseVisualStyleBackColor = true;
            this.btnOutputPath.Click += new System.EventHandler(this.btnOutputPath_Click);
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(98, 286);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(354, 21);
            this.tbPath.TabIndex = 5;
            // 
            // ckbCode
            // 
            this.ckbCode.AutoSize = true;
            this.ckbCode.Checked = true;
            this.ckbCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbCode.Location = new System.Drawing.Point(374, 15);
            this.ckbCode.Name = "ckbCode";
            this.ckbCode.Size = new System.Drawing.Size(72, 16);
            this.ckbCode.TabIndex = 6;
            this.ckbCode.Text = "过滤码表";
            this.ckbCode.UseVisualStyleBackColor = true;
            // 
            // ckInterface
            // 
            this.ckInterface.AutoSize = true;
            this.ckInterface.Location = new System.Drawing.Point(453, 15);
            this.ckInterface.Name = "ckInterface";
            this.ckInterface.Size = new System.Drawing.Size(72, 16);
            this.ckInterface.TabIndex = 7;
            this.ckInterface.Text = "业务接口";
            this.ckInterface.UseVisualStyleBackColor = true;
            // 
            // frmGeCodeFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 517);
            this.Controls.Add(this.ckInterface);
            this.Controls.Add(this.ckbCode);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.btnOutputPath);
            this.Controls.Add(this.ckGroupByPrefix);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.cklTables);
            this.MaximizeBox = false;
            this.Name = "frmGeCodeFiles";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "导出数据库文档";
            this.Load += new System.EventHandler(this.frmDBDocument_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cklTables;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.CheckBox ckGroupByPrefix;
        private System.Windows.Forms.Button btnOutputPath;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.CheckBox ckbCode;
        private System.Windows.Forms.CheckBox ckInterface;
    }
}