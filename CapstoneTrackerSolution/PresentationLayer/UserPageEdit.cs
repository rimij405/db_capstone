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
        List<string> emailList = new List<string>();
        List<string> phoneList = new List<string>();

        public UserPageEdit()
        {
            InitializeComponent();
            emailList.Add("example@rit.edu");
            emailList.Add("anotherexample@gmail.com");
            emailList.Add("onemoreexample@hotmail.com");
            phoneList.Add("555-555-5555");
            phoneList.Add("000-000-0000");
            phoneList.Add("123-456-7890");
            for(int i = 0; i < emailList.Count; i++)
                emails.Items.Add(emailList[i]);
            for (int i = 0; i < phoneList.Count; i++)
                phones.Items.Add(phoneList[i]);
        }

        private void save_Click(object sender, EventArgs e)
        {
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
            if(emailAddText.Text != "")
            {
                if (emailAddText.Text.Contains("@"))
                {
                    emails.Items.Add(emailAddText.Text);
                    error.Visible = false;
                    emailAddText.Text = "";
                }
                else
                {
                    error.Text = "Invalid email format";
                    error.Visible = true;
                }
            }
            else
            {
                error.Text = "No email entered";
                error.Visible = true;
            }
        }

        private void phoneAddButton_Click(object sender, EventArgs e)
        {
            if (phoneAddText.Text != "")
            {
                phones.Items.Add(phoneAddText.Text);
                error.Visible = false;
                phoneAddText.Text = "";
            }
            else
            {
                error.Text = "No phone number entered";
                error.Visible = true;
            }
        }
    }
}
