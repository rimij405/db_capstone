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
    public partial class CapstoneListStaff : Form
    {
        FormHandler fh = FormHandler.Instance;

        public CapstoneListStaff()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.CapstoneListStaff_FormClosed);
            this.capstoneList.DoubleClick += CapstoneList_DoubleClick;
            this.capstoneList.LostFocus += CapstoneList_Defocus;
            this.Click += CapstoneList_Defocus;
            this.orderValues.SelectedIndexChanged += OrderValues_IndexChanged;
            this.selectValues.SelectedIndexChanged += SelectValues_IndexChanged;

            LoadValues();
        }

        private void LoadValues()
        {
            orderValues.SelectedIndex = 0;
            //test data
            capstoneList.Items.Add("My capstone");
            statusList.Items.Add("Ongoing");
        }

        private void CapstoneListStaff_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // go to capstone view page when user double clicks a capstone in the list
        private void CapstoneList_DoubleClick(object sender, EventArgs e)
        {
            if (capstoneList.SelectedItem != null)
            {
                if (fh.GetCapstonePageView() == null) // in case page has already been created
                {
                    fh.CreateCapstonePageView();
                }
                fh.GetCapstonePageView().Show();
                fh.GetCapstoneListStaff().Hide();
            }
        }

        // deselects currently selected capstone when user clicks elsewhere on the form
        private void CapstoneList_Defocus(object sender, EventArgs e)
        {
            capstoneList.SelectedItem = null;
        }

        private void OrderValues_IndexChanged(object sender, System.EventArgs e)
        {

        }

        private void SelectValues_IndexChanged(object sender, System.EventArgs e)
        {

        }

        private void userPageReturn_Click(object sender, EventArgs e)
        {
            if (fh.GetUserPage() == null) // in case page has already been created
            {
                fh.CreateUserPage();
            }
            fh.GetUserPage().Show();
            fh.GetCapstoneListStaff().Hide();
        }
    }
}
