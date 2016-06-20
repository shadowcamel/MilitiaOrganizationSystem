using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MilitiaOrganizationSystem
{
    public class SqlBiz
    {//业务逻辑层
        public const string DataDir = "DataBases";

        private SqlDao sqlDao;//数据访问层

        public SqlBiz(string dbName)
        {
            sqlDao = new SqlDao(dbName);//根据数据库实例化数据访问层

            FormBizs.sqlBiz = this;//程序中唯一的sqlBiz实例
        }

        public void addMilitia(Militia militia)
        {//增
            sqlDao.saveMilitia(militia);
        }

        public void updateMilitia(Militia militia)
        {//改
            sqlDao.saveMilitia(militia);
        }

        public void deleteMilitia(Militia militia)
        {//删
            sqlDao.deleteOneMilitia(militia);
        }

        public List<Militia> getAllMilitias()
        {//获取所有民兵信息
            return sqlDao.queryAllMilitias();
        }
        

        public List<Militia> getMilitiasByGroup(string group)
        {
            return sqlDao.queryMilitiasByGroup(group);
        }

        public int getCountByGroup(string group)
        {
            return 2;
        }

        public void BulkInsertMilitias(List<Militia> mList)
        {
            sqlDao.bulkInsertMilitias(mList);
        }

        public void exportAsXmlFile(string fileName)
        {//将数据库里的所有民兵信息写入xml文件中
            List<Militia> mList = getAllMilitias();
            FileTool.MilitiaListToXml(mList, fileName);
        }

        public void exportAsSource(string folder)
        {//将全部数据库（System除外，因为没有权限根本复制不了）复制到folder文件夹下
            /*DirectoryInfo dirInfo = new DirectoryInfo(DataDir);
            DirectoryInfo[] dis = dirInfo.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                if (di.Name != "System2")
                {
                    //FileTool.CopyFolder(di.FullName, folder);
                    //sqlDao.backupOneDB(di.Name, folder);
                    //sqlDao.exportOneDB(di.Name, folder);
                }
            }*/
            sqlDao.copyDbTo(folder);
        }

        public void importFromMilitiaXml(string fileName)
        {
            List<Militia> mList = FileTool.XmlToMilitiaList(fileName);
            if(mList == null)
            {
                throw new Exception("导入民兵xml文件异常");
            } else
            {
                BulkInsertMilitias(mList);
            }
            
        }

        public void importFromSource(string folder)
        {//folder下全部是数据库文件夹，导入此文件夹下的所有数据库文件夹(复制到DataBases文件夹下)
            string[] dbpaths = Directory.GetDirectories(folder);
            foreach(string dbpath in dbpaths)
            {
                sqlDao.restoreOneDB(dbpath);
            }
        }

        

    }
}
