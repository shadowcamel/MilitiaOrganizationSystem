namespace MilitiaOrganizationSystem
{
    partial class BasicLevelForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BasicLevelForm));
            this.militia_ListView = new System.Windows.Forms.ListView();
            this.rMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.rAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.rEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.rDele = new System.Windows.Forms.ToolStripMenuItem();
            this.militiaImageList = new System.Windows.Forms.ImageList(this.components);
            this.menu_basicLevel = new System.Windows.Forms.MenuStrip();
            this.btn_militaInfomation = new System.Windows.Forms.ToolStripMenuItem();
            this.add = new System.Windows.Forms.ToolStripMenuItem();
            this.modify = new System.Windows.Forms.ToolStripMenuItem();
            this.dele = new System.Windows.Forms.ToolStripMenuItem();
            this.importFromXml = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_importXMLGroupTask = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rMenuStrip.SuspendLayout();
            this.menu_basicLevel.SuspendLayout();
            this.SuspendLayout();
            // 
            // militia_ListView
            // 
            this.militia_ListView.AllowColumnReorder = true;
            this.militia_ListView.AllowDrop = true;
            this.militia_ListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.militia_ListView.ContextMenuStrip = this.rMenuStrip;
            this.militia_ListView.FullRowSelect = true;
            this.militia_ListView.Location = new System.Drawing.Point(0, 55);
            this.militia_ListView.Name = "militia_ListView";
            this.militia_ListView.ShowItemToolTips = true;
            this.militia_ListView.Size = new System.Drawing.Size(850, 541);
            this.militia_ListView.SmallImageList = this.militiaImageList;
            this.militia_ListView.TabIndex = 0;
            this.militia_ListView.UseCompatibleStateImageBehavior = false;
            this.militia_ListView.View = System.Windows.Forms.View.Details;
            // 
            // rMenuStrip
            // 
            this.rMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rAdd,
            this.rEdit,
            this.rDele});
            this.rMenuStrip.Name = "rMenuStrip";
            this.rMenuStrip.Size = new System.Drawing.Size(101, 70);
            // 
            // rAdd
            // 
            this.rAdd.Name = "rAdd";
            this.rAdd.Size = new System.Drawing.Size(100, 22);
            this.rAdd.Text = "添加";
            this.rAdd.Click += new System.EventHandler(this.rAdd_Click);
            // 
            // rEdit
            // 
            this.rEdit.Name = "rEdit";
            this.rEdit.Size = new System.Drawing.Size(100, 22);
            this.rEdit.Text = "编辑";
            this.rEdit.Click += new System.EventHandler(this.rEdit_Click);
            // 
            // rDele
            // 
            this.rDele.Name = "rDele";
            this.rDele.Size = new System.Drawing.Size(100, 22);
            this.rDele.Text = "删除";
            this.rDele.Click += new System.EventHandler(this.rDele_Click);
            // 
            // militiaImageList
            // 
            this.militiaImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("militiaImageList.ImageStream")));
            this.militiaImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.militiaImageList.Images.SetKeyName(0, "u=1978385908,3347853851&fm=21&gp=0.jpg");
            // 
            // menu_basicLevel
            // 
            this.menu_basicLevel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_militaInfomation,
            this.btn_importXMLGroupTask});
            this.menu_basicLevel.Location = new System.Drawing.Point(0, 0);
            this.menu_basicLevel.Name = "menu_basicLevel";
            this.menu_basicLevel.Size = new System.Drawing.Size(850, 25);
            this.menu_basicLevel.TabIndex = 2;
            this.menu_basicLevel.Text = "basicLevelMenuStrip";
            // 
            // btn_militaInfomation
            // 
            this.btn_militaInfomation.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.add,
            this.modify,
            this.dele,
            this.importFromXml});
            this.btn_militaInfomation.Name = "btn_militaInfomation";
            this.btn_militaInfomation.Size = new System.Drawing.Size(68, 21);
            this.btn_militaInfomation.Text = "民兵信息";
            // 
            // add
            // 
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(168, 22);
            this.add.Text = "添加";
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // modify
            // 
            this.modify.Name = "modify";
            this.modify.Size = new System.Drawing.Size(168, 22);
            this.modify.Text = "编辑";
            this.modify.Click += new System.EventHandler(this.modify_Click);
            // 
            // dele
            // 
            this.dele.Name = "dele";
            this.dele.Size = new System.Drawing.Size(168, 22);
            this.dele.Text = "删除";
            this.dele.Click += new System.EventHandler(this.dele_Click);
            // 
            // importFromXml
            // 
            this.importFromXml.Name = "importFromXml";
            this.importFromXml.Size = new System.Drawing.Size(168, 22);
            this.importFromXml.Text = "从xml文件中导入";
            this.importFromXml.Click += new System.EventHandler(this.importFromXml_Click);
            // 
            // btn_importXMLGroupTask
            // 
            this.btn_importXMLGroupTask.Name = "btn_importXMLGroupTask";
            this.btn_importXMLGroupTask.Size = new System.Drawing.Size(112, 21);
            this.btn_importXMLGroupTask.Text = "导入xml分组任务";
            this.btn_importXMLGroupTask.Click += new System.EventHandler(this.importXMLGroupTask_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(83, 28);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(767, 21);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "未分组";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "筛选条件：";
            // 
            // BasicLevelForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 596);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.militia_ListView);
            this.Controls.Add(this.menu_basicLevel);
            this.Name = "BasicLevelForm";
            this.Text = "基层主页";
            this.Load += new System.EventHandler(this.BasicLevelForm_Load);
            this.rMenuStrip.ResumeLayout(false);
            this.menu_basicLevel.ResumeLayout(false);
            this.menu_basicLevel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView militia_ListView;
        private System.Windows.Forms.MenuStrip menu_basicLevel;
        private System.Windows.Forms.ToolStripMenuItem btn_militaInfomation;
        private System.Windows.Forms.ToolStripMenuItem btn_importXMLGroupTask;
        private System.Windows.Forms.ToolStripMenuItem add;
        private System.Windows.Forms.ToolStripMenuItem modify;
        private System.Windows.Forms.ToolStripMenuItem dele;
        private System.Windows.Forms.ToolStripMenuItem importFromXml;
        private System.Windows.Forms.ImageList militiaImageList;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip rMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem rAdd;
        private System.Windows.Forms.ToolStripMenuItem rEdit;
        private System.Windows.Forms.ToolStripMenuItem rDele;
    }
}

