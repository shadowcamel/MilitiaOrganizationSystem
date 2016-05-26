using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilitiaOrganizationSystem
{
    public partial class BasicLevelForm : Form
    {
        public const string xmlGroupFile = "xmlGroupFile.xml";//分组的配置文件
        public const string dbName = "basicLevelDB";//数据库名

        public static SqlBiz sqlBiz = new SqlBiz(dbName);//静态的数据库

        private XMLGroupTaskForm xmlGroupTaskForm;//分组界面

        private MilitiaListViewBiz listViewBiz;//民兵信息列表的业务逻辑层，用于对listView的增删改，存入数据库

        public BasicLevelForm()
        {
            InitializeComponent();
            xmlGroupTaskForm = null;
            listViewBiz = new MilitiaListViewBiz(militia_ListView, sqlBiz);//需指定数据库
            //从数据库中加载民兵信息到显示
            listViewBiz.loadAllMilitiaInDB();

            militia_ListView.MouseDoubleClick += Militia_ListView_MouseDoubleClick;
            militia_ListView.ItemDrag += Militia_ListView_ItemDrag;
            

        }

        private void Militia_ListView_ItemDrag(object sender, ItemDragEventArgs e)
        {

            if(e.Button == MouseButtons.Left)
            {
                DoDragDrop(militia_ListView, DragDropEffects.Move);
            }
        }

        private void Militia_ListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem lvi = militia_ListView.GetItemAt(e.X, e.Y);
            int subIndex = lvi.SubItems.IndexOf(lvi.GetSubItemAt(e.X, e.Y));
            if(lvi != null)
            {
                listViewBiz.editOne(lvi, subIndex);//弹出编辑窗口，并指定光标在subIndex处
            }
        }

        private void BasicLevelForm_Load(object sender, EventArgs e)
        {
            /**if (File.Exists(xmlGroupFile))
            {//判断分组任务是否已经存在，如果存在，即加载分组任务
                xmlGroupTaskForm = new XMLGroupTaskForm(xmlGroupFile);
                xmlGroupTaskForm.Show();
            }*/
            xmlGroupTaskForm = new XMLGroupTaskForm(xmlGroupFile);
            xmlGroupTaskForm.Show();
        }

        private void importXMLGroupTask_Click(object sender, EventArgs e)
        {
            /**if(xmlGroupTaskForm != null)
            {
                DialogResult re = MessageBox.Show("分组任务已存在，是否删除之前的分组数据，重新导入编组任务？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if(re == DialogResult.Cancel)
                {
                    return;
                }
            }*/

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;//已经选择了文件
                //MessageBox.Show("已选择文件:" + file, "选择文件提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                /**XMLGroupTaskForm lastForm = xmlGroupTaskForm;
                try
                {
                    xmlGroupTaskForm = new XMLGroupTaskForm(file);
                } catch(Exception xmlExeption)
                {
                    MessageBox.Show("导入xml文件出现异常！", "异常警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (lastForm != null)
                {//既然加载成功，那就关闭以前的，且在数据库里删除以前的分组信息
                    lastForm.Close();
                    //数据库操作
                }
                xmlGroupTaskForm.Show();*/
                try
                {
                    xmlGroupTaskForm.addXmlGroupTask(file);
                } catch(Exception xmlExeption)
                {
                    MessageBox.Show("导入xml文件出现异常！", "异常警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }

        }

        private void modify_Click(object sender, EventArgs e)
        {
            listViewBiz.editSelectedItems();
        }

        private void add_Click(object sender, EventArgs e)
        {
            listViewBiz.addOne();
        }

        private void dele_Click(object sender, EventArgs e)
        {
            listViewBiz.deleSelectedItems();
        }

        private void rAdd_Click(object sender, EventArgs e)
        {
            listViewBiz.addOne();
        }

        private void rEdit_Click(object sender, EventArgs e)
        {
            listViewBiz.editSelectedItems();
        }

        private void rDele_Click(object sender, EventArgs e)
        {
            listViewBiz.deleSelectedItems();
        }

        private void importFromXml_Click(object sender, EventArgs e)
        {
            listViewBiz.loadAllMilitiaInDB();
        }
    }
}
