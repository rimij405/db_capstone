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
    // Form which shows a student's capstone information in a read only format
    // Author: Jake Toporoff
    public partial class CapstonePageView : Form
    {
        FormHandler fh = FormHandler.Instance;

        // Initialize any events not created in the form and load in information
        public CapstonePageView()
        {

            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.CapstonePageView_FormClosed);
            this.Click += FacultyValue_Defocus;
            this.facultyValue.DoubleClick += FacultyValue_DoubleClick;
            this.facultyValue.LostFocus += FacultyValue_Defocus;

            LoadValues();
        }

        // Load any information that needs to be displayed in the form
        private void LoadValues()
        {
            titleValue.Text = fh.CapstonePVGetTitle();
            abstractValue.Text = fh.CapstonePVGetAbstract();
            List<string> faculty = fh.CapstonePVGetFaculty();
            for(int i = 0; i < faculty.Count; i++)
            {
                facultyValue.Items.Add(faculty[i]);
            }
            defenseDateValue.Text = fh.CapstonePVGetDefenseDate();
            statusValue.Text = fh.CapstonePVGetStatus();
            grade.Text = fh.CapstonePVGetGrade();
            plagarismScore.Text = fh.CapstonePVGetPlagarismScore();
        }

        // Deselect a faculty member if the user clicks elsewhere on the form
        private void FacultyValue_Defocus(object sender, EventArgs e)
        {
            facultyValue.SelectedItem = null;
        }

        // Closes entire application when the x button is pressed
        private void CapstonePageView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Navigate to capstone edit page
        private void edit_Click(object sender, EventArgs e)
        {
            if (fh.GetCapstonePageEdit() == null) // in case page has already been created
            {
                fh.CreateCapstonePageEdit();
            }
            fh.GetCapstonePageEdit().Show();
            fh.GetCapstonePageView().Hide();
        }

        // Go to userpage of the faculty member the user double clicks (the selected item)
        private void FacultyValue_DoubleClick(object sender, EventArgs e)
        {
            if(facultyValue.SelectedItem != null)
            {
                if (fh.GetUserPage() == null) // in case page has already been created
                {
                    fh.CreateUserPage();
                }
                fh.GetUserPage().Show();
                fh.GetCapstonePageView().Hide();
            }
        }

        // Navigate back to user page
        private void userPageReturn_Click(object sender, EventArgs e)
        {
            if (fh.GetUserPage() == null) // in case page has already been created
            {
                fh.CreateUserPage();
            }
            fh.GetUserPage().Show();
            fh.GetCapstonePageView().Hide();
        }

        // Navigate to capstone's status history form on click
        private void statusHistoryButton_Click(object sender, EventArgs e)
        {
            if (fh.GetStatusHistory() == null) // in case page has already been created
            {
                fh.CreateStatusHistory();
            }
            fh.GetStatusHistory().Show();
            fh.GetCapstonePageView().Hide();
        }
    }
}
