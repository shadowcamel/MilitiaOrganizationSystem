using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitiaOrganizationSystem
{
    public class SqlBiz
    {//业务逻辑层
        private SqlDao sqlDao;//数据访问层

        public SqlBiz(string dbName)
        {
            sqlDao = new SqlDao(dbName);//根据数据库实例化数据访问层
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

    }
}
