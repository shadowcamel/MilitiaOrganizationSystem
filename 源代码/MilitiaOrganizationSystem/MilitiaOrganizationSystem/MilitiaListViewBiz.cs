using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MilitiaOrganizationSystem
{
    class MilitiaListViewBiz
    {
        private static MilitiaEditDialog militiaEditDlg = new MilitiaEditDialog();
        private ListView militia_ListView;
        private XmlNodeList parameters;//参数
        private bool sort = false;
        private SqlBiz sqlBiz;

        public MilitiaListViewBiz(ListView listView, SqlBiz sBz)
        {
            militia_ListView = listView;
            parameters = MilitiaXmlConfig.parameters();
            sqlBiz = sBz;

            bindEvent();
        }

        private void bindEvent()
        {
            militia_ListView.ColumnClick += Militia_ListView_ColumnClick;//点击排序
        }

        class ListViewColumnSorter : IComparer
        {
            
            /// <summary>
            /// 指定按照哪个列排序
            /// </summary>
            private int ColumnToSort;
            
            /// <summary>
            /// 指定排序的方式
            /// </summary>
            private SortOrder OrderOfSort;
            
            /// <summary>
            /// 声明CaseInsensitiveComparer类对象，
            /// 参见ms-help://MS.VSCC.2003/MS.MSDNQTR.2003FEB.2052/cpref/html/frlrfSystemCollectionsCaseInsensitiveComparerClassTopic.htm
            /// </summary>
            private CaseInsensitiveComparer ObjectCompare;

            
            /// <summary>
            /// 构造函数
            /// </summary>
            public ListViewColumnSorter()
            {
                // 默认按第一列排序
                ColumnToSort = 0;

                // 排序方式为不排序
                OrderOfSort = SortOrder.None;

                // 初始化CaseInsensitiveComparer类对象
                ObjectCompare = new CaseInsensitiveComparer();
            }

            
            /// <summary>
            /// 重写IComparer接口.
            /// </summary>
            /// <param name="x">要比较的第一个对象</param>
            /// <param name="y">要比较的第二个对象</param>
            /// <returns>比较的结果.如果相等返回0，如果x大于y返回1，如果x小于y返回-1</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                ListViewItem listviewX, listviewY;

                // 将比较对象转换为ListViewItem对象
                listviewX = (ListViewItem)x;
                listviewY = (ListViewItem)y;

                // 比较
                compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

                // 根据上面的比较结果返回正确的比较结果
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // 因为是正序排序，所以直接返回结果
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // 如果是反序排序，所以要取负值再返回
                    return (-compareResult);
                }
                else
                {
                    // 如果相等返回0
                    return 0;
                }
            }

            
            /// <summary>
            /// 获取或设置按照哪一列排序.
            /// </summary>
            public int SortColumn
            {
                set
                {
                    ColumnToSort = value;
                }
                get
                {
                    return ColumnToSort;
                }
            }

            
            /// <summary>
            /// 获取或设置排序方式.
            /// </summary>
            public SortOrder Order
            {

                set
                {
                    OrderOfSort = value;
                }
                get
                {
                    return OrderOfSort;
                }
            }
        }

        private void Militia_ListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // 创建一个ListView排序类的对象，并设置militia_ListView的排序器
            ListViewColumnSorter lvwColumnSorter = new ListViewColumnSorter();


            if (sort == false)
            {
                sort = true;
                lvwColumnSorter.Order = SortOrder.Descending;
            }
            else
            {
                sort = false;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            lvwColumnSorter.SortColumn = e.Column;
            militia_ListView.ListViewItemSorter = lvwColumnSorter;

            // 用新的排序方法对ListView排序
            //this.listView1.Sort();
        }

        private void addColumnHeader()
        {
            
            foreach(XmlNode parameter in parameters)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = parameter.Attributes["name"].Value;   //设置列标题 
                ch.Width = 120;    //设置列宽度 
                ch.TextAlign = HorizontalAlignment.Left;   //设置列的对齐方式 
                militia_ListView.Columns.Add(ch);    //将列头添加到ListView控件。 
            }

            militia_ListView.Columns.Add("分组");
            
        }

        public void updateItem(ListViewItem lvi)
        {//用tag更新显示
            Militia militia = (Militia)lvi.Tag;
            lvi.ImageIndex = 0;
            XmlNode firstNode = parameters[0];
            string value = "";
            try
            {
                value = militia.InfoDic[parameters[0].Attributes["property"].Value].ToString();
            } catch(Exception e)
            {
                
            }
            
            XmlNode selectNode = firstNode.SelectSingleNode("selection[@value='" + value + "']");
            if (selectNode != null)
            {
                value = selectNode.Attributes["name"].Value;
            }
            lvi.Text = value;
            if(lvi.SubItems.Count != parameters.Count)
            {//此lvi是新建的,则将lvi的subItems添上
                string[] items = new string[parameters.Count];
                for(int i = 0; i < items.Length; i++)
                {
                    items[i] = "";
                }
                lvi.SubItems.AddRange(items);
            }
            for (int i = 1; i < parameters.Count; i++)
            {
                XmlNode node = parameters[i];
                value = "";
                try
                {
                    value = militia.InfoDic[parameters[i].Attributes["property"].Value].ToString();
                }
                catch (Exception e)
                {

                }
                selectNode = node.SelectSingleNode("selection[@value='" + value + "']");
                if (selectNode != null)
                {
                    value = selectNode.Attributes["name"].Value;
                }

                lvi.SubItems[i].Text = value;
            }

            lvi.SubItems[parameters.Count].Text = militia.Group;

            
        }

        public void loadMilitiaList(List<Militia> mList)
        {
            addColumnHeader();

            militia_ListView.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度 

            foreach (Militia militia in mList)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Tag = militia;

                updateItem(lvi);

                militia_ListView.Items.Add(lvi);
            }

            militia_ListView.EndUpdate();  //结束数据处理，UI界面一次性绘制。 

        }

        public void loadAllMilitiaInDB()
        {//加载数据库中所有民兵到ListView
            militia_ListView.Clear();
            loadMilitiaList(sqlBiz.getAllMilitias());
        }

        public void loadNotGroupedMilitiasInDb()
        {//加载未分组民兵到ListView
            militia_ListView.Clear();
            loadMilitiaList(sqlBiz.getMilitiasByGroup("未分组"));
        }

        public void addOneMilitia(Militia militia)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = militia;
            updateItem(lvi);
            militia_ListView.Items.Add(lvi);
            militia_ListView.SelectedItems.Clear();
            lvi.Selected = true;
        }

        public void addOne()
        {//添加一个民兵
            Militia militia = new Militia();
            
            if(militiaEditDlg.showEditDlg(militia) == DialogResult.OK)
            {
                addOneMilitia(militia);

                sqlBiz.addMilitia(militia);
            }
           
        }

        public void editOne(ListViewItem lvi, int focusIndex = 0)
        {//编辑一个民兵,focusIndex是弹出编辑对话框需要focus到哪个编辑框
            Militia militia = (Militia)lvi.Tag;

            if(militiaEditDlg.showEditDlg(militia, focusIndex) == DialogResult.OK)
            {
                updateItem(lvi);
                sqlBiz.updateMilitia(militia);
            }

            //通知GroupForm刷新民兵
            ((XMLGroupTaskForm)Program.formDic["GroupForm"]).updateMilitiaNode(militia);
        }

        public void editSelectedItems()
        {//编辑所有选中的item
            foreach(ListViewItem lvi in militia_ListView.SelectedItems)
            {
                editOne(lvi);
            }
        }

        public void deleSelectedItems()
        {//删除所有选中的item
            foreach(ListViewItem lvi in militia_ListView.SelectedItems)
            {
                Militia militia = (Militia)lvi.Tag;
                militia_ListView.Items.Remove(lvi);
                sqlBiz.deleteMilitia(militia);


                //通知GroupForm删除相应民兵节点
                ((XMLGroupTaskForm)Program.formDic["GroupForm"]).removeMilitaNode(militia);
            }
        }

        
    }
}
