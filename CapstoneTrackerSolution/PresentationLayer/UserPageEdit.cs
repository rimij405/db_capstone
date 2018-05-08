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
    // Form where a user can edit and update their emails and phone numbers
    // Author: Jake Toporoff
    public partial class UserPageEdit : Form
    {
        FormHandler fh = FormHandler.Instance;

        // Used to track temporary phone and email type changes
        List<int> emailTypeList;
        List<int> phoneTypeList;

        // Initialize any events not created in the form and load in information
        public UserPageEdit()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(this.UserPageEdit_FormClosed);
            this.emailAddText.KeyUp += EmailAddText_Enter;
            this.phoneAddText.KeyUp += PhoneAddText_Enter;
            this.emails.Click += Emails_SelectType;
            this.phones.Click += Phones_SelectType;
            this.emailTypes.SelectedIndexChanged += EmailTypes_IndexChanged;
            this.phoneTypes.SelectedIndexChanged += PhoneTypes_IndexChanged;

            LoadValues();
        }

        // Load any information that needs to be displayed in the form
        private void LoadValues()
        {
            /* firstName.Text = fh.GetBusinessUserPageEdit().UPEGetFirstName();
            lastName.Text = fh.GetBusinessUserPageEdit().UPEGetLastName();
            userRole.Text = fh.GetBusinessUserPageEdit().UPEGetUserRole();
            emailTypeList = fh.GetBusinessUserPageEdit().UPEGetEmailTypes();
            phoneTypeList = fh.GetBusinessUserPageEdit().UPEGetPhoneTypes();
            List<string> emailList = fh.GetBusinessUserPageEdit().UPEGetEmails();
            for (int i = 0; i < emailList.Count; i++)
            {
                emails.Items.Add(emailList[i]);
            }
            List<string> phoneList = fh.GetBusinessUserPageEdit().UPEGetPhones();
            for (int i = 0; i < phoneList.Count; i++)
            {
                phones.Items.Add(phoneList[i]);
            }*/
        }

        // Closes entire application when the x button is pressed
        private void UserPageEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        // If the email type has been changed, update the currently selected email
        private void Emails_SelectType(object sender, EventArgs e)
        {
            if(emails.SelectedItem != null)
            {
                emailTypes.SelectedIndex = emailTypeList[emails.SelectedIndex];
            }
        }

        // If the phone type has been changed, update the currently selected phone 
        private void Phones_SelectType(object sender, EventArgs e)
        {
            if(phones.SelectedItem != null)
            {
                phoneTypes.SelectedIndex = phoneTypeList[phones.SelectedIndex];
            }
        }

        // If the selected email changes, have the current email type match the selection
        private void EmailTypes_IndexChanged(object sender, EventArgs e)
        {
            if(emails.SelectedItem != null)
            {
                emailTypeList[emails.SelectedIndex] = emailTypes.SelectedIndex;
            }
        }

        // If the selected phone changes, have the current phone type match the selection
        private void PhoneTypes_IndexChanged(object sender, EventArgs e)
        {
            if(phones.SelectedItem != null)
            {
                phoneTypeList[phones.SelectedIndex] = phoneTypes.SelectedIndex;
            }
        }

        // Save any changes the user has made and navigate back to the user page view form
        private void save_Click(object sender, EventArgs e)
        {
            List<string> emailList = new List<string>();
            List<string> phoneList = new List<string>();

            for(int i = 0; i < emails.Items.Count; i++)
            {
                emailList.Add(emails.Items[i].ToString());
            }

            for(int i = 0; i < phones.Items.Count; i++)
            {
                phoneList.Add(phones.Items[i].ToString());
            }

            /*fh.GetBusinessUserPageEdit().UPESaveChanges(emailList, phoneList, emailTypeList, phoneTypeList);

            if (fh.GetUserPage() == null) // in case page has already been created
            {
                fh.CreateUserPage();
            }
            fh.GetUserPage().Show();
            fh.GetUserPageEdit().Hide();*/
        }

        // Return to user page if user doesn't want to save changes
        private void cancel_Click(object sender, EventArgs e)
        {
            /*if (fh.GetUserPage() == null) // in case page has already been created
            {
                fh.CreateUserPage();
            }
            fh.GetUserPage().Show();
            fh.GetUserPageEdit().Hide();*/
        }

        // Add email
        private void emailAddButton_Click(object sender, EventArgs e)
        {
            HandleAddEmail();
        }

        // Add phone number
        private void phoneAddButton_Click(object sender, EventArgs e)
        {
            HandleAddPhone();
        }

        // Remove email from userpage
        private void deleteEmail_Click(object sender, EventArgs e)
        {
            if (emails.SelectedItem != null)
            {
                emailTypeList.RemoveAt(emails.SelectedIndex);
                emails.Items.Remove(emails.SelectedItem);
                error.Text = "Successfully removed email";
                error.BackColor = Color.DarkSeaGreen;
                error.Visible = true;
            }
            else // error
            {
                error.Text = "No email selected";
                error.BackColor = Color.DarkSalmon;
                error.Visible = true;
            }
        }

        // Remove phone from userpage
        private void deletePhone_Click(object sender, EventArgs e)
        {
            if (phones.SelectedItem != null)
            {
                phoneTypeList.RemoveAt(phones.SelectedIndex);
                phones.Items.Remove(phones.SelectedItem);
                error.Text = "Successfully removed phone number";
                error.BackColor = Color.DarkSeaGreen;
                error.Visible = true;
            }
            else // error
            {
                error.Text = "No phone selected";
                error.BackColor = Color.DarkSalmon;
                error.Visible = true;
            }
        }

        // Accept user input if they hit the enter key while focused in the email input text box
        private void EmailAddText_Enter(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                HandleAddEmail();
        }

        // Accept user input if they hit the enter key while focused in the phone input text box
        private void PhoneAddText_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                HandleAddPhone();
        }

        // Check validity of user input in the email input text box
        private void HandleAddEmail()
        {
            if (emailAddText.Text != "")
            {
                if (emailAddText.Text.Contains("@")) // make sure it's an actual email address
                {
                    emails.Items.Add(emailAddText.Text);
                    emailTypeList.Add(0);
                    emailAddText.Text = "";
                    error.Text = "Successfully added email";
                    error.BackColor = Color.DarkSeaGreen;
                    error.Visible = true;
                }
                else // error = not an email
                {
                    error.Text = "Invalid email format";
                    error.BackColor = Color.DarkSalmon;
                    error.Visible = true;
                }
            }
            else // error - nothing entered
            {
                error.Text = "No email entered";
                error.BackColor = Color.DarkSalmon;
                error.Visible = true;
            }
        }

        // Check validity of user input in the phone input text box
        private void HandleAddPhone()
        {
            if (phoneAddText.Text != "")
            {
                phones.Items.Add(phoneAddText.Text);
                phoneTypeList.Add(0);
                phoneAddText.Text = "";
                error.Text = "Successfully added phone";
                error.BackColor = Color.DarkSeaGreen;
                error.Visible = true;
            }
            else // error - nothing entered
            {
                error.Text = "No phone number entered";
                error.BackColor = Color.DarkSalmon;
                error.Visible = true;
            }
        }
    }
}
