using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilitiaOrganizationSystem
{
    static class Program
    {

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            initial();//初始化静态类l

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (PsdXmlConfig.ClientType == "")
            {
                SetForm sf = new SetForm();
                if(sf.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            } else
            {
                LoginForm lf = new LoginForm();
                if(lf.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }

            Form mainForm = null;
            switch(PsdXmlConfig.ClientType)
            {
                case "省军分区":
                    mainForm = new ProvinceForm();
                    break;
                case "市军分区":
                    mainForm = new CityForm();
                    break;
                case "区县人武部":
                    mainForm = new DistrictForm();
                    break;
                case "基层":
                    mainForm = new BasicLevelForm();
                    break;
            }

            Application.Run(mainForm);
        }

        static void initial()
        {//静态初始化
            MilitiaXmlConfig.initial();
            PlaceXmlConfig.initial();
            GroupXmlConfig.initial();
            PsdXmlConfig.initial();
        }
    }
}
