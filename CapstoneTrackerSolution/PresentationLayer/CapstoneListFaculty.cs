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

            LoadValues();
        }

        private void LoadValues()
        {

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
    }
}
