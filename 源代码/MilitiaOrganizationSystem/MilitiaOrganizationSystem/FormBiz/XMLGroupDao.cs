using Raven.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MilitiaOrganizationSystem
{
    class XMLGroupDao
    {//分组任务xml访问层

        private string xmlGroupFileName;
        private XmlDocument xmlDoc;
        private XmlNode rootNode;//根节点
        private SqlBiz sqlBiz;//数据库业务逻辑层

        private TreeView groups_TreeView;//想了想还是把这个加上吧，这个类的对象应该是绑定了一个TreeView的

        /**private long maxId;//组中最大id

        private void setId(XmlNode node)
        {
            XmlAttribute idAttr = xmlDoc.CreateAttribute("Id");
            idAttr.Value = maxId.ToString();
            maxId++;
            node.Attributes.SetNamedItem(idAttr);//添加id
        }*/

        private void saveXml()
        {//保存xml文件
            /**if(rootNode.Attributes["maxId"] == null)
            {
                XmlAttribute attr = xmlDoc.CreateAttribute("maxId");
                attr.Value = "";
                rootNode.Attributes.SetNamedItem(attr);
            }
            rootNode.Attributes["maxId"].Value = maxId.ToString();*/
            xmlDoc.Save(xmlGroupFileName);
        }

        public string getToolTipText(XmlNode node)
        {//返回鼠标移到节点上面所显示的东西
            string ToolTipText = "";
            foreach (XmlAttribute attr in node.Attributes)
            {
                ToolTipText += attr.Name + "=\"" + attr.Value + "\" ";
            }

            return ToolTipText;
        }

        private string getNodePath(XmlNode node)
        {
            string path = node.Attributes["name"].Value;
            while(node.ParentNode.Attributes["name"] != null)
            {
                path = node.ParentNode.Attributes["name"].Value + "/" + path;
                node = node.ParentNode;
            }
            return path;
        }
        
        public XMLGroupDao(string xmlGroupFile, SqlBiz sBiz, TreeView tv)
        {//构造函数,传进需要保存的xml文件名,之后就从这个文件中读取和保存
            this.xmlGroupFileName = xmlGroupFile;
            xmlDoc = new XmlDocument();
            if(File.Exists(xmlGroupFile))
            {//如果文件存在，则加载到xmlDoc中
                xmlDoc.Load(xmlGroupFile);
                rootNode = xmlDoc.DocumentElement;
            } else
            {//如果不存在，则新建
                XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDoc.AppendChild(dec);
                rootNode = xmlDoc.CreateElement("root");
                xmlDoc.AppendChild(rootNode);
            }
            sqlBiz = sBiz;//数据库业务逻辑层
            groups_TreeView = tv;
            /**maxId = 1;*/
        }

        public void loadToTreeView()
        {
            Dictionary<string, FacetValue> fdict = sqlBiz.getGroupNums();
            addXmlNodeToTreeNode(rootNode, groups_TreeView.Nodes, fdict);
        }

        public void loadXMLFileToTreeView(string xmlFile, TreeView groups_TreeView)
        {//将xml分组文件加载到treeView中
            xmlDoc.Load(xmlFile);
            rootNode = xmlDoc.DocumentElement;
            /**if(rootNode.Attributes["maxId"] != null)
            {
                maxId = long.Parse(rootNode.Attributes["maxId"].Value);
                //MessageBox.Show(maxId.ToString());
            }*/
            Dictionary<string, FacetValue> fDict = sqlBiz.getGroupNums();
            addXmlNodeToTreeNode(rootNode, groups_TreeView.Nodes, fDict);
            saveXml();
        }

        public void addCountUpToAllParent(TreeNode tn, int Count)
        {
            while(tn.Parent != null)
            {
                GroupTag gt = (GroupTag)tn.Parent.Tag;
                gt.Count += Count;
                tn.Parent.Text = gt.info();

                tn = tn.Parent;//迭代
            }
            
        }

        private void addXmlNodeToTreeNode(XmlNode root, TreeNodeCollection rootNodes, Dictionary<string, FacetValue> fDic)
        {//将root下的所有节点加载到treeView中
            foreach (XmlNode node in root.ChildNodes)
            {
                /**if (node.Attributes["Id"] == null)
                {
                    setId(node);//给节点增加Id属性
                }*/

                TreeNode treeNode = rootNodes.Add(node.Attributes["name"].Value);

                treeNode.ToolTipText = getToolTipText(node);

                treeNode.Name = getNodePath(node);//查找TreeNode的Key

                GroupTag tag = new GroupTag(node);

                treeNode.Text = tag.info();

                

                //tag.groupPath = getNodePath(node);//path

                if (!node.HasChildNodes)
                {//是叶节点,则获取此组下的民兵，并将民兵添加到treeView中
                    //应该是获取数量，并显示到节点上
                    FacetValue fv;
                    if(fDic.TryGetValue(treeNode.Name, out fv))
                    {
                        tag.Count += fv.Hits;//本身加
                        addCountUpToAllParent(treeNode, fv.Hits);//所有父节点加
                    }    
                    
                    treeNode.Text = tag.info();
                    /*try
                    {
                        tag.militias = BasicLevelForm.sqlBiz.getMilitiasByGroup(tag.groupPath);

                        treeNode.ToolTipText += "组内已有民兵" + tag.militias.Count + "个";

                        foreach(Militia militia in tag.militias)
                        {
                            TreeNode mNode = treeNode.Nodes.Add(militia.info());
                            mNode.Name = militia.Id;//查找TreeNode的key
                        }
                    } catch(Exception e)
                    {
                        
                    }*/
                    
                }


                treeNode.Tag = tag;//记录节点


                addXmlNodeToTreeNode(node, treeNode.Nodes, fDic);
            }
        }

        public void modifyGroupName(XmlNode node, string newName)
        {//编辑组名
            node.Attributes["name"].Value = newName;
            saveXml();
        }

        public void modifyAllAttribute(XmlNode node, Dictionary<string, string> newAttributes)
        {//编辑组的所有属性
            foreach(KeyValuePair<string, string> attr in newAttributes)
            {
                XmlAttribute xmlAttr = xmlDoc.CreateAttribute(attr.Key);
                xmlAttr.Value = attr.Value;
                node.Attributes.SetNamedItem(xmlAttr);
            }
            saveXml();
            
        }
        
        public void deleteGroup(XmlNode node)
        {//删除组
            node.ParentNode.RemoveChild(node);//删除xml里面
            saveXml();
        }

        public GroupTag addGroupFromRoot(string groupName)
        {//从根节点增加组
            XmlElement xmlNode = xmlDoc.CreateElement("team");
           
            /**setId(xmlNode);*/
 
            xmlNode.SetAttribute("name", "新建组");
            rootNode.AppendChild(xmlNode);
            saveXml();
            return new GroupTag(xmlNode);
        }

        public GroupTag addGroupFromParent(XmlNode parentNode, string groupName)
        {//从父节点增加组
            XmlElement xmlNode = xmlDoc.CreateElement("team");

            /**setId(xmlNode);//设置Id*/

            xmlNode.SetAttribute("name", "新建组");
            parentNode.AppendChild(xmlNode);
            saveXml();

            return new GroupTag(xmlNode);
        }

        public string attributes(XmlNode node, string name)
        {//node中属性name的值
            XmlAttribute attr = node.Attributes[name];
            if(attr != null)
            {
                return attr.Value;
            } else
            {
                return "";
            }
        }

        private void mergeNode(XmlNode xmlNode, XmlNode xdNode, Dictionary<string, FacetValue> fDic)
        {//将xdNode的子节点合并到xmlNode的子节点中
            foreach(XmlNode xdChildNode in xdNode.ChildNodes)
            {
                bool isFinded = false;
                foreach(XmlNode xmlChildNode in xmlNode.ChildNodes)
                {
                    //MessageBox.Show(xdChildNode.Attributes["name"].Value + " " + xmlChildNode.Attributes["name"].Value);
                    if(xdChildNode.Attributes["name"].Value == xmlChildNode.Attributes["name"].Value)
                    {//这个节点有相同的
                        //那判断它是不是叶节点，如果是叶节点，还要加一下数量
                        if(!xdChildNode.HasChildNodes)
                        {//是叶节点，增加数量
                            TreeNode treeNode = groups_TreeView.Nodes.Find(getNodePath(xmlChildNode), true)[0];
                            GroupTag tag = (GroupTag)treeNode.Tag;
                            FacetValue fv;
                            if (fDic.TryGetValue(treeNode.Name, out fv))
                            {
                                tag.Count += fv.Hits;//本身加
                                addCountUpToAllParent(treeNode, fv.Hits);//所有父节点加
                            }
                        } else
                        {//有子节点
                            mergeNode(xmlChildNode, xdChildNode, fDic);
                        }
                        isFinded = true;
                    }
                }

                if(!isFinded)
                {//如果没有找到与xdChildNode相同的子节点，则将xdChildNode添加到xdNode的子节点中
                    XmlNode newNode = xmlNode.AppendChild(xmlDoc.ImportNode(xdChildNode, true));

                    TreeNode tn = groups_TreeView.Nodes.Find(getNodePath(xmlNode), true)[0];

                    TreeNode treeNode = tn.Nodes.Add(newNode.Attributes["name"].Value);

                    treeNode.ToolTipText = getToolTipText(newNode);

                    treeNode.Name = getNodePath(newNode);//查找TreeNode的Key

                    GroupTag tag = new GroupTag(newNode);

                    treeNode.Tag = tag;

                    if(newNode.HasChildNodes)
                    {
                        addXmlNodeToTreeNode(newNode, treeNode.Nodes, fDic);
                    } else
                    {
                        FacetValue fv;
                        if (fDic.TryGetValue(treeNode.Name, out fv))
                        {
                            tag.Count += fv.Hits;//本身加
                            addCountUpToAllParent(treeNode, fv.Hits);//所有父节点加
                        }

                        treeNode.Text = tag.info();
                    }
                    
                }
            }

        }

        public void addXml(string xmlFile, List<string> databases = null)
        {//将xmlFile合并到xmlDoc中并保存
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFile);//加载

            Dictionary<string, FacetValue> fdict = null;
            if(databases == null)
            {
                fdict = new Dictionary<string, FacetValue>();
            } else
            {
                fdict = sqlBiz.getGroupNums(databases);
            }

            XmlNode xdRoot = xd.DocumentElement;//根节点

            mergeNode(rootNode, xdRoot, fdict);

            saveXml();
        }

        public void exportXml(string fileName)
        {//将xml文件导出
            xmlDoc.Save(fileName);
        }
        

    }

}
