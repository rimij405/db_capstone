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
    // Form that displays and orders user based on a staff member's selection. New users can also be added.
    // Author: Jake Toporoff
    public partial class UserList : Form
    {
        FormHandler fh = FormHandler.Instance;

        // Initialize any events not created in the form and load in information
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

        // Load any information that needs to be displayed in the form
        private void LoadValues()
        {
            List<string> users = fh.UserListGetUsers(0, 0);
            for(int i = 0; i < users.Count; i++)
            {
                usersList.Items.Add(users[i]);
            }
        }

        // Closes entire application when the x button is pressed
        private void UserList_FormClosed(object sender, FormClosedEventArgs e)
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
            fh.GetUserList().Hide();
        }

        // Deselect user when mouse clicked elsewhere in the form
        private void UserList_Defocus(object sender, EventArgs e)
        {
            usersList.SelectedItem = null;
        }

        // Update user order when value is changed
        private void OrderValues_IndexChanged(object sender, EventArgs e)
        {
            List<string> users = fh.UserListGetUsers(orderValues.SelectedIndex, selectValues.SelectedIndex);
            for (int i = 0; i < users.Count; i++)
            {
                usersList.Items.Clear();
                usersList.Items.Add(users[i]);
            }
        }

        // Update user selection when value is changed
        private void SelectValues_IndexChanged(object sender, EventArgs e)
        {
            List<string> users = fh.UserListGetUsers(orderValues.SelectedIndex, selectValues.SelectedIndex);
            for (int i = 0; i < users.Count; i++)
            {
                usersList.Items.Clear();
                usersList.Items.Add(users[i]);
            }
        }

        // Navigate to selected user's user page on double click
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

        // Add a new user, then reload the userlist with the new user added
        private void addUserButton_Click(object sender, EventArgs e)
        {
            if (firstName.Text != "" && lastName.Text != "") // if something is entered
            {
                fh.UserListAddUser(firstName.Text, lastName.Text, roles.SelectedText);
                List<string> users = fh.UserListGetUsers(orderValues.SelectedIndex, selectValues.SelectedIndex);
                for (int i = 0; i < users.Count; i++)
                {
                    usersList.Items.Clear();
                    usersList.Items.Add(users[i]);
                }

                firstName.Text = "";
                lastName.Text = "";
                roles.SelectedIndex = 0;
                error.Text = "Successfully added user";
                error.BackColor = Color.DarkSeaGreen;
                error.Visible = true;
            }
            else // error
            {
                error.Text = "Missing user information";
                error.BackColor = Color.DarkSalmon;
                error.Visible = true;
            }
        }
    }
}
