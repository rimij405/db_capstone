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
    public partial class UserList : Form
    {
        FormHandler fh = FormHandler.Instance;

        public UserList()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.UserList_FormClosed);
            this.orderValues.SelectedIndexChanged += OrderValues_IndexChanged;
            this.selectValues.SelectedIndexChanged += SelectValues_IndexChanged;
            this.usersList.DoubleClick += UsersList_DoubleClick;
            this.Click += UserList_Defocus;
            this.usersList.LostFocus += UserList_Defocus;

            LoadValues();
        }

        private void LoadValues()
        {
            usersList.Items.Add("Student McStudentface");
        }

        private void UserList_FormClosed(object sender, FormClosedEventArgs e)
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
            fh.GetUserList().Hide();
        }

        private void UserList_Defocus(object sender, EventArgs e)
        {
            usersList.SelectedItem = null;
        }

        private void OrderValues_IndexChanged(object sender, EventArgs e)
        {

        }

        private void SelectValues_IndexChanged(object sender, EventArgs e)
        {

        }

        private void UsersList_DoubleClick(object sender, EventArgs e)
        {
            if (usersList.SelectedItem != null)
            {
                if (fh.GetUserPage() == null) // in case page has already been created
                {
                    fh.CreateUserPage();
                }
                fh.GetUserPage().Show();
                fh.GetUserList().Hide();
            }
        }
    }
}
