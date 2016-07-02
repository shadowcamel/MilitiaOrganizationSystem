using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MilitiaOrganizationSystem
{
    class MilitiaReflection
    {
        private static Type mt = typeof(Militia);
        private static Dictionary<string, PropertyInfo> piDict = mt.GetProperties().ToDictionary(x => x.Name);//属性控制字典
        public static Dictionary<string, PropertyInfo>.KeyCollection keys = piDict.Keys;//所有的属性名称

        private Militia militia;

        public MilitiaReflection(Militia m)
        {
            militia = m;
        }

        public object getProperty(string property)
        {//反射获取属性值
            return piDict[property].GetValue(militia);
        }

        public void setProperty(string property, object value)
        {//反射设置属性值
            piDict[property].SetValue(militia, value);
        }

    }
}
