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
        public string groupPath { get; set; }
        public List<Militia> militias { get; }

        public GroupTag(XmlNode node)
        {
            this.tagXmlNode = node;
            groupPath = "";
            militias = new List<Militia>();
        }



    }
}
