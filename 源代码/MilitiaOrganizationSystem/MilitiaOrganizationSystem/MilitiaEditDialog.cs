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
    public partial class MilitiaEditDialog : Form
    {
        private List<ComboBox> cList;//comboBox的list
        private XmlNodeList parameters;
        private Militia militia;

        public MilitiaEditDialog()
        {

            militia = null;
            cList = new List<ComboBox>();

            InitializeComponent();

            parameters = MilitiaXmlConfig.parameters();//配置文件NodeList

            int y = 10;//控件纵坐标
            

            for(int k = 0; k < parameters.Count; k++)
            {
                XmlNode xmlNode = parameters[k];

                Label label = new Label();//标签
                label.Text = xmlNode.Attributes["name"].Value;
                label.Dock = DockStyle.Fill;
                label.Anchor = AnchorStyles.Top & AnchorStyles.Bottom;
                label.AutoSize = true;
                ComboBox comboBox = new ComboBox();
                comboBox.Dock = DockStyle.Fill;
                comboBox.Tag = xmlNode;

                if (xmlNode.Attributes["type"].Value == "string")
                {
                    comboBox.Text = "";
                }
                else
                {

                    comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

                    for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
                    {
                        XmlNode selectNode = xmlNode.ChildNodes[i];
                        comboBox.Items.Add(selectNode.Attributes["name"].Value);
                    }

                    comboBox.SelectedIndex = 0;
                }

                
                tlp.Controls.Add(label);
                tlp.Controls.Add(comboBox);

                cList.Add(comboBox);

                y += 50;//下一个属性
            }

            for(int i = 0; i < tlp.RowStyles.Count; i++)
            {
                tlp.RowStyles[i].Height = 30;
            }

            
        }

        public DialogResult showEditDlg(Militia oneMilitia, int focusIndex = 0)
        {
            militia = oneMilitia;
            

            for(int k = 0; k < cList.Count; k++)
            {
                ComboBox comboBox = cList[k];
                XmlNode xmlNode = parameters[k];

                string strValue = "";
                try
                {
                    strValue = (string)militia.InfoDic[xmlNode.Attributes["property"].Value];

                }
                catch (Exception e)
                {

                }

                if (xmlNode.Attributes["type"].Value == "string")
                {
                    comboBox.Text = strValue;
                } else if(xmlNode.Attributes["type"].Value == "enum")
                {
                    comboBox.SelectedIndex = 0;

                    XmlNode selectChildNode = xmlNode.SelectSingleNode("selection[@value='" + strValue + "']");

                    for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
                    {
                        if(selectChildNode == xmlNode.ChildNodes[i])
                        {
                            comboBox.SelectedIndex = i;
                            break;
                        }
                    }
                }


            }

            //cList[focusIndex].;//设置光标首先出现的位置,默认是0
            if(focusIndex < cList.Count)
            {
                cList[focusIndex].Select();
            }
            

            return ShowDialog();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {//OK了，给传进的militia赋值
            if(militia == null)
            {
                MessageBox.Show("MilitiaEditDialog类使用错误！");
                return;
            }

            foreach(ComboBox cbb in cList)
            {
                XmlNode xmlNode = (XmlNode)cbb.Tag;
                string value = "";
                if(xmlNode.Attributes["type"].Value == "string")
                {
                    value = cbb.Text;
                } else if(xmlNode.Attributes["type"].Value == "enum")
                {
                    value = xmlNode.ChildNodes[cbb.SelectedIndex].Attributes["value"].Value;
                }
                militia.InfoDic[xmlNode.Attributes["property"].Value] = value;
            }
        }
    }
}
