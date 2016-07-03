using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MilitiaOrganizationSystem
{
    public partial class LoginForm : Form
    {
        private bool closeForm;
        public LoginForm()
        {
            InitializeComponent();
            closeForm = true;

            FormClosing += LoginForm_FormClosing;
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!closeForm)
            {
                closeForm = true;
                e.Cancel = true;
            }
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            if(PsdXmlConfig.loginSuccess(psdCombobox.Text))
            {
                MessageBox.Show("口令正确");
            } else
            {
                MessageBox.Show("口令错误！");
                closeForm = false;
            }
        }

        private void btn_modify_Click(object sender, EventArgs e)
        {

        }
    }
}
