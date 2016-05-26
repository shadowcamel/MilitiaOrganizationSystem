using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilitiaOrganizationSystem
{
    public partial class XMLGroupTaskForm : Form
    {
        private XMLGroupTreeViewBiz xmlGroupBiz;


        private void bindEvent()
        {
            view.Click += View_Click;
            view2.Click += View_Click;
            add.Click += Add_Click;
            add2.Click += Add_Click;
            dele.Click += Dele_Click;
            dele2.Click += Dele_Click;
            edit.Click += Edit_Click;
            edit2.Click += Edit_Click;
            addRoot.Click += addRoot_Click;

            groups_treeView.MouseClick += Groups_treeView_MouseClick;
            groups_treeView.AfterLabelEdit += Groups_treeView_AfterLabelEdit;

            groups_treeView.DragEnter += Groups_treeView_DragEnter;
            groups_treeView.DragOver += Groups_treeView_DragOver;
            groups_treeView.DragDrop += Groups_treeView_DragDrop;
            
        }

        private void Groups_treeView_DragOver(object sender, DragEventArgs e)
        {
            TreeNode node = groups_treeView.GetNodeAt(e.X - this.Location.X - groups_treeView.Location.X, e.Y - this.Location.Y - groups_treeView.Location.Y - menuStrip.Size.Height);
            
            
            if(node == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            groups_treeView.SelectedNode = node;//选中节点
            if(!xmlGroupBiz.allowDropAt(node))
            {//不允许拖放到此节点
                e.Effect = DragDropEffects.None;
            } else
            {//允许拖放到此节点
                e.Effect = DragDropEffects.Move;
            }
        }

        private void Groups_treeView_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode node = groups_treeView.SelectedNode;
            if(e.Effect == DragDropEffects.Move)
            {//已经允许放时
                GroupTag tag = (GroupTag)node.Tag;

                ListView militia_ListView = (ListView)e.Data.GetData(typeof(ListView));
                ListView.SelectedListViewItemCollection selectedItems = militia_ListView.SelectedItems;
                foreach(ListViewItem item in selectedItems)
                {
                    militia_ListView.Items.Remove(item);
                    Militia militia = (Militia)item.Tag;
                    militia.group = tag.groupPath;
                    tag.militias.Add(militia);//添加到组的tag中
                    node.Nodes.Add(militia.info());
                }
                BasicLevelForm.sqlBiz.saveChanges();//保存改变
            }
        }

        private void Groups_treeView_DragEnter(object sender, DragEventArgs e)
        {
            this.Focus();
            e.Effect = DragDropEffects.None;
        }

        public XMLGroupTaskForm(string xmlFile)
        {//构造函数
            InitializeComponent();
            bindEvent();

            this.ControlBox = false;//不要最大化最小化以及×

            xmlGroupBiz = new XMLGroupTreeViewBiz(groups_treeView, BasicLevelForm.xmlGroupFile);
            //xmlGroupBiz.loadGroupXmlFile(xmlFile);//加载xml文件
            xmlGroupBiz.refresh();
        }

        private void Groups_treeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {//右键单击
                TreeNode node = groups_treeView.GetNodeAt(e.X, e.Y);
                
                if (node == null)
                {//无节点
                    return;
                } else
                {
                    groups_treeView.SelectedNode = node;
                    rMenu.Show(groups_treeView, e.X, e.Y);
                }
            }
        }

        private void Groups_treeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {//编辑之后，同步修改xml文件和数据
            if(e.Label == null)
            {//根本就没有编辑过
                return;
            }
            xmlGroupBiz.modifyName(e.Node, e.Label);

        }


        private void Dele_Click(object sender, System.EventArgs e)
        {//删除选中
            xmlGroupBiz.deleSelectedNode();
        }

        private void Add_Click(object sender, System.EventArgs e)
        {
            xmlGroupBiz.addUnderSelectedNode();
        }

        private void Edit_Click(object sender, System.EventArgs e)
        {
            TreeNode selectNode = groups_treeView.SelectedNode;
            if (selectNode == null)
            {
                return;
            }
            selectNode.BeginEdit();
        }

        private void View_Click(object sender, System.EventArgs e)
        {
            TreeNode selectNode = groups_treeView.SelectedNode;
            if (selectNode == null)
            {
                return;
            }
            //查看代码
        }

        private void addRoot_Click(object sender, EventArgs e)
        {
            xmlGroupBiz.addRoot();
        }

        public void addXmlGroupTask(string xmlFile)
        {
            xmlGroupBiz.addXmlGroupTask(xmlFile);//从文件中增加分组任务
        }
    }
}
