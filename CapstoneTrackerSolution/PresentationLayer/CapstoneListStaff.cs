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
    // Form that displays and orders capstones selected by a staff member
    // Author: Jake Toporoff
    public partial class CapstoneListStaff : Form
    {
        FormHandler fh = FormHandler.Instance;

        // Initialize any events not created in the form and load in information
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

        // Load any information that needs to be displayed in the form
        private void LoadValues()
        {
            orderValues.SelectedIndex = 0;
            selectValues.SelectedIndex = 1;

            List<string> capstones = fh.GetBusinessCapstoneListStaff().CapstoneLSGetCapstones(0,1);
            List<string> statuses = fh.GetBusinessCapstoneListStaff().CapstoneLSGetStatuses(0,1);

            for(int i = 0; i < capstones.Count; i++)
            {
                capstoneList.Items.Clear();
                statusList.Items.Clear();
                capstoneList.Items.Add(capstones[i]);
                statusList.Items.Add(statuses[i]);
            }
        }

        // Closes entire application when the x button is pressed
        private void CapstoneListStaff_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Go to capstone view page when user double clicks a capstone in the list
        private void CapstoneList_DoubleClick(object sender, EventArgs e)
        {
            if (capstoneList.SelectedItem != null)
            {
                fh.SetSelectedCapstone(capstoneList.SelectedValue.ToString(), 0);
                if (fh.GetCapstonePageView() == null) // in case page has already been created
                {
                    fh.CreateCapstonePageView();
                }
                fh.GetCapstonePageView().Show();
                fh.GetCapstoneListStaff().Hide();
            }
        }

        // Deselects currently selected capstone when user clicks elsewhere on the form
        private void CapstoneList_Defocus(object sender, EventArgs e)
        {
            capstoneList.SelectedItem = null;
        }

        // Reorder capstone list when value is changed
        private void OrderValues_IndexChanged(object sender, System.EventArgs e)
        {
            List<string> capstones = fh.GetBusinessCapstoneListStaff().CapstoneLSGetCapstones(
                orderValues.SelectedIndex, 
                selectValues.SelectedIndex
                );
            List<string> statuses = fh.GetBusinessCapstoneListStaff().CapstoneLSGetStatuses(
                orderValues.SelectedIndex, 
                selectValues.SelectedIndex
                );

            for (int i = 0; i < capstones.Count; i++)
            {
                capstoneList.Items.Clear();
                statusList.Items.Clear();
                capstoneList.Items.Add(capstones[i]);
                statusList.Items.Add(statuses[i]);
            }
        }

        // Update capstone selection when value is changed
        private void SelectValues_IndexChanged(object sender, System.EventArgs e)
        {
            List<string> capstones = fh.GetBusinessCapstoneListStaff().CapstoneLSGetCapstones(
                orderValues.SelectedIndex, 
                selectValues.SelectedIndex
                );
            List<string> statuses = fh.GetBusinessCapstoneListStaff().CapstoneLSGetStatuses(
                orderValues.SelectedIndex,
                selectValues.SelectedIndex
                );

            for (int i = 0; i < capstones.Count; i++)
            {
                capstoneList.Items.Clear();
                statusList.Items.Clear();
                capstoneList.Items.Add(capstones[i]);
                statusList.Items.Add(statuses[i]);
            }
        }

        // Navigate back to the user page on click
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
