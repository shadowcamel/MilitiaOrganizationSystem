using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MilitiaOrganizationSystem
{
    class MilitiaXmlConfig
    {//配置民兵信息类，从xml民兵信息配置文件中读取并处理
        private const string xmlMilitiaConfigFile = "Parameters.xml";
        private static XmlDocument xmlDoc = null;
        private static XmlNode rootNode;
        

        public static void initial()
        {
            if(xmlDoc == null)
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlMilitiaConfigFile);
                rootNode = xmlDoc.DocumentElement;
            }
        }

        public static XmlNodeList parameters()
        {
            return rootNode.SelectNodes("parameter");
        }

        public static string getTypeOf(string propertyName)
        {
            XmlNode node = rootNode.SelectSingleNode("parameter[@property='" + propertyName + "']");
            return node.Attributes["type"].Value;
        }

        

        public static void generateMilitiaToXml(int n, string xmlFile)
        {//生成n个民兵信息到xmlFile
            Random rand = new Random();
            XmlNodeList xList = parameters();

            XmlDocument militiaDoc = new XmlDocument();
            XmlDeclaration dec = militiaDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            militiaDoc.AppendChild(dec);
            XmlElement militiaRoot = militiaDoc.CreateElement("root");
            militiaDoc.AppendChild(militiaRoot);

            for (int i = 0; i < n; i++)
            {
                XmlElement militia = militiaDoc.CreateElement("militia");
                foreach (XmlNode node in xList)
                {
                    XmlAttribute attr = militiaDoc.CreateAttribute(node.Attributes["property"].Value);
                    if (node.Attributes["type"].Value == "string")
                    {
                        byte[] buffer = new byte[8];
                        rand.NextBytes(buffer);
                        attr.Value = System.Text.Encoding.Unicode.GetString(buffer);
                    }
                    else if (node.Attributes["type"].Value == "enum")
                    {
                        attr.Value = node.ChildNodes[rand.Next(node.ChildNodes.Count)].Attributes["value"].Value;
                    }
                    militia.Attributes.Append(attr);

                }
                XmlAttribute attr2 = militiaDoc.CreateAttribute("groupId");
                string[] a = { "1", "2", "3" };//分组id
                attr2.Value = a[rand.Next(a.Length)];
                militia.Attributes.Append(attr2);

                militiaRoot.AppendChild(militia);
            }

            militiaDoc.Save(xmlFile);
        }

        public static void MilitiaListToXml(List<Militia> mList, string xmlFile)
        {
            XmlDocument militiaDoc = new XmlDocument();
            XmlDeclaration dec = militiaDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            militiaDoc.AppendChild(dec);
            XmlElement militiaRoot = militiaDoc.CreateElement("root");
            militiaDoc.AppendChild(militiaRoot);
            //以上是新建xml文档对象

            foreach(Militia militia in mList)
            {
                XmlElement militiaNode = militiaDoc.CreateElement("militia");
                foreach(KeyValuePair<string, string> keyValue in militia.InfoDic)
                {
                    XmlAttribute attr = militiaDoc.CreateAttribute(keyValue.Key);
                    attr.Value = keyValue.Value.ToString();
                    militiaNode.Attributes.Append(attr);
                }
                XmlAttribute attr2 = militiaDoc.CreateAttribute("group");
                attr2.Value = militia.group;
                militiaNode.Attributes.Append(attr2);
                militiaRoot.AppendChild(militiaNode);
            }

            

            militiaDoc.Save(xmlFile);
        }

        public static List<Militia> generateMilitias(int n)
        {//生成n个民兵对象为list
            Random rand = new Random();
            XmlNodeList xList = parameters();
            List<Militia> mList = new List<Militia>();
            for (int i = 0; i < n; i++)
            {
                Militia militia = new Militia();
                foreach (XmlNode node in xList)
                {       
                    if (node.Attributes["type"].Value == "string")
                    {
                        byte[] buffer = new byte[8];
                        rand.NextBytes(buffer);
                        militia.InfoDic[node.Attributes["property"].Value] = System.Text.Encoding.Unicode.GetString(buffer);
                    }
                    else if (node.Attributes["type"].Value == "enum")
                    {
                        militia.InfoDic[node.Attributes["property"].Value] = node.ChildNodes[rand.Next(node.ChildNodes.Count)].Attributes["value"].Value;
                    }
                }
                
                string[] a = { "1", "2", "3" };//分组id
                militia.group = a[rand.Next(a.Length)];

                mList.Add(militia);
            }
            return mList;
        }

        
    }
}
