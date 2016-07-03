using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MilitiaOrganizationSystem
{
    static class FormBizs
    {//全局管理Form的类
        public const string exportGroupFileName = "groupTask.xml";
        public const string exportMilitiaFileName = "militiaList.xml";


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

        public static void export(string fileName, string psd)
        {//fileName为导出文件，psd为压缩密码
            //导出之前，先检测冲突
            List<List<Militia>> mlList = sqlBiz.getConflictMilitiasOfMainDatabase();//主数据库
            if(mlList.Count > 0)
            {//检测到冲突
                ConflictMilitiasForm cmf = new ConflictMilitiasForm(mlList);
                cmf.ShowDialog();
            }
            mlList = sqlBiz.getConflictMilitiasBetweenDatabases();//数据库之间
            if (mlList.Count > 0)
            {//检测到冲突
                ConflictMilitiasForm cmf = new ConflictMilitiasForm(mlList);
                cmf.ShowDialog();
            }

            Zip zip = new Zip(fileName, psd, 6);
            if(LoginXmlConfig.ClientType == "基层")
            {
                sqlBiz.exportAsXmlFile(exportMilitiaFileName);
                zip.addFileOrFolder(exportMilitiaFileName);
            } else
            {//其他客户端导出整个数据库
                sqlBiz.exportZip(zip);
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

        public static void importOne(string importFile, string psd)
        {//导入一个
            UnZip unzip = new UnZip(importFile, SqlBiz.DataDir, psd);
            List<string> importedDatabases = sqlBiz.importUnzip(unzip);
            if(LoginXmlConfig.Place == "区县人武部")
            {//如果是区县人武部
                sqlBiz.importFromMilitiaXml(SqlBiz.DataDir + "\\" + Path.GetFileName(exportMilitiaFileName));
                //先导入数据库，然后刷新分组任务显示
                groupBiz.refresh();
            } else
            {//市军分区和省军分区则直接将刚才导入的数据库里的分组和各自的民兵数量加载到显示
                groupBiz.addXmlGroupTask(SqlBiz.DataDir + "\\" + Path.GetFileName(GroupXmlConfig.xmlGroupFile), importedDatabases);
            }
            
        }
    }
}
