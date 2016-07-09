using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace MilitiaOrganizationSystem
{
    static class FormBizs
    {//全局管理Form的类
        public const string exportGroupFileName = "groupTask.xml";
        public const string exportMilitiaFileName = "militiaList";
        



        public static SqlBiz sqlBiz = null;//一个程序有且仅有一个sqlBiz
        public static XMLGroupTreeViewBiz groupBiz = null;//有且仅有一个groupBiz
        public static List<MilitiaListViewBiz> mListBizs = new List<MilitiaListViewBiz>();

        public static LatestMilitiaForm latestMilitiaForm = new LatestMilitiaForm();


        public static void updateMilitiaItem(Militia militia)
        {//更新所有民兵ListView上的Item
            foreach(MilitiaListViewBiz mlvb in mListBizs)
            {
                mlvb.updateMilitiaItem(militia);
            }
        }

        public static void removeMilitiaItem(Militia militia)
        {//通知所有民兵列表删除
            foreach (MilitiaListViewBiz mlvb in mListBizs)
            {
                mlvb.removeMilitiaItem(militia);
            }
        }

        private static void exportToFolder(string folder)
        {
            if(!Directory.Exists(folder))
            {
                return;
            }
            string exportFolder = folder + "\\" + LoginXmlConfig.Place;//导出的文件夹
            Directory.CreateDirectory(exportFolder);
            if(LoginXmlConfig.ClientType == "基层")
            {
                sqlBiz.exportAsFile(exportFolder + "\\" + exportMilitiaFileName);
            } else
            {
                sqlBiz.backupAllDb(exportFolder);
            }
            groupBiz.exportXmlGroupTask(exportFolder + "\\" + exportGroupFileName);
        }

        public static void export()
        {

            FolderBrowserDialog fbdlg = new FolderBrowserDialog();
            fbdlg.Description = "请选择要导出的文件路径";
            if (fbdlg.ShowDialog() == DialogResult.OK)
            {
                string folder = fbdlg.SelectedPath;
                exportToFolder(folder);
            }
            MessageBox.Show("导出成功");//backup可能还在异步进行
        }

        /*private static void export(string fileName, string psd)
        {//fileName为导出文件，psd为压缩密码
            if (!Directory.Exists("export"))
            {
                Directory.CreateDirectory("export");
            }

            Zip zip = new Zip(fileName, psd, 6);
            if(LoginXmlConfig.ClientType == "基层")
            {
                sqlBiz.exportAsFile("export/militia");
            } else
            {//区县人武部，市军分区，省军分区
                sqlBiz.backupAllDb("export");
            }
            MessageBox.Show("backuping");

            zip.addFileOrFolder("export");
            
   
            zip.addFileOrFolder(GroupXmlConfig.xmlGroupFile);//导出分组文件
            zip.close();

            Directory.Delete("export", true);//删除
        }*/

        private static void importFormFolder(string folder)
        {
            if (!Directory.Exists(folder))
            {
                return;
            }
            if (LoginXmlConfig.ClientType == "区县人武部")
            {//区县人武部导入文件
                sqlBiz.importFormFile(folder + "\\" + exportMilitiaFileName);
            } else
            {//其他导入数据库
                sqlBiz.restoreDbs(folder);
            }

            groupBiz.addXmlGroupTask(folder + "\\" + exportGroupFileName);//导入分组任务

        }

        public static void import()
        {
            MessageBox.Show("开始导入， time = " + DateTime.Now);
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                string folder = fbd.SelectedPath;
                importFormFolder(folder);
            }
            detectConflicts();//检测冲突
            groupBiz.refresh();//刷新分组显示
        }

        public static void detectConflicts()
        {
            List<List<Militia>> mlList = sqlBiz.getConflictMilitias();
            if(mlList.Count == 0)
            {
                MessageBox.Show("没有检测到冲突");
            } else
            {
                MessageBox.Show("检测到" + mlList.Count + "个冲突");
                ConflictMilitiasForm cmf = new ConflictMilitiasForm(mlList);
                cmf.ShowDialog();
            }
        }

        /*public static void import()
        {
            MessageBox.Show("开始导入， time = " + DateTime.Now);
            OpenFileDialog ofdlg = new OpenFileDialog();
            ofdlg.Multiselect = true;//支持多选
            ofdlg.Filter = "民兵编组系统导出文件(*.dump)|*.*";
            if (ofdlg.ShowDialog() == DialogResult.OK)
            {
                string[] files = ofdlg.FileNames;
                foreach (string file in files)
                {
                    importOne(file, "hello");
                }
            }
            MessageBox.Show("导入完毕， time = " + DateTime.Now);
            List<List<Militia>> mlList = sqlBiz.getConflictMilitias();
            MessageBox.Show("拿到冲突, time = " + DateTime.Now);
            if (mlList.Count == 0)
            {
                MessageBox.Show("没有检测到冲突");
            }
            else
            {
                MessageBox.Show("检测到冲突");
                ConflictMilitiasForm cmf = new ConflictMilitiasForm(mlList);
                cmf.ShowDialog();
            }

            groupBiz.refresh();//刷新分组界面显示
        }

        private static void importOne(string importFile, string psd)
        {//导入一个
            if(!Directory.Exists("import"))
            {
                Directory.CreateDirectory("import");
            }
            UnZip unzip = new UnZip(importFile, "import", psd);//解压到数据库中
            List<string> importedDatabases = sqlBiz.importUnzip(unzip);//开始解压
            unzip.close();
            string[] files = Directory.GetFiles("import/export");
            foreach(string file in files)
            {//文件
                sqlBiz.importFormFile(file);
            }
            string[] databases = Directory.GetDirectories("import/export");//数据库
            sqlBiz.restoreDbs(databases.ToList());

            groupBiz.addXmlGroupTask("import/" + GroupXmlConfig.xmlGroupFile);

            Directory.Delete("import", true);//导入之后，删除
        }*/
    }
}
