namespace InstagramPhotos.CodeGender
{
    partial class frmConn
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
            this.lstConns = new System.Windows.Forms.ListBox();
            this.btnConn = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstConns
            // 
            this.lstConns.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstConns.FormattingEnabled = true;
            this.lstConns.HorizontalScrollbar = true;
            this.lstConns.ItemHeight = 16;
            this.lstConns.Location = new System.Drawing.Point(12, 12);
            this.lstConns.Name = "lstConns";
            this.lstConns.Size = new System.Drawing.Size(440, 196);
            this.lstConns.TabIndex = 0;
            this.lstConns.DoubleClick += new System.EventHandler(this.lstConns_DoubleClick);
            // 
            // btnConn
            // 
            this.btnConn.Location = new System.Drawing.Point(193, 230);
            this.btnConn.Name = "btnConn";
            this.btnConn.Size = new System.Drawing.Size(90, 40);
            this.btnConn.TabIndex = 1;
            this.btnConn.Text = "连上去";
            this.btnConn.UseVisualStyleBackColor = true;
            this.btnConn.Click += new System.EventHandler(this.btnConn_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(285, 230);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(90, 40);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "删了";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(98, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "新的连接";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmConn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 282);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnConn);
            this.Controls.Add(this.lstConns);
            this.Name = "frmConn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmConn";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lstConns;
        private System.Windows.Forms.Button btnConn;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button button1;
    }
}