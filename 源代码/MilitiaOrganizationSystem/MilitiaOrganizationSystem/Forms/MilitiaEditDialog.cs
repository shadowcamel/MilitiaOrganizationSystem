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
        private List<int> parameterIndexs;//需编辑的参数下标
        private XmlNodeList parameters;
        private Militia militia;

        public MilitiaEditDialog()
        {
            militia = null;
            cList = new List<ComboBox>();

            InitializeComponent();

            parameterIndexs = MilitiaXmlConfig.getEditParameterIndexs();//配置文件需编辑的参数

            parameters = MilitiaXmlConfig.parameters;
            
            int y = 10;//控件纵坐标

            foreach(int index in parameterIndexs)
            {
                XmlNode xmlNode = parameters[index];

                Label label = new Label();//标签
                label.Text = xmlNode.Attributes["name"].Value;
                label.Dock = DockStyle.Fill;
                label.Anchor = AnchorStyles.Top & AnchorStyles.Bottom;
                label.AutoSize = true;
                ComboBox comboBox = new ComboBox();
                comboBox.Dock = DockStyle.Fill;
                comboBox.Tag = xmlNode;
                string type = xmlNode.Attributes["type"].Value;
                switch(type)
                {
                    case "enum":
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;

                        for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
                        {
                            XmlNode selectNode = xmlNode.ChildNodes[i];
                            comboBox.Items.Add(selectNode.Attributes["name"].Value);
                        }

                        comboBox.SelectedIndex = 0;
                        break;
                    case "place":
                        comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                        comboBox.Items.Add("点击编辑");
                        comboBox.SelectedIndex = 0;
                        comboBox.MouseClick += ComboBox_MouseClick;
                        break;
                    default:
                        comboBox.Text = "";
                        break;
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

        private void ComboBox_MouseClick(object sender, MouseEventArgs e)
        {//编辑地区属性
            ComboBox cb = (ComboBox)sender;
            PlaceChooseForm pcf = new PlaceChooseForm(((XmlNode)cb.Tag).Attributes["name"].Value, cb.Text);
            if(pcf.ShowDialog() == DialogResult.OK)
            {
                cb.Items[0] = PlaceXmlConfig.getPlaceName(pcf.PCD_ID);
            }
        }

        public DialogResult showEditDlg(Militia oneMilitia, int focusIndex = 0)
        {//show编辑框
            militia = oneMilitia;
            MilitiaReflection mr = new MilitiaReflection(militia);//反射

            for(int k = 0; k < cList.Count; k++)
            {
                ComboBox comboBox = cList[k];
                XmlNode xmlNode = parameters[parameterIndexs[k]];//第k个要编辑的属性

                string strValue = "";
                try
                {
                    strValue = mr.getProperty(xmlNode.Attributes["property"].Value).ToString();

                }
                catch (Exception e)
                {

                }

                switch(xmlNode.Attributes["type"].Value)
                {
                    case "enum":
                        comboBox.SelectedIndex = 0;

                        XmlNode selectChildNode = xmlNode.SelectSingleNode("selection[@value='" + strValue + "']");

                        for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
                        {
                            if (selectChildNode == xmlNode.ChildNodes[i])
                            {
                                comboBox.SelectedIndex = i;
                                break;
                            }
                        }
                        break;
                    case "place":
                        comboBox.Items[0] = PlaceXmlConfig.getPlaceName(strValue);
                        break;
                    default:
                        comboBox.Text = strValue;
                        break;
                }


            }

            //cList[focusIndex].;//设置光标首先出现的位置,默认是0
            if(focusIndex >= 0 && focusIndex < cList.Count)
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
            MilitiaReflection mr = new MilitiaReflection(militia);//反射

            foreach(ComboBox cbb in cList)
            {
                XmlNode xmlNode = (XmlNode)cbb.Tag;
                string value = "";
                switch(xmlNode.Attributes["type"].Value)
                {
                    case "enum":
                        value = xmlNode.ChildNodes[cbb.SelectedIndex].Attributes["value"].Value;
                        break;
                    case "place":
                        value = PlaceXmlConfig.getPCD_ID(cbb.Items[0].ToString());
                        break;
                    default:
                        value = cbb.Text;
                        break;
                }
                mr.setProperty(xmlNode.Attributes["property"].Value, value);
            }
        }
    }
}
