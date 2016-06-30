using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitiaOrganizationSystem.Entity
{
    class Condition
    {
        public System.Linq.Expressions.Expression<Func<Militia, bool>> lambdaContition { get; set; }
        public string place { get; set; }//该页面的查询条件之一指定数据库
        private Dictionary<string, ChildCondition> conditionDict { get; set; }
        //查询条件的集合, key是PropertyName
        
        public Condition()
        {//构造函数
            lambdaContition = null;
            place = null;
        }

        private class ChildCondition
        {//一个条件
            public string PropertyType { get; set; }
            public string Method { get; set; }
            public List<string> Values { get; set; }


            public ChildCondition()
            {
                PropertyType = null;
                Method = null;
                Values = new List<string>();
            }
        }
    }
}
