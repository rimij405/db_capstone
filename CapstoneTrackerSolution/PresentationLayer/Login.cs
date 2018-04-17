using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class Login : Form
    {
        FormHandler fh = FormHandler.Instance;

        public Login()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.Login_FormClosed);
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (ValidateLogin())
            {
                if (fh.GetUserPage() == null) // in case page has already been created
                {
                    fh.CreateUserPage();
                }
                fh.GetUserPage().Show();
                fh.GetLogin().Hide();
            }
            else
            {
                error.Visible = true;
            }
        }

        // check to make sure the user entered proper login info
        private bool ValidateLogin()
        {
            if (fh.LoginSetPassword())
                return fh.LoginSetUserName();
            else
                return false;
        }
    }
}
