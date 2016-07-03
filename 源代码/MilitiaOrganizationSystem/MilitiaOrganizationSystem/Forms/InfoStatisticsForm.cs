using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MilitiaOrganizationSystem
{
    public partial class InfoStatisticsForm : Form
    {
        private System.Xml.XmlNodeList parameters = MilitiaXmlConfig.parameters;
        private XmlNode statisticsParameter = null;

        private Condition condition;//统计界面的筛选条件

        public InfoStatisticsForm(Condition c)
        {
            InitializeComponent();

            //初始化属性显示
            for (int i = 0; i < parameters.Count; i++)
            {//添加属性名
                System.Xml.XmlNode xn = parameters[i];
                propertyCombobox.Items.Add(xn.Attributes["name"].Value);
            }

            condition = new Condition(c);//复制传过来的条件

            propertyCombobox.SelectedIndexChanged += PropertyCombobox_SelectedIndexChanged;
        }

        private void PropertyCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            statisticsParameter = parameters[propertyCombobox.SelectedIndex];
            switch(statisticsParameter.Attributes["type"].Value)
            {
                case "enum":
                    foreach(XmlNode xn in statisticsParameter.ChildNodes)
                    {
                        catagoriesListBox.Items.Add(xn.Attributes["name"].Value);
                    }
                    break;
                default:
                    break;
            }
        }

        private void btn_statistics_Click(object sender, EventArgs e)
        {//统计
            
            if(statisticsParameter == null)
            {
                return;
            }

            statisticsListBox.Items.Clear();
            switch(statisticsParameter.Attributes["type"].Value)
            {
                case "enum":
                    Dictionary<string, Raven.Abstractions.Data.FacetValue> fdict
                        = FormBizs.sqlBiz.getEnumStatistics(
                            condition.lambdaCondition, 
                            statisticsParameter.Attributes["property"].Value, 
                            condition.place);
                    foreach(XmlNode xn in statisticsParameter.ChildNodes)
                    {
                        int num = 0;
                        Raven.Abstractions.Data.FacetValue fv = null;
                        if(fdict.TryGetValue(xn.Attributes["value"].Value, out fv))
                        {//数量
                            num = fv.Hits;
                        }
                        statisticsListBox.Items.Add(xn.Attributes["name"].Value + ": " + num + "人");
                    }

                    break;
                default:
                    break;
            }
        }
    }
}
