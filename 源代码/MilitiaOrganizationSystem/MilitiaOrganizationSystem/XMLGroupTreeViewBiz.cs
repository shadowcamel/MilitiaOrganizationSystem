using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilitiaOrganizationSystem
{
    class XMLGroupTreeViewBiz
    {//处理treeView上的增删改(与数据相关的部分)
        private XMLGroupDao xmlGroupDao;//xml访问层
        private TreeView groups_treeView;//树视图
        

        public XMLGroupTreeViewBiz(TreeView groups_TreeView, string xmlGroupFile)
        {//构造函数
            xmlGroupDao = new XMLGroupDao(xmlGroupFile);
            this.groups_treeView = groups_TreeView;
        }

        public void addUnderSelectedNode()
        {//在选中节点的下面添加组
            
            TreeNode selectNode = groups_treeView.SelectedNode;
            if (selectNode == null)
            {//如果没有选中节点，则从根节点添加组
                addRoot();

            }
            else
            {
                TreeNode node = new TreeNode("新建组");

                selectNode.Nodes.Add(node);
                selectNode.Expand();//增加后，展开

                GroupTag tag = (GroupTag)(selectNode.Tag);
                node.Tag = xmlGroupDao.addGroupFromParent(tag.tagXmlNode, "新建组");
                node.ToolTipText = xmlGroupDao.getToolTipText(((GroupTag)node.Tag).tagXmlNode);

                groups_treeView.SelectedNode = node;//选中新建的组
                node.BeginEdit();//开始编辑名称
                
            }
            
        }

        public void addRoot()
        {//增加根节点
            
            TreeNode node = new TreeNode("新建组");
            groups_treeView.Nodes.Add(node);
            groups_treeView.SelectedNode = node;
            
            node.Tag = xmlGroupDao.addGroupFromRoot("新建组");
            node.ToolTipText = xmlGroupDao.getToolTipText(((GroupTag)node.Tag).tagXmlNode);

            node.BeginEdit();
        }

        public void modifyName(TreeNode node, string newName)
        {//编辑组名
            GroupTag tag = (GroupTag)(node.Tag);
            xmlGroupDao.modifyGroupName(tag.tagXmlNode, newName);

        }

        public void modifyAll(TreeNode node, Dictionary<string, string> newAttributes)
        {//编辑所有属性
            GroupTag tag = (GroupTag)(node.Tag);
            xmlGroupDao.modifyAllAttribute(tag.tagXmlNode, newAttributes);
        }

        public void deleSelectedNode()
        {//删除选中的节点
            TreeNode selectNode = groups_treeView.SelectedNode;
            if (selectNode == null)
            {
                return;
            }
            if (selectNode.Nodes.Count > 0)
            {//需要被删除的节点下有子节点，则提示警告是否删除
                DialogResult re = MessageBox.Show("此节点下有子节点，确认是否删除此组及此组下的所有组？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (re == DialogResult.Cancel)
                {
                    return;
                }
            }
            //删除，不仅要删除xml文件中的对应项，还要删除数据库中的分组
            GroupTag tag = (GroupTag)(selectNode.Tag);
            xmlGroupDao.deleteGroup(tag.tagXmlNode);//删除xml里面
            selectNode.Nodes.Remove(selectNode);//treeview中删除
        }

        public void loadGroupXmlFile(string xmlFile)
        {//将xml文件加载到treeView中
            xmlGroupDao.loadXMLFileToTreeView(xmlFile, groups_treeView);
            groups_treeView.ExpandAll();
        }

        public void refresh()
        {//刷新，同步xml文件与treeView
            groups_treeView.Nodes.Clear();
            xmlGroupDao.loadToTreeView(groups_treeView);
            groups_treeView.ExpandAll();
        }

        public void addXmlGroupTask(string xmlFile)
        {//添加分组任务
            xmlGroupDao.addXml(xmlFile);
            refresh();
        }

        public bool allowDropAt(TreeNode treeNode)
        {//判断是否允许拖放到这个节点
            if(treeNode.Tag == null)
            {//这是个民兵节点
                return false;
            }
            GroupTag tag = (GroupTag)treeNode.Tag;
            if(tag.militias != null)
            {//不为空，说明可以分组
                return true;
            }
            return false;
        }

        public TreeNode getTreeNodeByText(TreeNodeCollection Nodes, string text)
        {
            foreach(TreeNode treeNode in Nodes)
            {
                if(treeNode.Text == text)
                {
                    return treeNode;
                }
            }
            return null;
        }

        public TreeNode getTreeNodeByText(string text)
        {
            string[] groups = text.Split(new Char[] { '/' });
            TreeNodeCollection Nodes = groups_treeView.Nodes;
            TreeNode treeNode = null;
            foreach (string groupName in groups)
            {
                treeNode = getTreeNodeByText(Nodes, groupName);
                if(treeNode == null)
                {
                    return null;
                }
                Nodes = treeNode.Nodes;
            }

            return treeNode;
        }
    }
}
