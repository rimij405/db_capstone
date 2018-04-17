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
    public partial class UserPage : Form
    {
        FormHandler fh = FormHandler.Instance;
        bool isStaff, isFaculty = false; // to determine user type

        public UserPage()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.UserPage_FormClosed);

            LoadValues();
        }

        private void LoadValues()
        {
            if(isStaff || isFaculty)
            {
                viewUsers.Visible = true;
            }
        }

        private void UserPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void viewCapstones_Click(object sender, EventArgs e)
        {
            if (isStaff)
            {
                if (fh.GetCapstoneListStaff() == null) // in case page has already been created
                {
                    fh.CreateCapstoneListStaff();
                }
                fh.GetCapstoneListStaff().Show();
                fh.GetUserPage().Hide();
            }
            else if (isFaculty)
            {
                if (fh.GetCapstoneListFaculty() == null) // in case page has already been created
                {
                    fh.CreateCapstoneListFaculty();
                }
                fh.GetCapstoneListFaculty().Show();
                fh.GetUserPage().Hide();
            }
            else
            {
                if (fh.GetCapstonePageView() == null) // in case page has already been created
                {
                    fh.CreateCapstonePageView();
                }
                fh.GetCapstonePageView().Show();
                fh.GetUserPage().Hide();
            }
        }

        private void viewUsers_Click(object sender, EventArgs e)
        {
            if(fh.GetUserList() == null) // in case page has already been created
            {
                fh.CreateUserList();
            }
            fh.GetUserList().Show();
            fh.GetUserPage().Hide();
        }

        private void editProfile_Click(object sender, EventArgs e)
        {
            if(fh.GetUserPageEdit() == null) // in case page has already been created
            {
                fh.CreateUserPageEdit();
            }
            fh.GetUserPageEdit().Show();
            fh.GetUserPage().Hide();
        }
    }
}
