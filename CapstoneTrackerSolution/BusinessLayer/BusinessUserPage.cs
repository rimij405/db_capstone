using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    // Business layer class that handles taking data from the database and converting it to a usable format for the
    // presentation layer and vice versa for the user page form
    // Author: Jake Toporoff
    public class BusinessUserPage
    {
        string fName;
        string lName;
        string role;
        List<string> emails;
        List<string> emailTypes;
        List<string> phones;
        List<string> phoneTypes;

        // UserPage Get Functions
        public string UPGetFirstName()
        {
            return "Firstname";
        }

        public string UPGetLastName()
        {
            return "Lastname";
        }

        public string UPGetUserRole()
        {
            return "Role";
        }

        public List<string> UPGetEmails() // Type appended to email
        {
            List<string> emails = new List<string>();
            emails.Add("Test email 1");
            emails.Add("Test email 2");
            return emails;
        }

        public List<string> UPGetPhones() // Type appended to phone
        {
            List<string> phones = new List<string>();
            phones.Add("Test phone 1");
            phones.Add("Test phone 2");
            return phones;
        }
        // End UserPage Get Functions
    }
}
