using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilitiaOrganizationSystem
{
    static class Program
    {
        public static Dictionary<string, Form> formDic = new Dictionary<string, Form>();//运行的Form字典

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            initial();//初始化静态类

            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            BasicLevelForm basicLevelForm = new BasicLevelForm();
            formDic["MilitiaForm"] = basicLevelForm;


            Application.Run(basicLevelForm);
        }

        static void initial()
        {//静态初始化
            MilitiaXmlConfig.initial();
        }
    }
}
