using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ISTE.BAL.Implementations;

namespace PresentationLayer
{
    // Login form that accepts user input and validates login information
    // Author: Jake Toporoff
    public partial class Login : Form
    {
        FormHandler fh = FormHandler.Instance;

        // Initialize all events not created through the form
        public Login()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.Login_FormClosed);
        }

        // Closes entire application when the x button is pressed
        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Attempts to log the user in
        private void loginButton_Click(object sender, EventArgs e)
        {
            if (ValidateLogin())
            {
                fh.Settings.UserPage.Show();
                fh.Settings.Login.Hide();
            }
            else
            {
                error.Visible = true;
            }
        }

        // Checks to make sure the user entered proper login info
        private bool ValidateLogin()
        {
            // Create current user.
            BusinessUser newUser = new BusinessUser(fh.Database, username.Text, password.Text);

            // Add user to the form handler.
            fh.Settings.CurrentUser = newUser;

            // Login.
            BusinessLogin businessLogic = fh.BusinessLogin;
            return businessLogic.Login();
        }
    }
}
