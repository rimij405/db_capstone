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
    // Form to view status history of a capstone and display the description of the selected status
    // Author: Jake Toporoff
    public partial class StatusHistory : Form
    {
        FormHandler fh = FormHandler.Instance;

        public StatusHistory()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.StatusHistory_FormClosed);
            this.statusValues.SelectedIndexChanged += Status_IndexChanged;

            LoadValues();
        }

        // Change dislpayed status description based on currently selected status
        private void Status_IndexChanged(object sender, EventArgs e)
        {
            statusDescriptions.Text = fh.GetBusinessStatusHistory().StatusHistoryGetDescription(statusValues.Items[statusValues.SelectedIndex].ToString());
        }

        // Load any information that needs to be displayed in the form
        private void LoadValues()
        {
            List<string> statuses = fh.GetBusinessStatusHistory().StatusHistoryGetStatuses();
            List<string> statusDates = fh.GetBusinessStatusHistory().StatusHistoryGetStatusDates();

            for(int i = 0; i < statuses.Count; i++)
            {
                statusValues.Items.Add(statuses[i]);
                statusDateValues.Items.Add(statusDates[i]);
            }
        }

        // Closes entire application when the x button is pressed
        private void StatusHistory_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // Return to capstone view page on click
        private void returnButton_Click(object sender, EventArgs e)
        {
            if (fh.GetCapstonePageView() == null) // in case page has already been created
            {
                fh.CreateCapstonePageView();
            }
            fh.GetCapstonePageView().Show();
            fh.GetStatusHistory().Hide();
        }
    }
}
