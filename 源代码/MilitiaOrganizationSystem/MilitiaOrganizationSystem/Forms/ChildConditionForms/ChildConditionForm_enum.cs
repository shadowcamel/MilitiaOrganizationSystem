using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MilitiaOrganizationSystem
{
    public partial class ChildConditionForm_enum : Form
    {
        private Condition.ChildCondition childCondtion;
        private XmlNode paraNode;

        public ChildConditionForm_enum(Condition.ChildCondition cc)
        {
            InitializeComponent();

            childCondtion = cc;
            paraNode = cc.parameterNode;

            propertyNameLabel.Text = paraNode.Attributes["name"].Value;

            for (int i = 0; i < paraNode.ChildNodes.Count; i++)
            {
                XmlNode selectNode = paraNode.ChildNodes[i];
                propertyvaluesListbox.Items.Add(selectNode.Attributes["name"].Value);
            }

            foreach(string value in cc.Values)
            {//设置选中
                XmlNode selectChildNode = paraNode.SelectSingleNode("selection[@value='" + value + "']");

                for (int i = 0; i < paraNode.ChildNodes.Count; i++)
                {
                    if (selectChildNode == paraNode.ChildNodes[i])
                    {
                        propertyvaluesListbox.SetItemChecked(i, true);
                        break;
                    }
                }
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {//ok,条件赋值
            childCondtion.Values.Clear();
            for(int i = 0; i < propertyvaluesListbox.Items.Count; i++)
            {
                if(propertyvaluesListbox.GetItemChecked(i))
                {
                    childCondtion.Values.Add(paraNode.ChildNodes[i].Attributes["value"].Value);
                }
            }
            childCondtion.Method = "Equal";
        }
    }
}
