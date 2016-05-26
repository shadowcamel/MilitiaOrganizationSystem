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
        
        public XMLGroupDao(string xmlGroupFile)
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
            /**maxId = 1;*/
        }

        public void loadToTreeView(TreeView groups_TreeView)
        {
            addXmlNodeToTreeNode(rootNode, groups_TreeView.Nodes);
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
            addXmlNodeToTreeNode(rootNode, groups_TreeView.Nodes);
            saveXml();
        }

        private void addXmlNodeToTreeNode(XmlNode root, TreeNodeCollection rootNodes)
        {//将root下的所有节点加载到treeView中
            foreach (XmlNode node in root.ChildNodes)
            {
                /**if (node.Attributes["Id"] == null)
                {
                    setId(node);//给节点增加Id属性
                }*/

                TreeNode treeNode = rootNodes.Add(node.Attributes["name"].Value);

                treeNode.ToolTipText = getToolTipText(node);

                GroupTag tag = new GroupTag(node);

                tag.groupPath = getNodePath(node);//path

                //写获取民兵信息

                treeNode.Tag = tag;//记录节点

                
                
                addXmlNodeToTreeNode(node, treeNode.Nodes);
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

        private void mergeNode(XmlNode xmlNode, XmlNode xdNode)
        {//将xdNode的子节点合并到xmlNode的子节点中
            foreach(XmlNode xdChildNode in xdNode.ChildNodes)
            {
                bool isFinded = false;
                foreach(XmlNode xmlChildNode in xmlNode.ChildNodes)
                {
                    //MessageBox.Show(xdChildNode.Attributes["name"].Value + " " + xmlChildNode.Attributes["name"].Value);
                    if(xdChildNode.Attributes["name"].Value == xmlChildNode.Attributes["name"].Value)
                    {
                        mergeNode(xmlChildNode, xdChildNode);
                        isFinded = true;
                    }
                }

                if(!isFinded)
                {//如果没有找到与xdChildNode相同的子节点，则将xdChildNode添加到xdNode的子节点中
                    xmlNode.AppendChild(xmlDoc.ImportNode(xdChildNode, true));
                }
            }

        }

        public void addXml(string xmlFile)
        {//将xmlFile合并到xmlDoc中并保存
            XmlDocument xd = new XmlDocument();
            xd.Load(xmlFile);//加载

            XmlNode xdRoot = xd.DocumentElement;//根节点
            
            mergeNode(rootNode, xdRoot);

            saveXml();
        }
        

    }

}
