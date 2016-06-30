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
    public partial class GroupMilitiaForm : Form
    {
        private SqlBiz sqlBiz;
        private System.Linq.Expressions.Expression<Func<Militia, bool>> lambdaCondition;//筛选条件
        private MilitiaListViewBiz listViewBiz;//民兵信息列表业务逻辑层

        public GroupMilitiaForm(SqlBiz sBiz, string group)
        {
            InitializeComponent();
            sqlBiz = sBiz;
            lambdaCondition = x => x.Group.StartsWith(group);
            listViewBiz = new MilitiaListViewBiz(militia_ListView, sqlBiz, lambdaCondition);//需指定数据库

            /*//从数据库中加载相应民兵信息到显示
            int sum;
            listViewBiz.loadMilitiaList(sqlBiz.getMilitiasByGroup(group, 0, 1000, out sum));*/


            militia_ListView.ItemDrag += Militia_ListView_ItemDrag;
        }

        private void Militia_ListView_ItemDrag(object sender, ItemDragEventArgs e)
        {//移动选中的items
            if (e.Button == MouseButtons.Left)
            {
                List<Militia> mList = new List<Militia>();
                foreach (ListViewItem lvi in militia_ListView.SelectedItems)
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
                        if (!lambdaCondition.Compile()(militia))
                        {
                            lvi.Remove();
                        }
                    }
                }
            }
        }

        private void updatePageUpDown()
        {
            pageUpDown.Value = listViewBiz.page;
            pageUpDown.Maximum = listViewBiz.maxPage;
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
    }
}
