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
    public partial class UserPageEdit : Form
    {
        FormHandler fh = FormHandler.Instance;
        List<int> emailTypeList;
        List<int> phoneTypeList;

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

        private void LoadValues()
        {
            firstName.Text = fh.UPEGetFirstName();
            lastName.Text = fh.UPEGetLastName();
            userRole.Text = fh.UPEGetUserRole();
            emailTypeList = fh.UPEGetEmailTypes();
            phoneTypeList = fh.UPEGetPhoneTypes();
            List<string> emailList = fh.UPEGetEmails();
            for (int i = 0; i < emailList.Count; i++)
            {
                emails.Items.Add(emailList[i]);
            }
            List<string> phoneList = fh.UPEGetPhones();
            for (int i = 0; i < phoneList.Count; i++)
            {
                phones.Items.Add(phoneList[i]);
            }
        }

        private void UserPageEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Emails_SelectType(object sender, EventArgs e)
        {
            if(emails.SelectedItem != null)
            {
                emailTypes.SelectedIndex = emailTypeList[emails.SelectedIndex];
            }
        }

        private void Phones_SelectType(object sender, EventArgs e)
        {
            if(phones.SelectedItem != null)
            {
                phoneTypes.SelectedIndex = phoneTypeList[phones.SelectedIndex];
            }
        }

        private void EmailTypes_IndexChanged(object sender, EventArgs e)
        {
            if(emails.SelectedItem != null)
            {
                emailTypeList[emails.SelectedIndex] = emailTypes.SelectedIndex;
            }
        }

        private void PhoneTypes_IndexChanged(object sender, EventArgs e)
        {
            if(phones.SelectedItem != null)
            {
                phoneTypeList[phones.SelectedIndex] = phoneTypes.SelectedIndex;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            fh.UPESaveChanges();

            if (fh.GetUserPage() == null) // in case page has already been created
            {
                fh.CreateUserPage();
            }
            fh.GetUserPage().Show();
            fh.GetUserPageEdit().Hide();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            if (fh.GetUserPage() == null) // in case page has already been created
            {
                fh.CreateUserPage();
            }
            fh.GetUserPage().Show();
            fh.GetUserPageEdit().Hide();
        }

        private void emailAddButton_Click(object sender, EventArgs e)
        {
            HandleAddEmail();
        }

        private void phoneAddButton_Click(object sender, EventArgs e)
        {
            HandleAddPhone();
        }

        // remove email from userpage
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

        // remove phone from userpage
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

        // accept user input if they hit the enter key while focused in the email input text box
        private void EmailAddText_Enter(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                HandleAddEmail();
        }

        // accept user input if they hit the enter key while focused in the phone input text box
        private void PhoneAddText_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                HandleAddPhone();
        }

        // check validity of user input in the email input text box
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

        // check validity of user input in the phone input text box
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
