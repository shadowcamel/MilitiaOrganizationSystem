using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MilitiaOrganizationSystem
{
    public partial class ChildConditionForm : Form
    {
        protected bool closeForm;

        public ChildConditionForm()
        {
            InitializeComponent();
            closeForm = true;//是否关闭
            FormClosing += ChildConditionForm_FormClosing;
        }

        private void ChildConditionForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!closeForm)
            {
                closeForm = true;
                e.Cancel = true;
            }
        }
    }
}
