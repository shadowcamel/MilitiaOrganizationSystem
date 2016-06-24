using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MilitiaOrganizationSystem
{
    class GroupTag
    {//标记，将xmlNode与treeNode相连

        public XmlNode tagXmlNode { get; set; }
        //public string groupPath { get; set; }
        //public List<Militia> militias { get; set; }
        public int Count { get; set; }//代表此组下的民兵数量

        public GroupTag(XmlNode node)
        {
            this.tagXmlNode = node;
            Count = 0;
            //groupPath = "";
            //militias = null;
        }

        public string info()
        {
            return tagXmlNode.Attributes["name"].Value + "(" + "已有民兵" + Count + "人)";
        }


    }
}
