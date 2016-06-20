using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitiaOrganizationSystem
{
    public class Condition
    {
        public Condition()
        {//构造函数
            
        }

        public bool match(Militia militia)
        {
            if(militia.Group == "未分组")
            {
                return true;
            }

            return false;
        }
    }
}
