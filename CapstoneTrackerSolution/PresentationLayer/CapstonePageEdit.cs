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
    public partial class CapstonePageEdit : Form
    {
        FormHandler fh = FormHandler.Instance;
        bool isStaff, isFaculty = false; // to determine user type

        public CapstonePageEdit()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.CapstonePageEdit_FormClosed);
            this.facultyRequest.GotFocus += this.facultyRequest_OnFocus;
            this.facultyRequest.LostFocus += this.facultyRequest_Defocus;

            LoadValues();
        }

        private void LoadValues()
        {
            // load page differently based on user type
            if(isFaculty || isStaff)
            {
                titleValue.ReadOnly = true;
                abstractValue.ReadOnly = true;
                facultyRequest.Visible = false;
                facultyRequestConfirm.Visible = false;
                deleteFaculty.Visible = false;
                dragFileLabel.Visible = false;

                if (isFaculty)
                {
                    defenseDateValue.Enabled = false;
                    plagarismScoreValue.ReadOnly = true;
                }
                else // if staff
                {
                    gradeValue.ReadOnly = true;
                }
            }
        }

        // Place helpful text for user if they aren't currently entering a new faculty member
        private void facultyRequest_Defocus(object sender, EventArgs e)
        {
            if(facultyRequest.Text == "")
                facultyRequest.Text = "Add Faculty Here";
        }

        // Clear faculty add text box when user clicks on it (unless they already have typed something in)
        private void facultyRequest_OnFocus(object sender, EventArgs e)
        {
            if(facultyRequest.Text == "Add Faculty Here")
                facultyRequest.Text = "";
        }

        private void CapstonePageEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Add faculty member to capstone
        private void facultyRequestConfirm_Click(object sender, EventArgs e)
        {
            if(facultyRequest.Text != "") // if something is entered
            {
                facultyValue.Items.Add(facultyRequest.Text);
                facultyRequest.Text = "";
                feedbackText.Text = "Successfully added faculty member";
                feedbackText.BackColor = Color.DarkSeaGreen;
                feedbackText.Visible = true;
            }
            else // error
            {
                feedbackText.Text = "No faculty to add";
                feedbackText.BackColor = Color.DarkSalmon;
                feedbackText.Visible = true;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (fh.GetCapstonePageView() == null) // in case page has already been created
            {
                fh.CreateCapstonePageView();
            }
            fh.GetCapstonePageView().Show();
            fh.GetCapstonePageEdit().Hide();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            if (fh.GetCapstonePageView() == null) // in case page has already been created
            {
                fh.CreateCapstonePageView();
            }
            fh.GetCapstonePageView().Show();
            fh.GetCapstonePageEdit().Hide();
        }

        // remove faculty member from capstone
        private void deleteFaculty_Click(object sender, EventArgs e)
        {
            if(facultyValue.SelectedItem != null) // if a faculty member is selected
            {
                facultyValue.Items.Remove(facultyValue.SelectedItem);
                feedbackText.Text = "Successfully removed faculty member";
                feedbackText.BackColor = Color.DarkSeaGreen;
                feedbackText.Visible = true;
            }
            else // no faculty selected - error
            {
                feedbackText.Text = "No faculty selected";
                feedbackText.BackColor = Color.DarkSalmon;
                feedbackText.Visible = true;
            }
        }
    }
}
