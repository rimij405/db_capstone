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
    public partial class CapstoneListFaculty : Form
    {
        FormHandler fh = FormHandler.Instance;

        public CapstoneListFaculty()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.CapstoneListFaculty_FormClosed);
            this.capstonePendingList.DoubleClick += CapstonePending_DoubleClick;
            this.capstoneCurrentList.DoubleClick += CapstoneCurrent_DoubleClick;

            LoadValues();
        }

        private void LoadValues()
        {
            capstonePendingList.Items.Add("My capstone");
            capstoneCurrentList.Items.Add("My other capstone");
            capstoneGradeList.Items.Add("45%");
        }

        private void CapstoneCurrent_DoubleClick(object sender, EventArgs e)
        {
            if (capstoneCurrentList.SelectedItem != null)
            {
                if (fh.GetCapstonePageView() == null) // in case page has already been created
                {
                    fh.CreateCapstonePageView();
                }
                fh.GetCapstonePageView().Show();
                fh.GetCapstoneListFaculty().Hide();
            }
        }

        private void CapstonePending_DoubleClick(object sender, EventArgs e)
        {
            if (capstonePendingList.SelectedItem != null)
            {
                if (fh.GetCapstonePageView() == null) // in case page has already been created
                {
                    fh.CreateCapstonePageView();
                }
                fh.GetCapstonePageView().Show();
                fh.GetCapstoneListFaculty().Hide();
            }
        }

        private void CapstoneListFaculty_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void userPageReturn_Click(object sender, EventArgs e)
        {
            if (fh.GetUserPage() == null) // in case page has already been created
            {
                fh.CreateUserPage();
            }
            fh.GetUserPage().Show();
            fh.GetCapstoneListFaculty().Hide();
        }

        private void acceptCapstone_Click(object sender, EventArgs e)
        {
            if (capstonePendingList.SelectedItem != null)
            {
                capstoneCurrentList.Items.Add(capstonePendingList.SelectedItem);
                capstonePendingList.Items.Remove(capstonePendingList.SelectedItem);
                error.Text = "Successfully accepted capstone committee request";
                error.BackColor = Color.DarkSeaGreen;
                error.Visible = true;
            }
            else
            {
                error.Text = "No capstone selected";
                error.BackColor = Color.DarkSalmon;
                error.Visible = true;
            }
        }

        private void rejectCapstone_Click(object sender, EventArgs e)
        {
            if (capstonePendingList.SelectedItem != null)
            {
                capstonePendingList.Items.Remove(capstonePendingList.SelectedItem);
                error.Text = "Successfully declined capstone committee request";
                error.BackColor = Color.DarkSeaGreen;
                error.Visible = true;
            }
            else
            {
                error.Text = "No capstone selected";
                error.BackColor = Color.DarkSalmon;
                error.Visible = true;
            }
        }
    }
}
