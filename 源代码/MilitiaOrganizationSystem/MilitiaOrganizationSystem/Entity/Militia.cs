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
        public string Group { get; set; }//分组Id
        public string Id { get; set; }//数据库会自动给它赋值

        public Militia()
        {
            InfoDic = new Dictionary<string, string>();
            Group = "未分组";
        }

        public string info()
        {
            return "姓名：" + InfoDic["Name"] + ", 身份证：" + InfoDic["CredentialNumber"];
        }

        public bool satisfy()
        {
            return true;
        }
    }
}
