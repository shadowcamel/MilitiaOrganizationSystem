using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MilitiaOrganizationSystem
{
    static class FormBizs
    {
        public const string exportGroupFileName = "groupTask.xml";
        public const string exportMilitiaFileName = "militiaList.xml";


        public static SqlBiz sqlBiz = null;
        public static XMLGroupTreeViewBiz groupBiz = null;
        public static List<MilitiaListViewBiz> mListBizs = new List<MilitiaListViewBiz>();


        public static void updateMilitiaItem(Militia militia)
        {
            foreach(MilitiaListViewBiz mlvb in mListBizs)
            {
                mlvb.updateMilitiaItem(militia);
            }
        }

        public static void removeMilitiaItem(Militia militia)
        {
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
            Zip zip = new Zip(fileName, psd, 6);
            sqlBiz.exportZip(zip);
            zip.addFileOrFolder(groupBiz.groupFile);
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
        {
            UnZip unzip = new UnZip(importFile, SqlBiz.DataDir, psd);
            List<string> importedDatabases = sqlBiz.importUnzip(unzip);
            groupBiz.addXmlGroupTask(SqlBiz.DataDir + "\\" + Path.GetFileName(groupBiz.groupFile), importedDatabases);
            List<List<Militia>> mlList = sqlBiz.getConflictMilitiasBetweenDatabases();
            System.Windows.MessageBox.Show("count = " + mlList.Count);
        }
    }
}
