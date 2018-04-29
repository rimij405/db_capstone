using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer
{
    // Business layer class that handles taking data from the database and converting it to a usable format for the
    // presentation layer and vice versa for the user page edit form
    // Author: Jake Toporoff
    public class BusinessUserPageEdit
    {
        string fName;
        string lName;
        string role;
        List<string> emails;
        List<string> phones;
        List<int> emailTypes;
        List<int> phoneTypes;

        // UserPageEdit Get Functions
        public string UPEGetFirstName()
        {
            return "Firstname";
        }

        public string UPEGetLastName()
        {
            return "Lastname";
        }

        public string UPEGetUserRole()
        {
            return "Role";
        }

        public List<string> UPEGetEmails()
        {
            List<string> emails = new List<string>();
            emails.Add("Test email 1");
            emails.Add("Test email 2");
            return emails;
        }

        public List<string> UPEGetPhones()
        {
            List<string> phones = new List<string>();
            phones.Add("Test phone 1");
            phones.Add("Test phone 2");
            return phones;
        }

        public List<int> UPEGetEmailTypes()
        {
            List<int> emailTypes = new List<int>();
            emailTypes.Add(1);
            emailTypes.Add(2);
            return emailTypes;
        }

        public List<int> UPEGetPhoneTypes()
        {
            List<int> phoneTypes = new List<int>();
            phoneTypes.Add(1);
            phoneTypes.Add(2);
            return phoneTypes;
        }
        // End UserPageEdit Get Functions

        // UserPageEdit Set Functions
        public void UPESaveChanges(List<string> emails, List<string> phones, List<int> emailTypes, List<int> phoneTypes)
        {
            SetEmails(emails);
            SetPhones(phones);
            SetEmailTypes(emailTypes);
            SetPhoneTypes(phoneTypes);
        }

        private void SetEmails(List<string> emails)
        {
            return;
        }

        private void SetPhones(List<string> phones)
        {
            return;
        }

        private void SetEmailTypes(List<int> emailTypes)
        {
            return;
        }

        private void SetPhoneTypes(List<int> phoneTypes)
        {
            return;
        }
        // End UserPageEdit Set Functions
    }
}
