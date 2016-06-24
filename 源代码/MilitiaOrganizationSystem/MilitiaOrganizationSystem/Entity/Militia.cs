using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitiaOrganizationSystem
{
    public class Militia
    {//民兵类
        public Dictionary<string, string> InfoDic { get; }//信息字典
        public string Group { get; set; }//分组路径
        public string Place { get; set; }//采集地ID，也是数据库名
        public string CredentialNumber { get; set; }//身份证号，应该是唯一标志的，但可能会冲突
        public string Id { get; set; }//数据库会自动给它赋值

        public Militia()
        {
            InfoDic = new Dictionary<string, string>();//属性字典，根据MilitiaXmlConfig配置
            Group = "未分组";//分组
            Place = null;//数据库名初始化为null
        }

        public string info()
        {
            return "姓名：" + InfoDic["Name"] + ", 身份证：" + InfoDic["CredentialNumber"];
        }

        public bool isEqual(Militia m)
        {//判断两个民兵是否是一模一样
            if(m.Id == Id && m.Place == Place)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool satisfy()
        {
            return true;
        }
    }
}
