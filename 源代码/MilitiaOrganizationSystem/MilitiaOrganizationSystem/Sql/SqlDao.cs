using Raven.Client;
using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Raven.Database.Smuggler;
using Raven.Abstractions.Smuggler;
using Raven.Abstractions.Data;
using System.Linq.Expressions;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace MilitiaOrganizationSystem
{
    class SqlDao
    {//数据库访问层
        private string dbName;
        private EmbeddableDocumentStore store;

        private const int timeoutseconds = 5;

        public SqlDao(string db)
        {
            this.dbName = db;

            newStore();
        }

        private void newStore()
        {//新建store并初始化

            store = new EmbeddableDocumentStore()
            {
                DefaultDatabase = dbName
            };

            //store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;

            store.Initialize();

            new Militias_CredentialNumbers().Execute(store);
            new Militias_Groups().Execute(store);   

        }
        

        public void saveMilitia(Militia militia)
        {//保存一个民兵，若Id相同，会覆盖数据库里的(省市须指定数据库)
            if(militia.Place == null)
            {
                militia.Place = dbName;
            }

            string database = militia.Place;//指定数据库

            using (var session = store.OpenSession(database))
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
        

        public List<Militia> queryMilitiasByGroup(string groupPath)
        {
            using (var session = store.OpenSession())
            {
                var mlist = from x in session.Query<Militia>()
                            where x.Group.StartsWith(groupPath)
                            select x;
                return mlist.ToList();
            }
        }


        public void deleteOneMilitia(Militia militia)
        {//从数据库中删除一个民兵，（省市级别须指定数据库）
            string database = militia.Place;//指定数据库

            using (var session = store.OpenSession(database))
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
        }//这个有希望代替直接复制，据说直接复制对数据库会造成损害，但是restore的时候我只能restore一个，连续restore两个就会造成冲突

        /**public async void exportDocumentDataBase(string exportFolder)
        {
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
                From = new EmbeddedRavenConnectionStringOptions(),
                ToFile = exportFolder + "\\" + dbName + ".dump"
            };

            await dataDumper.ExportData(exportOptions);

        }*///导出只能导出DocumentDataBase或SystemDataBase的数据，我不知道其他数据库该如何导出

        /**public async void importToDocumentDataBase(string dumpFile)
        {
            var dataDumper = new DatabaseDataDumper(
                store.DocumentDatabase,
                new SmugglerDatabaseOptions
                {
                    OperateOnTypes = ItemType.Documents | ItemType.Indexes | ItemType.Attachments | ItemType.Transformers,
                    Incremental = false,

                }
            );
            SmugglerImportOptions<RavenConnectionStringOptions> importOptions = new SmugglerImportOptions<RavenConnectionStringOptions>
            {
                FromFile = dumpFile,
                To = new EmbeddedRavenConnectionStringOptions()
            };

            await dataDumper.ImportData(importOptions);

            MessageBox.Show("complete?");
        }*///import的话如果id相同，会覆盖掉原数据库中的数据

        public void copyDbTo(string folder)
        {//直接将除System之外的数据库复制出去
            store.Dispose();//先释放strore，才能copy

            DirectoryInfo dirInfo = new DirectoryInfo(SqlBiz.DataDir);
            DirectoryInfo[] dis = dirInfo.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                if (di.Name != "System")
                {//除了System数据库，其他的都导出
                    FileTool.CopyFolder(di.FullName, folder);
                    //sqlDao.backupOneDB(di.Name, folder);
                }
            }

            newStore();
        }

        public void zipDb(Zip zip)
        {//直接将除System之外的数据库添加到压缩文件中
            store.Dispose();//先释放strore，才能zip

            DirectoryInfo dirInfo = new DirectoryInfo(SqlBiz.DataDir);
            DirectoryInfo[] dis = dirInfo.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                if (di.Name != "System")
                {//除了System数据库，其他的都导出
                    zip.addFileOrFolder(SqlBiz.DataDir + "\\" + di.Name);
                }
            }

            newStore();
        }
        
        
        public List<Militia> queryByContition(Expression<Func<Militia, bool>> lambdaContition, int skip, int take, out int sum, string database = null)
        {//通过lambda表达式查询数据库database里的东西
            if(database == null)
            {
                database = dbName;
            }
            store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(database);
            using(var session = store.OpenSession(database))
            {
                RavenQueryStatistics stats;
                var mList =  session.Query<Militia>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(timeoutseconds)))
                    .Statistics(out stats).Where(lambdaContition).Skip(skip).Take(take).ToList();
                sum = stats.TotalResults;

                return mList;
            }
        }

        /*public Dictionary<string, Militias_Groups.Result> getGroups(int skip, int take, out int sum, string database = null)
        {//通过静态索引查询组内民兵个数
            if (database == null)
            {
                database = dbName;
            }

            using(var session = store.OpenSession(database))
            {
                RavenQueryStatistics stats;
                var gDictionary = session.Query<Militias_Groups.Result, Militias_Groups>().Statistics(out stats).Skip(skip).Take(take).ToDictionary(x => x.Group);
                sum = stats.TotalResults;

                return gDictionary;
            }
        }*/

        public List<FacetValue> getGroupNums(string database = null)
        {//通过静态索引查询组内民兵个数
            if (database == null)
            {
                database = dbName;
            }
            store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(database);
            using (var session = store.OpenSession(database))
            {
                List<FacetValue> fList;
                var gfacetResults = session.Query<Militias_Groups.Result, Militias_Groups>()
                     //.ProjectFromIndexFieldsInto<Militias_Groups.Result>()
                     .AggregateBy(x => x.Group).CountOn(x => x.Group).ToList();
                
                fList = gfacetResults.Results["Group"].Values;

                return fList;
            }

        }

        public List<FacetValue> getAggregateNums(Expression<Func<Militia, bool>> lambdaContition, string propertyName, string database = null)
        {//统计
            if (database == null)
            {
                database = dbName;
            }

            var parameter = Expression.Parameter(typeof(Militia), "x");
            var property = Expression.Property(parameter, propertyName);
            var propertyExpression = Expression.Lambda<Func<Militia, object>>(property, parameter);

            store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(database);
            using (var session = store.OpenSession(database))
            {
                var gfacetResults = session.Query<Militia>()
                    .Customize(x => x.WaitForNonStaleResultsAsOfNow(TimeSpan.FromSeconds(timeoutseconds)))
                    .Where(lambdaContition)
                    .AggregateBy(propertyExpression).CountOn(x => x.Group).ToList();
                foreach (string key in gfacetResults.Results.Keys)
                {
                    System.Windows.MessageBox.Show(key);
                }
                return gfacetResults.Results[propertyName].Values;
            }
        }

        public List<Militia> getMilitiasOfGroup(string Group, int skip, int take, out int sum, string database = null)
        {//通过指定的Group(可以是非叶结点)，查询lambda表达式限定下的民兵列表
            if (database == null)
            {
                database = dbName;
            }
            
            using (var session = store.OpenSession(database))
            {
                RavenQueryStatistics stats;
                var militias = session.Query<Militias_Groups.Result, Militias_Groups>().Statistics(out stats)
                    .Where(x => x.Group.StartsWith(Group)).OfType<Militia>() //转换为militias
                    .Skip(skip).Take(take).ToList();
                sum = stats.TotalResults;

                return militias;
            }
        }

        public List<Militias_CredentialNumbers.Result> getCredentialNumbers(int skip, int take, out int sum, string database = null)
        {//获取身份证号（从小到大排序了的）
            if (database == null)
            {
                database = dbName;
            }

            using (var session = store.OpenSession(database))
            {
                RavenQueryStatistics stats;
                var credentialNumbers = session.Query<Militias_CredentialNumbers.Result, Militias_CredentialNumbers>()
                    .Statistics(out stats)
                    .Skip(skip).Take(take)
                    .ProjectFromIndexFieldsInto<Militias_CredentialNumbers.Result>()
                    .OrderBy(x => x.CredentialNumber)
                    .ToList();

                sum = stats.TotalResults;

                foreach(Militias_CredentialNumbers.Result r in credentialNumbers)
                {//给r.DbName赋值，表示从这个数据库查出来的
                    r.DbName = database;
                }

                return credentialNumbers;
            }
        }

        public List<Militias_CredentialNumbers.Result> getAllCredentialNumbers(string database = null)
        {//获取一个数据库的所有身份证号（从小到大排序了的）
            if (database == null)
            {
                database = dbName;
            }

            store.DatabaseCommands.GlobalAdmin.EnsureDatabaseExists(database);

            //new Militias_CredentialNumbers().Execute(store.DatabaseCommands.ForDatabase(database), store.Conventions);

            using (var session = store.OpenSession(database))
            {
                int sum = 1;
                List<Militias_CredentialNumbers.Result> rList = new List<Militias_CredentialNumbers.Result>();

                while(rList.Count < sum)
                {
                    RavenQueryStatistics stats;
                    var credentialNumbers = session.Query<Militias_CredentialNumbers.Result, Militias_CredentialNumbers>()
                        .Statistics(out stats)
                        .Skip(rList.Count).Take(1000)
                        .ProjectFromIndexFieldsInto<Militias_CredentialNumbers.Result>()
                        .OrderBy(x => x.CredentialNumber)
                        .ToList();

                    sum = stats.TotalResults;

                    foreach (Militias_CredentialNumbers.Result r in credentialNumbers)
                    {//给r.DbName赋值，表示从这个数据库查出来的
                        r.DbName = database;
                    }

                    rList.AddRange(credentialNumbers);
                }
                

                return rList;
            }

        }

        public List<Militia> getMilitiasByCredentialNumber(string CredentialNumber, string database = null)
        {//根据身份证号获取民兵
            if (database == null)
            {
                database = dbName;
            }

            using (var session = store.OpenSession(database))
            {
                var mList = session.Query<Militias_CredentialNumbers.Result, Militias_CredentialNumbers>()
                    .Where(x => x.CredentialNumber == CredentialNumber)
                    .Skip(0).Take(1000)
                    .OfType<Militia>()
                    .ToList();
                

                return mList;
            }
        }

    }

    public class Militias_CredentialNumbers : AbstractIndexCreationTask<Militia, Militias_CredentialNumbers.Result>
    {
        public class Result
        {
            public string CredentialNumber { get; set; } //身份证号
            
            public string DbName { get; set; } //数据库名，之后冲突检测的时候使用
        }

        public Militias_CredentialNumbers()
        {
            Map = militias => from militia in militias
                              select new
                              {
                                  CredentialNumber = militia.CredentialNumber
                              };

            Sort(r => r.CredentialNumber, Raven.Abstractions.Indexing.SortOptions.String);
        }
    }

    public class Militias_Groups : AbstractIndexCreationTask<Militia>
    {
        public class Result
        {
            public string Group { get; set; } //组名
        }

        public Militias_Groups()
        {
            Map = militias => from militia in militias
                              select new
                              {
                                  Group = militia.Group
                              };

            
        }
    }
}
