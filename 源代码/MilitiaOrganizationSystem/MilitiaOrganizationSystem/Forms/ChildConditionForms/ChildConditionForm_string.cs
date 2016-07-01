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
    public partial class ChildConditionForm_string : Form
    {
        private Condition.ChildCondition childCondition;

        private System.Xml.XmlNode paraNode;

        public ChildConditionForm_string(Condition.ChildCondition cc)
        {
            InitializeComponent();

            childCondition = cc;

            paraNode = cc.parameterNode;

            propertyName.Text = paraNode.Attributes["name"].Value;

            if(cc.Method == "StartsWith")
            {
                radio_StartsWith.Checked = true;
                startwithCombobox.Text = cc.Values[0];
            } else if(cc.Method == "EndsWith")
            {
                radio_EndsWith.Checked = true;
                endswithCombobox.Text = cc.Values[0];
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if(radio_StartsWith.Checked)
            {
                childCondition.Method = "StartsWith";
                childCondition.Values.Add(startwithCombobox.Text);
            } else if(radio_EndsWith.Checked)
            {
                childCondition.Method = "EndsWith";
                childCondition.Values.Add(endswithCombobox.Text);
            } else
            {

            }
        }
    }
}
