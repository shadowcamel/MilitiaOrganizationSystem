﻿using System;
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
        private SqlBiz sqlBiz;

        public XMLGroupTaskForm()
        {//构造函数
            InitializeComponent();
            bindEvent();

            this.ControlBox = false;//不要最大化最小化以及×
            sqlBiz = FormBizs.sqlBiz;
            xmlGroupBiz = new XMLGroupTreeViewBiz(groups_treeView, sqlBiz);
            xmlGroupBiz.refresh();//加载xml分组文件
        }


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

            groups_treeView.NodeMouseDoubleClick += Groups_treeView_NodeMouseDoubleClick;

            groups_treeView.DragEnter += Groups_treeView_DragEnter;
            groups_treeView.DragOver += Groups_treeView_DragOver;
            groups_treeView.DragDrop += Groups_treeView_DragDrop;
            
        }

        private void Groups_treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode tn = e.Node;
            tn.Toggle();
            GroupMilitiaForm gm = new GroupMilitiaForm(sqlBiz, tn.Name);
            gm.Show();
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
            {//已经允许放时,必定已经选中了一个节点,所以node不为空; 现在已经放下，表示move
                MoveTag mt = (MoveTag)e.Data.GetData(typeof(MoveTag));
                List<Militia> mList = mt.moveMilitias;
                foreach(Militia militia in mList)
                {
                    if(militia.Id == null)
                    {//删除后的民兵来分组
                        if (MessageBox.Show("民兵：" + militia.info() + " 已经被删除，是否恢复它并继续操作？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            militia.Group = "未分组";
                            sqlBiz.addMilitia(militia);
                            //相当于新建一个民兵，并分组
                        } else
                        {
                            continue;
                        }
                    }
                    
                    if(militia.Group == node.Name)
                    {//分组本来就是它,则无需操作
                        e.Effect = DragDropEffects.None;
                        continue;
                    }
                    if(militia.Group != "未分组")
                    {//不是从未分组来分组,则需要将它从原来的组删去，故弹出对话框确认
                        DialogResult re = MessageBox.Show(militia.Name + "已有分组为" + militia.Group + ", 是否更改其分组？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if(re == DialogResult.Cancel)
                        {
                            e.Effect = DragDropEffects.None;
                            continue;
                        } else if(re == DialogResult.OK)
                        {//ok时，还要将militia从原来的组中删除，也在这个界面
                            TreeNode groupNode = xmlGroupBiz.getTreeNodeByText(militia.Group);//找到他原来的组节点
                            if(groupNode != null)
                            {
                                xmlGroupBiz.reduceCount(groupNode, 1);//减少数量
                            }
                            
                            
                        }
                    }
                    militia.Group = node.Name;
                    xmlGroupBiz.addCount(node, 1);
                    sqlBiz.updateMilitia(militia);//保存分组
                    //通知MilitiaForm更改分组
                    FormBizs.updateMilitiaItem(militia);
                }
            }
        }

        private void Groups_treeView_DragEnter(object sender, DragEventArgs e)
        {
            this.Focus();
            e.Effect = DragDropEffects.None;
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
            //查看民兵
            GroupMilitiaForm gm = new GroupMilitiaForm(sqlBiz, selectNode.Name);
            gm.Show();

        }

        private void addRoot_Click(object sender, EventArgs e)
        {
            xmlGroupBiz.addRoot();
        }
    }
}
