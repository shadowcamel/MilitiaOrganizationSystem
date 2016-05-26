using Raven.Client;
using Raven.Client.Embedded;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitiaOrganizationSystem
{
    class SqlDao
    {//数据库访问层
        private string dbName;
        private IDocumentSession session;//数据库的session
        private EmbeddableDocumentStore store;

        private int time;//saveChanges的次数

        public SqlDao(string db)
        {
            this.dbName = db;

            newStoreSession();
        }

        private void newStoreSession()
        {
            time = 0;//初始化次数为0

            store = new EmbeddableDocumentStore()
            {
                DataDirectory = dbName
            };
            store.Initialize();

            session = store.OpenSession();//连接数据库
        }

        public void saveChanges()
        {
            session.SaveChanges();
            time++;
            if(time > 25)
            {//重新开启store和session
                session.Dispose();
                store.Dispose();

                newStoreSession();
            }
        }

        public void saveMilitia(Militia militia)
        {//保存一个民兵信息

            session.Store(militia);
            saveChanges();
        }

        public List<Militia> queryAllMilitias()
        {//返回所有的民兵信息
            
            return session.Query<Militia>().ToList();
        }


        public void deleteOneMilitia(Militia militia)
        {//从数据库中删除一个民兵
            session.Delete(militia.Id);
            saveChanges();
        }

        
    }
}
