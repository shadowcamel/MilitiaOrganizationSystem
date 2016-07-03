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
    public partial class CityForm : Form
    {//市军分区主界面
        public static string dbName = LoginXmlConfig.Place;//数据库名

        public static SqlBiz sqlBiz = new SqlBiz(dbName);//静态的数据库

        private XMLGroupTaskForm xmlGroupTaskForm;//分组界面

        private Condition condition;//此界面下的lambda表达式

        private string place { get; set; }//该页面的查询条件之一指定数据库
        //此页面的查询条件

        private MilitiaListViewBiz listViewBiz;//民兵信息列表的业务逻辑层，用于对listView的增删改，存入数据库

        public CityForm()
        {//构造函数
            InitializeComponent();
            xmlGroupTaskForm = null;
            condition = new Condition("未分组");
            conditionLabel.Text = condition.ToString();
            listViewBiz = new MilitiaListViewBiz(militia_ListView, sqlBiz, condition);//需指定数据库
            updatePageUpDown();
            /*//从数据库中加载未分组民兵信息到显示
            listViewBiz.loadNotGroupedMilitiasInDb();*/

            militia_ListView.MouseDoubleClick += Militia_ListView_MouseDoubleClick;
            militia_ListView.ItemDrag += Militia_ListView_ItemDrag;

            militia_ListView.DragEnter += Militia_ListView_DragEnter;
            militia_ListView.DragOver += Militia_ListView_DragOver;
            militia_ListView.DragDrop += Militia_ListView_DragDrop;
        }

        private void Militia_ListView_DragOver(object sender, DragEventArgs e)
        {
            //MessageBox.Show((sender == militia_ListView) + "");
            MoveTag mt = (MoveTag)e.Data.GetData(typeof(MoveTag));
            if(mt.source == this)
            {//如果是从自己移过来的
                e.Effect = DragDropEffects.None;
            } else
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void Militia_ListView_DragDrop(object sender, DragEventArgs e)
        {//自动的，好像当e.effect==None时不会调用这个函数
            MoveTag mt = (MoveTag)e.Data.GetData(typeof(MoveTag));
            List<Militia> mList = mt.moveMilitias;
            militia_ListView.BeginUpdate();//开始更新界面
            foreach(Militia militia in mList)
            {
                if(militia.Id == null)
                {//是删除后移动过来的
                    if(MessageBox.Show("民兵：" + militia.info() + " 已经被删除，是否恢复它并继续操作？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        militia.Group = "未分组";
                        sqlBiz.addMilitia(militia);
                        if (condition.lambdaCondition.Compile()(militia))
                        {//如果满足条件，就添加到显示
                            listViewBiz.addOneMilitia(militia);
                        }
                    }
                    continue;
                }
                //在之前让原分组界面的个数减少1
                FormBizs.groupBiz.reduceCount(militia);
                ListViewItem lvi = listViewBiz.findItemWithMilitia(militia);
                militia.Group = "未分组";
                FormBizs.updateMilitiaItem(militia);//通知所有的民兵列表更新
                sqlBiz.updateMilitia(militia);//保存
                if (lvi != null)
                {
                    lvi.Tag = militia;
                    listViewBiz.updateItem(lvi);
                    if (!condition.lambdaCondition.Compile()(militia))
                    {//不满足筛选条件，则不能显示在这个界面
                        lvi.Remove();
                    }
                } else if(condition.lambdaCondition.Compile()(militia))
                {
                    listViewBiz.addOneMilitia(militia);
                }
            }
            militia_ListView.EndUpdate();//结束更新界面
        }

        private void Militia_ListView_DragEnter(object sender, DragEventArgs e)
        {
            this.Focus();
            e.Effect = DragDropEffects.Move;
        }

        private void Militia_ListView_ItemDrag(object sender, ItemDragEventArgs e)
        {//移动选中的items

            if(e.Button == MouseButtons.Left)
            {
                List<Militia> mList = new List<Militia>();
                foreach(ListViewItem lvi in militia_ListView.SelectedItems)
                {
                    Militia militia = (Militia)lvi.Tag;
                    mList.Add(militia);
                }
                MoveTag mt = new MoveTag(this, mList);
                if (DoDragDrop(mt, DragDropEffects.Move) == DragDropEffects.Move)
                {//移动成功后
                    foreach (ListViewItem lvi in militia_ListView.SelectedItems)
                    {
                        Militia militia = (Militia)lvi.Tag;
                        //if militia 不符合筛选条件，则删掉这个item
                        if(!condition.lambdaCondition.Compile()(militia))
                        {
                            lvi.Remove();
                        } else
                        {
                            listViewBiz.updateItem(lvi);//如果符合条件，则刷新显示
                        }
                    }
                }
            }
        }

        private void Militia_ListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {//双击编辑
            ListViewItem lvi = militia_ListView.GetItemAt(e.X, e.Y);
            int subIndex = lvi.SubItems.IndexOf(lvi.GetSubItemAt(e.X, e.Y));
            if(lvi != null)
            {
                listViewBiz.editOne(lvi, listViewBiz.displayedParameterIndexs[subIndex]);//弹出编辑窗口，并指定光标在subIndex处
            }
        }

        private void BasicLevelForm_Load(object sender, EventArgs e)
        {//加载时,同时打开分组界面
            xmlGroupTaskForm = new XMLGroupTaskForm();
            xmlGroupTaskForm.Show();
        }

        private void importXMLGroupTask_Click(object sender, EventArgs e)
        {//导入分组任务（添加任务）
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string file = fileDialog.FileName;//已经选择了文件
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
        {//测试所用，实际上是加载主数据库所有的民兵
            militia_ListView.Clear();
            listViewBiz.loadMilitiaList(sqlBiz.getAllMilitias());
        }

        private void export_Click(object sender, EventArgs e)
        {//导出
            FolderBrowserDialog fbdlg = new FolderBrowserDialog();
            fbdlg.Description = "请选择要导出的文件路径";
            if(fbdlg.ShowDialog() == DialogResult.OK)
            {
                string folder = fbdlg.SelectedPath;
                FormBizs.export(folder + "\\place.dump", "hello");
            }
        }

        private void import_Click(object sender, EventArgs e)
        {//导入
            MessageBox.Show("开始导入， time = " + DateTime.Now);
            OpenFileDialog ofdlg = new OpenFileDialog();
            ofdlg.Multiselect = true;//支持多选
            ofdlg.Filter = "民兵编组系统导出文件(*.dump)|*.dump";
            if(ofdlg.ShowDialog() == DialogResult.OK)
            {
                string[] files = ofdlg.FileNames;
                foreach(string file in files)
                {
                    FormBizs.importOne(file, "hello");
                }
            }
            MessageBox.Show("导入完毕， time = " + DateTime.Now);
            List<List<Militia>> mlList = sqlBiz.getConflictMilitiasBetweenDatabases();
            MessageBox.Show("拿到冲突, time = " + DateTime.Now);
            if(mlList.Count == 0)
            {
                MessageBox.Show("没有检测到冲突");
            } else
            {
                MessageBox.Show("检测到冲突");
                ConflictMilitiasForm cmf = new ConflictMilitiasForm(mlList);
                cmf.ShowDialog();
            }
        }

        private void updatePageUpDown()
        {//更新显示
            pageUpDown.Maximum = listViewBiz.maxPage;
            pageUpDown.Value = listViewBiz.page;
        }

        private void skipPage_Click(object sender, EventArgs e)
        {
            listViewBiz.toPage((int)pageUpDown.Value);
            updatePageUpDown();
        }

        private void lastPage_Click(object sender, EventArgs e)
        {
            listViewBiz.lastPage();
            updatePageUpDown();
            
        }

        private void currentPage_Click(object sender, EventArgs e)
        {
            listViewBiz.refreshCurrentPage();
            updatePageUpDown();
        }

        private void nextPage_Click(object sender, EventArgs e)
        {
            listViewBiz.nextPage();
            updatePageUpDown();
        }

        private void finalPage_Click(object sender, EventArgs e)
        {
            listViewBiz.finalPage();
            updatePageUpDown();
        }

        private void options_Click(object sender, EventArgs e)
        {//打开设置界面
            listViewBiz.setoption();
        }

        private void conditionLabel_Click(object sender, EventArgs e)
        {//打开筛选条件界面
            ConditionForm cf = new ConditionForm(condition);
            if(cf.ShowDialog() == DialogResult.OK)
            {
                conditionLabel.Text = condition.ToString();
                listViewBiz.refreshCurrentPage();
            }
        }

        private void doConflict_Click(object sender, EventArgs e)
        {//检测冲突，在数据库之间
            List<List<Militia>> mlList = sqlBiz.getConflictMilitiasBetweenDatabases();
            ConflictMilitiasForm cmf = new ConflictMilitiasForm(mlList);
            cmf.ShowDialog();
        }

        private void latestMilitias_Click(object sender, EventArgs e)
        {//最近编辑的民兵
            FormBizs.latestMilitiaForm.Show();
        }

        private void stastistics_Click(object sender, EventArgs e)
        {
            InfoStatisticsForm isf = new InfoStatisticsForm(condition);
            isf.Show();
        }
    }
}
