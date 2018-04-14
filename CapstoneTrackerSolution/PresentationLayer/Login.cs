﻿using System;
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
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (username.Text != "" && password.Text != "")
            {
                fh.CreateUserPage();
                fh.GetUserPage().Show();
                fh.GetLogin().Close();
            }
            else
            {
                error.Visible = true;
            }
        }
    }
}
