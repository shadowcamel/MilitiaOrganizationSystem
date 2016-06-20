using Raven.Client;
using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using Raven.Database.Smuggler;
using Raven.Abstractions.Smuggler;
using Raven.Abstractions.Data;

namespace MilitiaOrganizationSystem
{
    class SqlDao
    {//数据库访问层
        private string dbName;
        private EmbeddableDocumentStore store;

        //private int time;//saveChanges的次数

        public SqlDao(string db)
        {
            this.dbName = db;

            newStoreSession();
        }

        private void newStoreSession()
        {
            //time = 0;//初始化次数为0

            store = new EmbeddableDocumentStore()
            {
                DefaultDatabase = dbName//如果在这里设置了dbName，那么好像没有权限复制出去。。
            };
            store.Initialize();
            
        }

        /*public void saveChanges()
        {
            session.SaveChanges();
            time++;
            if(time > 25)
            {//重新开启store和session
                session.Dispose();
                store.Dispose();
            }
        }*/
        

        public void saveMilitia(Militia militia)
        {
            using(var session = store.OpenSession())
            {
                session.Store(militia);
                session.SaveChanges();
            }
            
        }

        public void bulkInsertMilitias(List<Militia> mList)
        {//批量插入
            using (var bulkInsert = store.BulkInsert())
            {
                foreach (Militia m in mList)
                {
                    bulkInsert.Store(m);
                }
            }
        }

        public List<Militia> queryAllMilitias()
        {//返回所有的民兵信息
            using (var session = store.OpenSession())
            {
                var mlist = from x in session.Query<Militia>()
                            where x.Id != ""
                            select x;
                return mlist.ToList();
            }
            
        }
        

        public List<Militia> queryMilitiasByGroup(string groupPath)
        {
            using (var session = store.OpenSession())
            {
                var mlist = from x in session.Query<Militia>()
                            where x.Group.StartsWith(groupPath)
                            select x;
                return mlist.ToList();
            }
            /*var militias = from x in session.Query<Militia>()
                           where x.@group.Equals(groupPath)
                           select x;
            List<Militia> mList = militias.ToList();
            if(mList == null)
            {
                mList = new List<Militia>();
            }
            return mList;*/
        }


        public void deleteOneMilitia(Militia militia)
        {//从数据库中删除一个民兵
            using (var session = store.OpenSession())
            {
                session.Delete(militia.Id);
                session.SaveChanges();
            }
        }

        public void backupOneDB(string dbName, string exportFolder)
        {//dbName数据库名，exportFolder导出文件夹路径，会在路径下创建一个名为dbName的新文件夹
            string dbFolder = exportFolder + "\\" + dbName;
            if(!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }
            store.DatabaseCommands.GlobalAdmin.StartBackup(dbFolder, null, false, dbName);
        }

        public void restoreOneDB(string importFolder)
        {//importFolder是备份数据库的文件夹路径,文件夹名即为数据库名
            DirectoryInfo dirInfo = new DirectoryInfo(importFolder);
            store.DatabaseCommands.GlobalAdmin.StartRestore(new Raven.Abstractions.Data.DatabaseRestoreRequest
            {
                BackupLocation = dirInfo.FullName,
                DatabaseName = dirInfo.Name
            });
        }

        public async void exportOneDB(string dbName, string exportFolder)
        {
            MessageBox.Show(store.DocumentDatabase.Name);
            var dataDumper = new DatabaseDataDumper(
                store.DocumentDatabase,
                new SmugglerDatabaseOptions
                {
                    OperateOnTypes = ItemType.Documents | ItemType.Indexes | ItemType.Attachments | ItemType.Transformers,
                    Incremental = false,
                
                }
            );

            SmugglerExportOptions<RavenConnectionStringOptions> exportOptions = new SmugglerExportOptions<RavenConnectionStringOptions>
            {
                From = new EmbeddedRavenConnectionStringOptions
                {
                    DataDirectory = "DataBases",
                    DefaultDatabase = dbName
                },
                ToFile = exportFolder + "\\" + dbName + ".dump"
            };

            await dataDumper.ExportData(exportOptions);
        }

        public void copyDbTo(string folder)
        {
            store.Dispose();//先释放strore，才能copy

            DirectoryInfo dirInfo = new DirectoryInfo(SqlBiz.DataDir);
            DirectoryInfo[] dis = dirInfo.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                if (di.Name != "System2")
                {
                    FileTool.CopyFolder(di.FullName, folder);
                    //sqlDao.backupOneDB(di.Name, folder);
                }
            }

            newStoreSession();
        }
        
    }
}
