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
    public partial class ConditionForm : Form
    {
        private System.Xml.XmlNodeList parameters = MilitiaXmlConfig.parameters;

        public Condition condition { get; set; }//条件

        public ConditionForm(Condition currentCondition)
        {
            InitializeComponent();

            condition = new Condition(currentCondition);

            foreach (System.Xml.XmlNode xn in parameters)
            {//添加item
                parasCheckBox.Items.Add(xn.Attributes["name"].Value);
            }
            
            //显示到listbox
            foreach(Condition.ChildCondition cc in condition.conditionDict.Values)
            {
                conditionListBox.Items.Add(cc);
            }

            parasCheckBox.MouseClick += ParasCheckBox_MouseClick;
            parasCheckBox.MouseDoubleClick += ParasCheckBox_MouseDoubleClick;

            conditionListBox.MouseDoubleClick += ConditionListBox_MouseDoubleClick;
        }

        private void showChildConditionDialog(Condition.ChildCondition cc)
        {
            
        }

        private void ConditionListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {//弹出条件编辑
            Condition.ChildCondition cc = (Condition.ChildCondition)conditionListBox.SelectedItem;


        }

        private void ParasCheckBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {//双击选中，并打开一个ChildCondition对话框
            string propertyName = (string)parasCheckBox.SelectedItem;
            Condition.ChildCondition cc = new Condition.ChildCondition(MilitiaXmlConfig.getNodeByProperty(propertyName));
            


        }

        private void ParasCheckBox_MouseClick(object sender, MouseEventArgs e)
        {//阻止默认的单击选中
            if(parasCheckBox.GetItemChecked(parasCheckBox.SelectedIndex))
            {
                parasCheckBox.SetItemChecked(parasCheckBox.SelectedIndex, false);
            } else
            {
                parasCheckBox.SetItemChecked(parasCheckBox.SelectedIndex, true);
            }
            
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {//ok了，生成条件

        }

        private void deleteCondition_Click(object sender, EventArgs e)
        {//删除listBox里的条件
            conditionListBox.Items.Remove(conditionListBox.SelectedItem);
        }
    }
}
