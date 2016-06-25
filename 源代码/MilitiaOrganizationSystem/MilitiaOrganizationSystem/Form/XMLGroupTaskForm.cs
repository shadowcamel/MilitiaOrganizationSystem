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

            groups_treeView.NodeMouseDoubleClick += Groups_treeView_NodeMouseDoubleClick;

            groups_treeView.DragEnter += Groups_treeView_DragEnter;
            groups_treeView.DragOver += Groups_treeView_DragOver;
            groups_treeView.DragDrop += Groups_treeView_DragDrop;

            //groups_treeView.ItemDrag += Groups_treeView_ItemDrag;
            
        }

        private void Groups_treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode tn = e.Node;
            tn.Toggle();
            GroupMilitiaForm gm = new GroupMilitiaForm(BasicLevelForm.sqlBiz, tn.Name);
            gm.Show();
        }

        /***private void Groups_treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {//拖动treeView的节点触发
            TreeNode treeNode = (TreeNode)e.Item;
            groups_treeView.SelectedNode = treeNode;
            if(treeNode.Tag == null)
            {//是民兵节点,则拖动
                GroupTag tag = (GroupTag)treeNode.Parent.Tag;//父节点，即组的tag
                int index = treeNode.Parent.Nodes.IndexOf(treeNode);//获取子节点在父节点的index，也是民兵的index
                Militia militia = tag.militias[index];//获取此节点代表的民兵
                MoveTag mt = new MoveTag(this, new List<Militia> { militia });
                if (groups_treeView.DoDragDrop(mt, DragDropEffects.Move) == DragDropEffects.Move)
                {//移动成功,应删除此节点
                    tag.militias.Remove(militia);
                    treeNode.Remove();
                };
                
            }
        }*/

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
                    //MilitiaReflection mr = new MilitiaReflection(militia);//反射
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
                                //int index = groupTag.militias.IndexOf(militia);
                                //MessageBox.Show(groupTag.militias.Count +"");
                                /***int index = groupTag.militias.FindIndex(delegate (Militia m) {//不同session查询出的militia对象不是同一个,故根据Id判断
                                    if (m.Id == militia.Id)
                                    {
                                        return true;
                                    }
                                    return false;
                                });
                                if (index >= 0)
                                {
                                    groupTag.militias.RemoveAt(index);
                                    groupNode.Nodes.RemoveAt(index);
                                }*/
                                ///减少组节点显示的民兵数量

                                //通知MilitiaForm更改分组
                                militia.Group = groupNode.Name;
                                //((BasicLevelForm)Program.formDic["MilitiaForm"]).updateMilitiaItem(militia);
                                FormBizs.updateMilitiaItem(militia);
                            }
                            
                            
                        }
                    }
                    militia.Group = node.Name;
                    xmlGroupBiz.addCount(node, 1);
                    BasicLevelForm.sqlBiz.updateMilitia(militia);//保存分组
                    //此时此组中一定没有这个对象，前面如果民兵已有分组，到此时已经被删除
                    //tag.militias.Add(militia);//添加到组的tag中
                    //groups_treeView.SelectedNode = node.Nodes.Add(militia.info());//选中它
                }
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

            xmlGroupBiz = new XMLGroupTreeViewBiz(groups_treeView, BasicLevelForm.xmlGroupFile, BasicLevelForm.sqlBiz);
            xmlGroupBiz.refresh();//加载xml分组文件
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
            GroupMilitiaForm gm = new GroupMilitiaForm(BasicLevelForm.sqlBiz, selectNode.Name);
            gm.Show();

        }

        private void addRoot_Click(object sender, EventArgs e)
        {
            xmlGroupBiz.addRoot();
        }

        public void addXmlGroupTask(string xmlFile)
        {
            xmlGroupBiz.addXmlGroupTask(xmlFile);//从文件中增加分组任务
        }

        /*public void updateMilitiaNode(Militia militia)
        {//改变一个民兵的信息（可能在编辑界面被更改了信息）,函数被编辑页面调用
            TreeNode groupNode = xmlGroupBiz.getTreeNodeByText(militia.Group);//找到他原来的组节点
            if(groupNode == null)
            {
                return;
            }
            GroupTag groupTag = (GroupTag)groupNode.Tag;
            int index = groupTag.militias.FindIndex(delegate (Militia m) {//不同session查询出的militia对象不是同一个,故根据Id判断
                if (m.Id == militia.Id)
                {
                    return true;
                }
                return false;
            });
            if (index >= 0)
            {
                groupTag.militias[index] = militia;
                groupNode.Nodes[index].Text = militia.info();
            }
        }*/

        
    }
}
