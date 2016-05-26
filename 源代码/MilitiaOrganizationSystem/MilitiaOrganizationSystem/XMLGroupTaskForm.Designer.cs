
using System.Windows.Forms;

namespace MilitiaOrganizationSystem
{
    partial class XMLGroupTaskForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XMLGroupTaskForm));
            this.groups_treeView = new System.Windows.Forms.TreeView();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menu = new System.Windows.Forms.ToolStripMenuItem();
            this.view = new System.Windows.Forms.ToolStripMenuItem();
            this.add = new System.Windows.Forms.ToolStripMenuItem();
            this.edit = new System.Windows.Forms.ToolStripMenuItem();
            this.dele = new System.Windows.Forms.ToolStripMenuItem();
            this.addRoot = new System.Windows.Forms.ToolStripMenuItem();
            this.rMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.view2 = new System.Windows.Forms.ToolStripMenuItem();
            this.edit2 = new System.Windows.Forms.ToolStripMenuItem();
            this.add2 = new System.Windows.Forms.ToolStripMenuItem();
            this.dele2 = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip.SuspendLayout();
            this.rMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // groups_treeView
            // 
            this.groups_treeView.AllowDrop = true;
            this.groups_treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groups_treeView.HotTracking = true;
            this.groups_treeView.ImageIndex = 0;
            this.groups_treeView.ImageList = this.treeViewImageList;
            this.groups_treeView.LabelEdit = true;
            this.groups_treeView.Location = new System.Drawing.Point(-1, 27);
            this.groups_treeView.Name = "groups_treeView";
            this.groups_treeView.SelectedImageIndex = 0;
            this.groups_treeView.ShowNodeToolTips = true;
            this.groups_treeView.Size = new System.Drawing.Size(818, 518);
            this.groups_treeView.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(816, 25);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "菜单流";
            // 
            // menu
            // 
            this.menu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.view,
            this.add,
            this.edit,
            this.dele,
            this.addRoot});
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(44, 21);
            this.menu.Text = "菜单";
            // 
            // view
            // 
            this.view.Name = "view";
            this.view.Size = new System.Drawing.Size(148, 22);
            this.view.Text = "查看";
            // 
            // add
            // 
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(148, 22);
            this.add.Text = "添加组";
            // 
            // edit
            // 
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(148, 22);
            this.edit.Text = "编辑组名";
            // 
            // dele
            // 
            this.dele.Name = "dele";
            this.dele.Size = new System.Drawing.Size(148, 22);
            this.dele.Text = "删除组";
            // 
            // addRoot
            // 
            this.addRoot.Name = "addRoot";
            this.addRoot.Size = new System.Drawing.Size(148, 22);
            this.addRoot.Text = "新建根节点组";
            // 
            // rMenu
            // 
            this.rMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.view2,
            this.edit2,
            this.add2,
            this.dele2});
            this.rMenu.Name = "rMenu";
            this.rMenu.Size = new System.Drawing.Size(173, 92);
            // 
            // view2
            // 
            this.view2.Name = "view2";
            this.view2.Size = new System.Drawing.Size(172, 22);
            this.view2.Text = "查看";
            // 
            // edit2
            // 
            this.edit2.Name = "edit2";
            this.edit2.Size = new System.Drawing.Size(172, 22);
            this.edit2.Text = "编辑组名";
            // 
            // add2
            // 
            this.add2.Name = "add2";
            this.add2.Size = new System.Drawing.Size(172, 22);
            this.add2.Text = "在此组下添加新组";
            // 
            // dele2
            // 
            this.dele2.Name = "dele2";
            this.dele2.Size = new System.Drawing.Size(172, 22);
            this.dele2.Text = "删除此组";
            // 
            // treeViewImageList
            // 
            this.treeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewImageList.ImageStream")));
            this.treeViewImageList.TransparentColor = System.Drawing.Color.Maroon;
            this.treeViewImageList.Images.SetKeyName(0, "u=1978385908,3347853851&fm=21&gp=0.jpg");
            // 
            // XMLGroupTaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 542);
            this.Controls.Add(this.groups_treeView);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "XMLGroupTaskForm";
            this.Text = "分组任务";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.rMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.TreeView groups_treeView;//树控件
        private System.Windows.Forms.ContextMenuStrip rMenu;//右键菜单
        private MenuStrip menuStrip;
        private ToolStripMenuItem menu;
        private ToolStripMenuItem view;
        private ToolStripMenuItem add;
        private ToolStripMenuItem edit;
        private ToolStripMenuItem dele;
        private ToolStripMenuItem addRoot;
        private ToolStripMenuItem view2;
        private ToolStripMenuItem edit2;
        private ToolStripMenuItem add2;
        private ToolStripMenuItem dele2;
        private ImageList treeViewImageList;
    }


}