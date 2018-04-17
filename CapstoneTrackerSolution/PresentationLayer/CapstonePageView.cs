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
    public partial class CapstonePageView : Form
    {
        FormHandler fh = FormHandler.Instance;

        public CapstonePageView()
        {

            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.CapstonePageView_FormClosed);
            this.Click += FacultyValue_Defocus;
            this.facultyValue.DoubleClick += FacultyValue_DoubleClick;
            this.facultyValue.LostFocus += FacultyValue_Defocus;

            LoadValues();
        }

        private void LoadValues()
        {

        }

        // deselect a faculty member if the user clicks elsewhere on the form
        private void FacultyValue_Defocus(object sender, EventArgs e)
        {
            facultyValue.SelectedItem = null;
        }

        private void CapstonePageView_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void edit_Click(object sender, EventArgs e)
        {
            if (fh.GetCapstonePageEdit() == null) // in case page has already been created
            {
                fh.CreateCapstonePageEdit();
            }
            fh.GetCapstonePageEdit().Show();
            fh.GetCapstonePageView().Hide();
        }

        // go to userpage of the faculty member the user double clicks (the selected item)
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

        private void userPageReturn_Click(object sender, EventArgs e)
        {
            if (fh.GetUserPage() == null) // in case page has already been created
            {
                fh.CreateUserPage();
            }
            fh.GetUserPage().Show();
            fh.GetCapstonePageView().Hide();
        }
    }
}
