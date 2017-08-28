namespace InstagramPhotos.CodeGender
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbConnect = new System.Windows.Forms.ToolStripButton();
            this.tsbExportDBDoc = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tsbExportMarkCode = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.tvTables = new System.Windows.Forms.TreeView();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.scCenter = new System.Windows.Forms.SplitContainer();
            this.lvColumns = new System.Windows.Forms.ListView();
            this.chTtile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.condition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chIsPrimaryKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTypeName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRemarks = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabCodeGenderType = new System.Windows.Forms.TabControl();
            this.tpEntity = new System.Windows.Forms.TabPage();
            this.txtQueryNameSpace = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.txtDtoNameSpace = new System.Windows.Forms.TextBox();
            this.txtBllNameSpace = new System.Windows.Forms.TextBox();
            this.txtDalNameSpace = new System.Windows.Forms.TextBox();
            this.txtSubClassName = new System.Windows.Forms.TextBox();
            this.txtModelNameSpace = new System.Windows.Forms.TextBox();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.ckbDbIndex = new System.Windows.Forms.CheckBox();
            this.ckbServerIndex = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ckPartial = new System.Windows.Forms.CheckBox();
            this.ckbPreNameSpace = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.ckParamCache = new System.Windows.Forms.CheckBox();
            this.ckSingletonBLL = new System.Windows.Forms.CheckBox();
            this.ckNeedAutoGuid = new System.Windows.Forms.CheckBox();
            this.ckProtoBuf = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpOther = new System.Windows.Forms.TabPage();
            this.txtAuthor = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGenerateEntity = new System.Windows.Forms.Button();
            this.tabCodes = new System.Windows.Forms.TabControl();
            this.tpEntityCode = new System.Windows.Forms.TabPage();
            this.tpQueryCode = new System.Windows.Forms.TabPage();
            this.tpEntityDto = new System.Windows.Forms.TabPage();
            this.tpMapperExtension = new System.Windows.Forms.TabPage();
            this.tpRepository = new System.Windows.Forms.TabPage();
            this.tpBusiness = new System.Windows.Forms.TabPage();
            this.tpIServices = new System.Windows.Forms.TabPage();
            this.tpDataAccessCode = new System.Windows.Forms.TabPage();
            this.tpManagerCode = new System.Windows.Forms.TabPage();
            this.tpStoredProceduresCode = new System.Windows.Forms.TabPage();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scCenter)).BeginInit();
            this.scCenter.Panel1.SuspendLayout();
            this.scCenter.Panel2.SuspendLayout();
            this.scCenter.SuspendLayout();
            this.tabCodeGenderType.SuspendLayout();
            this.tpEntity.SuspendLayout();
            this.tpOther.SuspendLayout();
            this.tabCodes.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbConnect,
            this.tsbExportDBDoc,
            this.toolStripButton1,
            this.tsbExportMarkCode});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1184, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbConnect
            // 
            this.tsbConnect.Image = ((System.Drawing.Image)(resources.GetObject("tsbConnect.Image")));
            this.tsbConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbConnect.Name = "tsbConnect";
            this.tsbConnect.Size = new System.Drawing.Size(88, 22);
            this.tsbConnect.Text = "连接数据库";
            this.tsbConnect.Click += new System.EventHandler(this.tsbConnect_Click);
            // 
            // tsbExportDBDoc
            // 
            this.tsbExportDBDoc.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportDBDoc.Image")));
            this.tsbExportDBDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportDBDoc.Name = "tsbExportDBDoc";
            this.tsbExportDBDoc.Size = new System.Drawing.Size(112, 22);
            this.tsbExportDBDoc.Text = "导出数据库文档";
            this.tsbExportDBDoc.Visible = false;
            this.tsbExportDBDoc.Click += new System.EventHandler(this.tsbExportDBDoc_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(100, 22);
            this.toolStripButton1.Text = "导出码表枚举";
            this.toolStripButton1.Visible = false;
            this.toolStripButton1.Click += new System.EventHandler(this.tsbExportMarkCode_Click);
            // 
            // tsbExportMarkCode
            // 
            this.tsbExportMarkCode.Image = ((System.Drawing.Image)(resources.GetObject("tsbExportMarkCode.Image")));
            this.tsbExportMarkCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExportMarkCode.Name = "tsbExportMarkCode";
            this.tsbExportMarkCode.Size = new System.Drawing.Size(124, 22);
            this.tsbExportMarkCode.Text = "批量生成代码文件";
            this.tsbExportMarkCode.Visible = false;
            this.tsbExportMarkCode.Click += new System.EventHandler(this.tspCodeFiles_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 637);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1184, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(0, 25);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tvTables);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.scMain);
            this.splitContainer.Size = new System.Drawing.Size(1184, 612);
            this.splitContainer.SplitterDistance = 165;
            this.splitContainer.TabIndex = 2;
            // 
            // tvTables
            // 
            this.tvTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTables.Location = new System.Drawing.Point(0, 0);
            this.tvTables.Name = "tvTables";
            this.tvTables.Size = new System.Drawing.Size(165, 612);
            this.tvTables.TabIndex = 0;
            this.tvTables.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvTables_AfterExpand);
            this.tvTables.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTables_AfterSelect);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scMain.Location = new System.Drawing.Point(0, 0);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.scCenter);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.tabCodes);
            this.scMain.Size = new System.Drawing.Size(1015, 612);
            this.scMain.SplitterDistance = 346;
            this.scMain.TabIndex = 1;
            // 
            // scCenter
            // 
            this.scCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scCenter.Location = new System.Drawing.Point(0, 0);
            this.scCenter.Name = "scCenter";
            this.scCenter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scCenter.Panel1
            // 
            this.scCenter.Panel1.Controls.Add(this.lvColumns);
            // 
            // scCenter.Panel2
            // 
            this.scCenter.Panel2.Controls.Add(this.tabCodeGenderType);
            this.scCenter.Panel2.Controls.Add(this.btnGenerateEntity);
            this.scCenter.Size = new System.Drawing.Size(346, 612);
            this.scCenter.SplitterDistance = 224;
            this.scCenter.TabIndex = 2;
            // 
            // lvColumns
            // 
            this.lvColumns.CheckBoxes = true;
            this.lvColumns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTtile,
            this.condition,
            this.chName,
            this.chIsPrimaryKey,
            this.chTypeName,
            this.chRemarks});
            this.lvColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvColumns.FullRowSelect = true;
            this.lvColumns.GridLines = true;
            this.lvColumns.Location = new System.Drawing.Point(0, 0);
            this.lvColumns.Name = "lvColumns";
            this.lvColumns.Size = new System.Drawing.Size(346, 224);
            this.lvColumns.TabIndex = 0;
            this.lvColumns.UseCompatibleStateImageBehavior = false;
            this.lvColumns.View = System.Windows.Forms.View.Details;
            this.lvColumns.SelectedIndexChanged += new System.EventHandler(this.lvColumns_SelectedIndexChanged);
            this.lvColumns.Resize += new System.EventHandler(this.lvColumns_Resize);
            // 
            // chTtile
            // 
            this.chTtile.Text = "序列";
            this.chTtile.Width = 50;
            // 
            // condition
            // 
            this.condition.Text = "条件";
            this.condition.Width = 50;
            // 
            // chName
            // 
            this.chName.Text = "名称";
            this.chName.Width = 100;
            // 
            // chIsPrimaryKey
            // 
            this.chIsPrimaryKey.Text = "主键";
            this.chIsPrimaryKey.Width = 38;
            // 
            // chTypeName
            // 
            this.chTypeName.Text = "类型";
            this.chTypeName.Width = 75;
            // 
            // chRemarks
            // 
            this.chRemarks.Text = "备注";
            this.chRemarks.Width = 340;
            // 
            // tabCodeGenderType
            // 
            this.tabCodeGenderType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCodeGenderType.Controls.Add(this.tpEntity);
            this.tabCodeGenderType.Controls.Add(this.tpOther);
            this.tabCodeGenderType.Location = new System.Drawing.Point(0, 2);
            this.tabCodeGenderType.Name = "tabCodeGenderType";
            this.tabCodeGenderType.SelectedIndex = 0;
            this.tabCodeGenderType.Size = new System.Drawing.Size(346, 330);
            this.tabCodeGenderType.TabIndex = 2;
            // 
            // tpEntity
            // 
            this.tpEntity.Controls.Add(this.txtQueryNameSpace);
            this.tpEntity.Controls.Add(this.label5);
            this.tpEntity.Controls.Add(this.textBox1);
            this.tpEntity.Controls.Add(this.txtDtoNameSpace);
            this.tpEntity.Controls.Add(this.txtBllNameSpace);
            this.tpEntity.Controls.Add(this.txtDalNameSpace);
            this.tpEntity.Controls.Add(this.txtSubClassName);
            this.tpEntity.Controls.Add(this.txtModelNameSpace);
            this.tpEntity.Controls.Add(this.txtClassName);
            this.tpEntity.Controls.Add(this.label13);
            this.tpEntity.Controls.Add(this.ckbDbIndex);
            this.tpEntity.Controls.Add(this.ckbServerIndex);
            this.tpEntity.Controls.Add(this.label12);
            this.tpEntity.Controls.Add(this.ckPartial);
            this.tpEntity.Controls.Add(this.ckbPreNameSpace);
            this.tpEntity.Controls.Add(this.label11);
            this.tpEntity.Controls.Add(this.label10);
            this.tpEntity.Controls.Add(this.ckParamCache);
            this.tpEntity.Controls.Add(this.ckSingletonBLL);
            this.tpEntity.Controls.Add(this.ckNeedAutoGuid);
            this.tpEntity.Controls.Add(this.ckProtoBuf);
            this.tpEntity.Controls.Add(this.label2);
            this.tpEntity.Controls.Add(this.label3);
            this.tpEntity.Controls.Add(this.label1);
            this.tpEntity.Location = new System.Drawing.Point(4, 22);
            this.tpEntity.Name = "tpEntity";
            this.tpEntity.Padding = new System.Windows.Forms.Padding(3);
            this.tpEntity.Size = new System.Drawing.Size(338, 304);
            this.tpEntity.TabIndex = 0;
            this.tpEntity.Text = "C#";
            this.tpEntity.UseVisualStyleBackColor = true;
            // 
            // txtQueryNameSpace
            // 
            this.txtQueryNameSpace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQueryNameSpace.Location = new System.Drawing.Point(96, 113);
            this.txtQueryNameSpace.Name = "txtQueryNameSpace";
            this.txtQueryNameSpace.Size = new System.Drawing.Size(236, 21);
            this.txtQueryNameSpace.TabIndex = 24;
            this.txtQueryNameSpace.Text = "InstagramPhotos.QueryModel";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 23;
            this.label5.Text = "Query命名空间:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(96, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(236, 21);
            this.textBox1.TabIndex = 22;
            this.textBox1.Text = "InstagramPhotos";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtDtoNameSpace
            // 
            this.txtDtoNameSpace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDtoNameSpace.Location = new System.Drawing.Point(96, 84);
            this.txtDtoNameSpace.Name = "txtDtoNameSpace";
            this.txtDtoNameSpace.Size = new System.Drawing.Size(236, 21);
            this.txtDtoNameSpace.TabIndex = 18;
            this.txtDtoNameSpace.Text = "InstagramPhotos.ViewModel";
            // 
            // txtBllNameSpace
            // 
            this.txtBllNameSpace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBllNameSpace.Location = new System.Drawing.Point(96, 163);
            this.txtBllNameSpace.Name = "txtBllNameSpace";
            this.txtBllNameSpace.Size = new System.Drawing.Size(236, 21);
            this.txtBllNameSpace.TabIndex = 14;
            this.txtBllNameSpace.Text = "InstagramPhotos.Service";
            // 
            // txtDalNameSpace
            // 
            this.txtDalNameSpace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDalNameSpace.Location = new System.Drawing.Point(96, 140);
            this.txtDalNameSpace.Name = "txtDalNameSpace";
            this.txtDalNameSpace.Size = new System.Drawing.Size(236, 21);
            this.txtDalNameSpace.TabIndex = 12;
            this.txtDalNameSpace.Text = "InstagramPhotos.Repository";
            // 
            // txtSubClassName
            // 
            this.txtSubClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSubClassName.Location = new System.Drawing.Point(96, 8);
            this.txtSubClassName.Name = "txtSubClassName";
            this.txtSubClassName.Size = new System.Drawing.Size(236, 21);
            this.txtSubClassName.TabIndex = 3;
            this.txtSubClassName.TextChanged += new System.EventHandler(this.txtSubClassName_TextChanged);
            // 
            // txtModelNameSpace
            // 
            this.txtModelNameSpace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModelNameSpace.Location = new System.Drawing.Point(96, 59);
            this.txtModelNameSpace.Name = "txtModelNameSpace";
            this.txtModelNameSpace.Size = new System.Drawing.Size(236, 21);
            this.txtModelNameSpace.TabIndex = 3;
            this.txtModelNameSpace.Text = "InstagramPhotos.DomainModel";
            // 
            // txtClassName
            // 
            this.txtClassName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClassName.Location = new System.Drawing.Point(96, 188);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(236, 21);
            this.txtClassName.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1, 39);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(83, 12);
            this.label13.TabIndex = 21;
            this.label13.Text = "解决方案名称:";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // ckbDbIndex
            // 
            this.ckbDbIndex.AutoSize = true;
            this.ckbDbIndex.Location = new System.Drawing.Point(152, 279);
            this.ckbDbIndex.Name = "ckbDbIndex";
            this.ckbDbIndex.Size = new System.Drawing.Size(96, 16);
            this.ckbDbIndex.TabIndex = 20;
            this.ckbDbIndex.Text = "添加db_index";
            this.ckbDbIndex.UseVisualStyleBackColor = true;
            // 
            // ckbServerIndex
            // 
            this.ckbServerIndex.AutoSize = true;
            this.ckbServerIndex.Location = new System.Drawing.Point(6, 279);
            this.ckbServerIndex.Name = "ckbServerIndex";
            this.ckbServerIndex.Size = new System.Drawing.Size(120, 16);
            this.ckbServerIndex.TabIndex = 19;
            this.ckbServerIndex.Text = "添加server_index";
            this.ckbServerIndex.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1, 90);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 17;
            this.label12.Text = "DTO命名空间:";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // ckPartial
            // 
            this.ckPartial.AutoSize = true;
            this.ckPartial.Checked = true;
            this.ckPartial.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckPartial.Location = new System.Drawing.Point(6, 257);
            this.ckPartial.Name = "ckPartial";
            this.ckPartial.Size = new System.Drawing.Size(144, 16);
            this.ckPartial.TabIndex = 16;
            this.ckPartial.Text = "业务逻辑类使用分部类";
            this.ckPartial.UseVisualStyleBackColor = true;
            // 
            // ckbPreNameSpace
            // 
            this.ckbPreNameSpace.AutoSize = true;
            this.ckbPreNameSpace.Checked = true;
            this.ckbPreNameSpace.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbPreNameSpace.Location = new System.Drawing.Point(152, 259);
            this.ckbPreNameSpace.Name = "ckbPreNameSpace";
            this.ckbPreNameSpace.Size = new System.Drawing.Size(132, 16);
            this.ckbPreNameSpace.TabIndex = 15;
            this.ckbPreNameSpace.Text = "表前缀加入命名空间";
            this.ckbPreNameSpace.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1, 169);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 13;
            this.label11.Text = "BLL命名空间:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1, 144);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 11;
            this.label10.Text = "DAL命名空间:";
            // 
            // ckParamCache
            // 
            this.ckParamCache.AutoSize = true;
            this.ckParamCache.Checked = true;
            this.ckParamCache.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckParamCache.Location = new System.Drawing.Point(6, 235);
            this.ckParamCache.Name = "ckParamCache";
            this.ckParamCache.Size = new System.Drawing.Size(96, 16);
            this.ckParamCache.TabIndex = 10;
            this.ckParamCache.Text = "使用参数缓存";
            this.ckParamCache.UseVisualStyleBackColor = true;
            // 
            // ckSingletonBLL
            // 
            this.ckSingletonBLL.AutoSize = true;
            this.ckSingletonBLL.Checked = true;
            this.ckSingletonBLL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckSingletonBLL.Location = new System.Drawing.Point(152, 235);
            this.ckSingletonBLL.Name = "ckSingletonBLL";
            this.ckSingletonBLL.Size = new System.Drawing.Size(132, 16);
            this.ckSingletonBLL.TabIndex = 9;
            this.ckSingletonBLL.Text = "使用单例业务逻辑类";
            this.ckSingletonBLL.UseVisualStyleBackColor = true;
            // 
            // ckNeedAutoGuid
            // 
            this.ckNeedAutoGuid.AutoSize = true;
            this.ckNeedAutoGuid.Location = new System.Drawing.Point(152, 213);
            this.ckNeedAutoGuid.Name = "ckNeedAutoGuid";
            this.ckNeedAutoGuid.Size = new System.Drawing.Size(96, 16);
            this.ckNeedAutoGuid.TabIndex = 9;
            this.ckNeedAutoGuid.Text = "需要自增GUID";
            this.ckNeedAutoGuid.UseVisualStyleBackColor = true;
            // 
            // ckProtoBuf
            // 
            this.ckProtoBuf.AutoSize = true;
            this.ckProtoBuf.Location = new System.Drawing.Point(6, 213);
            this.ckProtoBuf.Name = "ckProtoBuf";
            this.ckProtoBuf.Size = new System.Drawing.Size(120, 16);
            this.ckProtoBuf.TabIndex = 8;
            this.ckProtoBuf.Text = "Dto ProtoBuf特性";
            this.ckProtoBuf.UseVisualStyleBackColor = true;
            this.ckProtoBuf.CheckedChanged += new System.EventHandler(this.ckProtoBuf_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Model命名空间:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "子模块名称:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 191);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "实体类名称:";
            // 
            // tpOther
            // 
            this.tpOther.Controls.Add(this.txtAuthor);
            this.tpOther.Controls.Add(this.label4);
            this.tpOther.Location = new System.Drawing.Point(4, 22);
            this.tpOther.Name = "tpOther";
            this.tpOther.Padding = new System.Windows.Forms.Padding(3);
            this.tpOther.Size = new System.Drawing.Size(338, 304);
            this.tpOther.TabIndex = 2;
            this.tpOther.Text = "其他";
            this.tpOther.UseVisualStyleBackColor = true;
            // 
            // txtAuthor
            // 
            this.txtAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAuthor.Location = new System.Drawing.Point(77, 6);
            this.txtAuthor.Name = "txtAuthor";
            this.txtAuthor.Size = new System.Drawing.Size(255, 21);
            this.txtAuthor.TabIndex = 5;
            this.txtAuthor.Text = "WayneChen";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "作者:";
            // 
            // btnGenerateEntity
            // 
            this.btnGenerateEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateEntity.Enabled = false;
            this.btnGenerateEntity.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGenerateEntity.Location = new System.Drawing.Point(237, 334);
            this.btnGenerateEntity.Name = "btnGenerateEntity";
            this.btnGenerateEntity.Size = new System.Drawing.Size(105, 44);
            this.btnGenerateEntity.TabIndex = 1;
            this.btnGenerateEntity.Text = "生成";
            this.btnGenerateEntity.UseVisualStyleBackColor = true;
            this.btnGenerateEntity.Click += new System.EventHandler(this.btnGenerateEntity_Click);
            // 
            // tabCodes
            // 
            this.tabCodes.Controls.Add(this.tpEntityCode);
            this.tabCodes.Controls.Add(this.tpQueryCode);
            this.tabCodes.Controls.Add(this.tpEntityDto);
            this.tabCodes.Controls.Add(this.tpMapperExtension);
            this.tabCodes.Controls.Add(this.tpRepository);
            this.tabCodes.Controls.Add(this.tpBusiness);
            this.tabCodes.Controls.Add(this.tpIServices);
            this.tabCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCodes.Location = new System.Drawing.Point(0, 0);
            this.tabCodes.Name = "tabCodes";
            this.tabCodes.SelectedIndex = 0;
            this.tabCodes.Size = new System.Drawing.Size(665, 612);
            this.tabCodes.TabIndex = 0;
            // 
            // tpEntityCode
            // 
            this.tpEntityCode.Location = new System.Drawing.Point(4, 22);
            this.tpEntityCode.Name = "tpEntityCode";
            this.tpEntityCode.Padding = new System.Windows.Forms.Padding(3);
            this.tpEntityCode.Size = new System.Drawing.Size(657, 586);
            this.tpEntityCode.TabIndex = 0;
            this.tpEntityCode.Text = "实体类";
            this.tpEntityCode.UseVisualStyleBackColor = true;
            // 
            // tpQueryCode
            // 
            this.tpQueryCode.Location = new System.Drawing.Point(4, 22);
            this.tpQueryCode.Name = "tpQueryCode";
            this.tpQueryCode.Padding = new System.Windows.Forms.Padding(3);
            this.tpQueryCode.Size = new System.Drawing.Size(657, 586);
            this.tpQueryCode.TabIndex = 5;
            this.tpQueryCode.Text = "查询实体";
            this.tpQueryCode.UseVisualStyleBackColor = true;
            // 
            // tpEntityDto
            // 
            this.tpEntityDto.Location = new System.Drawing.Point(4, 22);
            this.tpEntityDto.Name = "tpEntityDto";
            this.tpEntityDto.Padding = new System.Windows.Forms.Padding(3);
            this.tpEntityDto.Size = new System.Drawing.Size(657, 586);
            this.tpEntityDto.TabIndex = 6;
            this.tpEntityDto.Text = "ViewModel";
            this.tpEntityDto.UseVisualStyleBackColor = true;
            // 
            // tpMapperExtension
            // 
            this.tpMapperExtension.Location = new System.Drawing.Point(4, 22);
            this.tpMapperExtension.Name = "tpMapperExtension";
            this.tpMapperExtension.Padding = new System.Windows.Forms.Padding(3);
            this.tpMapperExtension.Size = new System.Drawing.Size(657, 586);
            this.tpMapperExtension.TabIndex = 7;
            this.tpMapperExtension.Text = "MapperExtension";
            this.tpMapperExtension.UseVisualStyleBackColor = true;
            // 
            // tpRepository
            // 
            this.tpRepository.Location = new System.Drawing.Point(4, 22);
            this.tpRepository.Name = "tpRepository";
            this.tpRepository.Padding = new System.Windows.Forms.Padding(3);
            this.tpRepository.Size = new System.Drawing.Size(657, 586);
            this.tpRepository.TabIndex = 8;
            this.tpRepository.Text = "Repository";
            this.tpRepository.UseVisualStyleBackColor = true;
            // 
            // tpBusiness
            // 
            this.tpBusiness.Location = new System.Drawing.Point(4, 22);
            this.tpBusiness.Name = "tpBusiness";
            this.tpBusiness.Padding = new System.Windows.Forms.Padding(3);
            this.tpBusiness.Size = new System.Drawing.Size(657, 586);
            this.tpBusiness.TabIndex = 9;
            this.tpBusiness.Text = "Business";
            this.tpBusiness.UseVisualStyleBackColor = true;
            // 
            // tpIServices
            // 
            this.tpIServices.Location = new System.Drawing.Point(4, 22);
            this.tpIServices.Name = "tpIServices";
            this.tpIServices.Padding = new System.Windows.Forms.Padding(3);
            this.tpIServices.Size = new System.Drawing.Size(657, 586);
            this.tpIServices.TabIndex = 10;
            this.tpIServices.Text = "Interface Service";
            this.tpIServices.UseVisualStyleBackColor = true;
            // 
            // tpDataAccessCode
            // 
            this.tpDataAccessCode.Location = new System.Drawing.Point(4, 22);
            this.tpDataAccessCode.Name = "tpDataAccessCode";
            this.tpDataAccessCode.Padding = new System.Windows.Forms.Padding(3);
            this.tpDataAccessCode.Size = new System.Drawing.Size(507, 461);
            this.tpDataAccessCode.TabIndex = 4;
            this.tpDataAccessCode.Text = "数据访问类";
            this.tpDataAccessCode.UseVisualStyleBackColor = true;
            // 
            // tpManagerCode
            // 
            this.tpManagerCode.Location = new System.Drawing.Point(4, 22);
            this.tpManagerCode.Name = "tpManagerCode";
            this.tpManagerCode.Padding = new System.Windows.Forms.Padding(3);
            this.tpManagerCode.Size = new System.Drawing.Size(507, 461);
            this.tpManagerCode.TabIndex = 1;
            this.tpManagerCode.Text = "管理类";
            this.tpManagerCode.UseVisualStyleBackColor = true;
            // 
            // tpStoredProceduresCode
            // 
            this.tpStoredProceduresCode.Location = new System.Drawing.Point(4, 22);
            this.tpStoredProceduresCode.Name = "tpStoredProceduresCode";
            this.tpStoredProceduresCode.Padding = new System.Windows.Forms.Padding(3);
            this.tpStoredProceduresCode.Size = new System.Drawing.Size(507, 461);
            this.tpStoredProceduresCode.TabIndex = 3;
            this.tpStoredProceduresCode.Text = "存储过程";
            this.tpStoredProceduresCode.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 659);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "代码生成工具";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.scCenter.Panel1.ResumeLayout(false);
            this.scCenter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scCenter)).EndInit();
            this.scCenter.ResumeLayout(false);
            this.tabCodeGenderType.ResumeLayout(false);
            this.tpEntity.ResumeLayout(false);
            this.tpEntity.PerformLayout();
            this.tpOther.ResumeLayout(false);
            this.tpOther.PerformLayout();
            this.tabCodes.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView tvTables;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.Button btnGenerateEntity;
        private System.Windows.Forms.SplitContainer scCenter;
        private System.Windows.Forms.TabControl tabCodes;
        private System.Windows.Forms.TabPage tpEntityCode;
        private System.Windows.Forms.TabPage tpManagerCode;
        private System.Windows.Forms.TabPage tpStoredProceduresCode;
        private System.Windows.Forms.TabPage tpDataAccessCode;
        private System.Windows.Forms.ToolStripButton tsbConnect;
        private System.Windows.Forms.ToolStripButton tsbExportDBDoc;
        private System.Windows.Forms.ToolStripButton tsbExportMarkCode;
        private System.Windows.Forms.ListView lvColumns;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chIsPrimaryKey;
        private System.Windows.Forms.ColumnHeader chTypeName;
        private System.Windows.Forms.ColumnHeader chRemarks;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TabPage tpQueryCode;
        private System.Windows.Forms.ColumnHeader chTtile;
        private System.Windows.Forms.ColumnHeader condition;
        private System.Windows.Forms.TabPage tpEntityDto;
        private System.Windows.Forms.TabPage tpMapperExtension;
        private System.Windows.Forms.TabPage tpRepository;
        private System.Windows.Forms.TabPage tpBusiness;
        private System.Windows.Forms.TabPage tpIServices;
        private System.Windows.Forms.TabControl tabCodeGenderType;
        private System.Windows.Forms.TabPage tpEntity;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox txtDtoNameSpace;
        private System.Windows.Forms.TextBox txtBllNameSpace;
        private System.Windows.Forms.TextBox txtDalNameSpace;
        private System.Windows.Forms.TextBox txtSubClassName;
        private System.Windows.Forms.TextBox txtModelNameSpace;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox ckbDbIndex;
        private System.Windows.Forms.CheckBox ckbServerIndex;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox ckPartial;
        private System.Windows.Forms.CheckBox ckbPreNameSpace;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox ckParamCache;
        private System.Windows.Forms.CheckBox ckSingletonBLL;
        private System.Windows.Forms.CheckBox ckNeedAutoGuid;
        private System.Windows.Forms.CheckBox ckProtoBuf;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tpOther;
        private System.Windows.Forms.TextBox txtAuthor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtQueryNameSpace;
        private System.Windows.Forms.Label label5;
    }
}

