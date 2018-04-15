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

        public UserPage()
        {
            InitializeComponent();
        }

        private void viewCapstones_Click(object sender, EventArgs e)
        {
            if (isStaff.Checked)
            {
                if (fh.GetCapstoneListStaff() == null) // in case page has already been created
                {
                    fh.CreateCapstoneListStaff();
                }
                fh.GetCapstoneListStaff().Show();
                fh.GetUserPage().Hide();
            }
            else if (isFaculty.Checked)
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

        private void editProfile_Click(object sender, EventArgs e)
        {
            if(fh.GetUserPageEdit() == null) // in case page has already been created
            {
                fh.CreateUserPageEdit();
            }
            fh.GetUserPageEdit().Show();
            fh.GetUserPage().Hide();
        }

        private void isStaff_CheckedChanged(object sender, EventArgs e)
        {
            if (isStaff.Checked)
            {
                viewUsers.Visible = true;
                viewCapstones.Text = "View Capstones";
            }
            else
            {
                viewUsers.Visible = false;
                viewCapstones.Text = "View Capstone";
            }
        }

        private void isFaculty_CheckedChanged(object sender, EventArgs e)
        {
            if (isFaculty.Checked)
            {
                viewCapstones.Text = "View Capstones";
            }
            else
            {
                viewCapstones.Text = "View Capstone";
            }
        }
    }
}
