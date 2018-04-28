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
    // Form that displays a faculty member's pending and current capstones, and allows them to accept or reject pending invitations
    // Author: Jake Toporoff
    public partial class CapstoneListFaculty : Form
    {
        FormHandler fh = FormHandler.Instance;

        // Initialize any events not created in the form and load in information
        public CapstoneListFaculty()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.CapstoneListFaculty_FormClosed);
            this.capstonePendingList.DoubleClick += CapstonePending_DoubleClick;
            this.capstoneCurrentList.DoubleClick += CapstoneCurrent_DoubleClick;

            LoadValues();
        }

        // Load any information that needs to be displayed in the form
        private void LoadValues()
        {
            List<string> pendingCapstones = fh.CapstoneLFGetPendingCapstones();
            List<string> currentCapstones = fh.CapstoneLFGetCurrentCapstones();
            List<string> grades = fh.CapstoneLFGetCurrentGrades();

            for(int i = 0; i < pendingCapstones.Count; i++)
            {
                capstonePendingList.Items.Add(pendingCapstones[i]);
            }

            for(int i = 0; i < currentCapstones.Count; i++)
            {
                capstoneCurrentList.Items.Add(currentCapstones[i]);
                capstoneGradeList.Items.Add(grades[i]);
            }
        }

        // Navigate to selected capstone from current capstones list on double click
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

        // Navigate to selected capstone from pending capstones list on double click
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

        // Closes entire application when the x button is pressed
        private void CapstoneListFaculty_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Navigate back to user page on click
        private void userPageReturn_Click(object sender, EventArgs e)
        {
            if (fh.GetUserPage() == null) // in case page has already been created
            {
                fh.CreateUserPage();
            }
            fh.GetUserPage().Show();
            fh.GetCapstoneListFaculty().Hide();
        }

        // Accept capstone invitation and move selection from pending list to current list
        private void acceptCapstone_Click(object sender, EventArgs e)
        {
            if (capstonePendingList.SelectedItem != null)
            {
                fh.CapstoneLFAcceptInvitation();

                capstoneCurrentList.Items.Add(capstonePendingList.SelectedItem);
                capstonePendingList.Items.Remove(capstonePendingList.SelectedItem);
                error.Text = "Successfully accepted capstone committee request";
                error.BackColor = Color.DarkSeaGreen;
                error.Visible = true;
            }
            else // error
            {
                error.Text = "No capstone selected";
                error.BackColor = Color.DarkSalmon;
                error.Visible = true;
            }
        }

        // Reject capstone invitation and remove selection from pending list
        private void rejectCapstone_Click(object sender, EventArgs e)
        {
            if (capstonePendingList.SelectedItem != null)
            {
                fh.CapstoneLFRejectInvitation();

                capstonePendingList.Items.Remove(capstonePendingList.SelectedItem);
                error.Text = "Successfully declined capstone committee request";
                error.BackColor = Color.DarkSeaGreen;
                error.Visible = true;
            }
            else // error
            {
                error.Text = "No capstone selected";
                error.BackColor = Color.DarkSalmon;
                error.Visible = true;
            }
        }
    }
}
