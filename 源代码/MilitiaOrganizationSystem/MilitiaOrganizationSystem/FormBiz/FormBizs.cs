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
        public const string exportMilitiaFileName = "export/militiaList";

        public const string importDataDir = "import";



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

        /**public static void export(string folder, string name)
        {//在folder下生成一个文件夹作为导出，文件夹的名称为name
            string exportFolder = folder + "\\" + name;
            if (!Directory.Exists(exportFolder))
            {
                Directory.CreateDirectory(exportFolder);
            }
            groupBiz.exportXmlGroupTask(exportFolder + "\\" + exportGroupFileName);//导出分组任务

            string x = "";//客户端类别
            if(x == "基层")
            {
                sqlBiz.exportAsXmlFile(exportFolder + "\\" + exportMilitiaFileName);//导出数据库民兵信息
            } else
            {
                sqlBiz.exportAsSource(exportFolder);//直接将数据库复制到文件夹下
            }
            
        }*/

        public static void export()
        {
            FolderBrowserDialog fbdlg = new FolderBrowserDialog();
            fbdlg.Description = "请选择要导出的文件路径";
            if (fbdlg.ShowDialog() == DialogResult.OK)
            {
                string folder = fbdlg.SelectedPath;
                export(folder + "\\" + LoginXmlConfig.Place + ".zip", "hello");
            }
        }

        private static void export(string fileName, string psd)
        {//fileName为导出文件，psd为压缩密码
            //导出之前，先检测冲突
            List<List<Militia>> mlList = sqlBiz.getConflictMilitiasOfMainDatabase();//主数据库
            if(mlList.Count > 0)
            {//检测到冲突
                ConflictMilitiasForm cmf = new ConflictMilitiasForm(mlList);
                cmf.ShowDialog();
            }

            Zip zip = new Zip(fileName, psd, 6);
            List<string> exportMilitiaFiles = sqlBiz.exportAsXmlFile(exportMilitiaFileName);//为文件
            foreach(string exportFile in exportMilitiaFiles)
            {
                zip.addFileOrFolder(exportFile);
                File.Delete(exportFile);//加入压缩文件后，删去文件
            }
            zip.addFileOrFolder(GroupXmlConfig.xmlGroupFile);//导出分组文件
            zip.close();
        }

        /**public static void importOne(string importFolder)
        {

            List<string> importedDatabases = null;
            
            string x = "";//客户端类别
            if(x == "基层")
            {
                return;
            }
            if (x == "区县")
            {
                sqlBiz.importFromMilitiaXml(importFolder + "\\" + exportMilitiaFileName);
            }
            else
            {//复制数据库文件夹到DataBases
                importedDatabases = sqlBiz.importFromSource(importFolder);//导入成功的所有数据库名
            }

            groupBiz.addXmlGroupTask(importFolder + "\\" + exportGroupFileName, importedDatabases);//导入分组任务

        }*/

        public static void import()
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
            List<List<Militia>> mlList = sqlBiz.getConflictMilitiasOfMainDatabase();
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
            if(!Directory.Exists(importDataDir))
            {
                Directory.CreateDirectory(importDataDir);
            }
            UnZip unzip = new UnZip(importFile, importDataDir, psd);//解压到importDataDir中
            sqlBiz.importUnzip(unzip);//开始解压
            unzip.close();

            string[] files = Directory.GetFiles(importDataDir);
            foreach(string file in files)
            {//导入militiaXml或者GroupXml
                if(Path.GetFileName(file).StartsWith(Path.GetFileName(exportMilitiaFileName)))
                {//militiaList
                    sqlBiz.importFromMilitiaXml(file);
                } else if(file == exportGroupFileName)
                {
                    groupBiz.addXmlGroupTask(file);
                }
            }
            //导入之后，删去import文件夹
            Directory.Delete(importDataDir, true);
        }
    }
}
