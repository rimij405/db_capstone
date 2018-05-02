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
    // Form to edit and update a user's capstone information. All user types can edit different parts of the form within their limitations.
    // Author: Jake Toporoff
    public partial class CapstonePageEdit : Form
    {
        FormHandler fh = FormHandler.Instance;
        bool isStaff = false;
        bool isFaculty = false; // to determine user type

        // Initialize any events not created in the form and load in information
        public CapstonePageEdit()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.CapstonePageEdit_FormClosed);
            this.facultyRequest.GotFocus += this.facultyRequest_OnFocus;
            this.facultyRequest.LostFocus += this.facultyRequest_Defocus;

            LoadValues();
        }

        // Load any information that needs to be displayed in the form
        private void LoadValues()
        {
            string role = fh.GetRole();
            if (role == "staff") isStaff = true;
            if (role == "faculty") isFaculty = true;


            // load page differently based on user type
            if(isFaculty || isStaff)
            {
                titleValue.ReadOnly = true;
                abstractValue.ReadOnly = true;
                facultyRequest.Visible = false;
                facultyRequestConfirm.Visible = false;
                deleteFaculty.Visible = false;

                if (isFaculty)
                {
                    defenseDateValue.Enabled = false;
                    plagarismScoreValue.ReadOnly = true;
                    gradeValue.ReadOnly = false;
                }
                else // if staff
                {
                    gradeValue.ReadOnly = true;
                    plagarismScoreValue.ReadOnly = false;
                    statuses.Visible = true;
                }
            }

            titleValue.Text = fh.GetBusinessCapstonePageEdit().CapstonePEGetTitle();
            abstractValue.Text = fh.GetBusinessCapstonePageEdit().CapstonePEGetAbstract();
            List<string> faculty = fh.GetBusinessCapstonePageEdit().CapstonePEGetFaculty();
            for (int i = 0; i < faculty.Count; i++)
            {
                facultyValue.Items.Add(faculty[i]);
            }
            defenseDateValue.Value = fh.GetBusinessCapstonePageEdit().CapstonePEGetDefenseDate();
            statuses.SelectedText = fh.GetBusinessCapstonePageEdit().CapstonePEGetStatus();
            gradeValue.Text = fh.GetBusinessCapstonePageEdit().CapstonePEGetGrade();
            plagarismScoreValue.Text = fh.GetBusinessCapstonePageEdit().CapstonePEGetPlagarismScore();
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

        // Closes entire application when the x button is pressed
        private void CapstonePageEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Add faculty member to capstone
        private void facultyRequestConfirm_Click(object sender, EventArgs e)
        {
            if(facultyRequest.Text != "" && facultyRequest.Text != "Add Faculty Here") // if something is entered
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

        // Save any changes to the page the user has made and navigate back to the capstone's viewing page
        private void save_Click(object sender, EventArgs e)
        {
            List<string> faculty = new List<string>();
            for(int i = 0; i < facultyValue.Items.Count; i++)
            {
                faculty.Add(facultyValue.Items[i].ToString());
            }
            fh.GetBusinessCapstonePageEdit().CapstonePESaveChanges(
                titleValue.Text,
                abstractValue.Text,
                faculty,
                defenseDateValue.ToString(),
                statuses.SelectedItem.ToString(),
                gradeValue.Text,
                plagarismScoreValue.Text
                );

            if (fh.GetCapstonePageView() == null) // in case page has already been created
            {
                fh.CreateCapstonePageView();
            }
            fh.GetCapstonePageView().Show();
            fh.GetCapstonePageEdit().Hide();
        }

        // Discard any user made changes and navigate back to the capstone's viewing page
        private void cancel_Click(object sender, EventArgs e)
        {
            if (fh.GetCapstonePageView() == null) // in case page has already been created
            {
                fh.CreateCapstonePageView();
            }
            fh.GetCapstonePageView().Show();
            fh.GetCapstonePageEdit().Hide();
        }

        // Remove faculty member from capstone
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
