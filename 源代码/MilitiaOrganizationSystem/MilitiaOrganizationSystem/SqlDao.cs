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

        private bool condition(Militia x)
        {
            return true;
        }

        public List<Militia> queryMilitiasByGroup(string groupPath)
        {
            using (var session = store.OpenSession())
            {
                var mlist = from x in session.Query<Militia>()
                            where x.Group == groupPath
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

        
    }
}
