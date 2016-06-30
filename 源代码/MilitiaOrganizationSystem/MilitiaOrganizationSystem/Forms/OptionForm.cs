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
    public partial class OptionForm : Form
    {
        private MilitiaListViewBiz listviewBiz = null;
        private List<int> checkedIndexs = new List<int>();//选中的下标（有顺序）

        private System.Xml.XmlNodeList parameters = MilitiaXmlConfig.parameters;

        public OptionForm()
        {
            InitializeComponent();
            
            foreach(System.Xml.XmlNode xn in parameters)
            {
                parasCheckBox.Items.Add(xn.Attributes["name"].Value);
            }

            checkAll.CheckedChanged += CheckAll_CheckedChanged;
            parasCheckBox.ItemCheck += ParasCheckBox_ItemCheck;
        }

        private void ParasCheckBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {//选择变化
            if(e.CurrentValue == CheckState.Checked)
            {//取消
                checkAll.Checked = false;
                checkedParaListBox.Items.Remove(parasCheckBox.Items[e.Index]);
                checkedIndexs.Remove(e.Index);
            } else
            {//选中
                checkedParaListBox.Items.Add(parasCheckBox.Items[e.Index]);
                checkedIndexs.Add(e.Index);
            }
        }

        private void CheckAll_CheckedChanged(object sender, EventArgs e)
        {//全选
            if(checkAll.Checked == true)
            {
                for (int i = 0; i < parasCheckBox.Items.Count; i++)
                {
                    if(!parasCheckBox.GetItemChecked(i))
                    {
                        parasCheckBox.SetItemChecked(i, true);
                    }
                }
            }
        }

        public DialogResult showOptionDialog(MilitiaListViewBiz mlvb)
        {//显示设置，并赋值
            listviewBiz = mlvb;
            pageSize.Value = mlvb.pageSize;

            for(int i = 0; i < parasCheckBox.Items.Count; i++)
            {
                parasCheckBox.SetItemChecked(i, false);
            }
            checkedParaListBox.Items.Clear();
            checkedIndexs.Clear();

            foreach(int index in mlvb.displayedParameterIndexs)
            {
                parasCheckBox.SetItemChecked(index, true);
            }

            return ShowDialog();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {//ok，保存设置
            listviewBiz.pageSize = (int)pageSize.Value;//最大显示数量
            listviewBiz.displayedParameterIndexs.Clear();
            listviewBiz.displayedParameterIndexs.AddRange(checkedIndexs);//不能直接赋值，因为这个类是共享的
        }
    }
}
