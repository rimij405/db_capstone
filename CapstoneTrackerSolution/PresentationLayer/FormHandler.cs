using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public class FormHandler
    {
        private static FormHandler instance;

        private FormHandler() { }

        public static FormHandler Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new FormHandler();
                }
                return instance;
            }
        }

        Login login = null;
        UserPage userPage = null;
        UserPageEdit userPageEdit = null;
        CapstonePageView capstonePageView = null;
        CapstonePageEdit capstonePageEdit = null;
        CapstoneListStaff capstoneListStaff = null;
        CapstoneListFaculty capstoneListFaculty = null;
        UserList userList = null;

        public void CreateLogin()  { login = new Login(); }
        public void CreateUserPage() { userPage = new UserPage(); }
        public void CreateUserPageEdit() { userPageEdit = new UserPageEdit(); }
        public void CreateCapstonePageView() { capstonePageView = new CapstonePageView(); }
        public void CreateCapstonePageEdit() { capstonePageEdit = new CapstonePageEdit(); }
        public void CreateCapstoneListStaff() { capstoneListStaff = new CapstoneListStaff(); }
        public void CreateCapstoneListFaculty() { capstoneListFaculty = new CapstoneListFaculty(); }
        public void CreateUserList() { userList = new UserList(); }

        public Login GetLogin() { return login; }
        public UserPage GetUserPage() { return userPage; }
        public UserPageEdit GetUserPageEdit() { return userPageEdit; }
        public CapstonePageEdit GetCapstonePageEdit() { return capstonePageEdit; }
        public CapstonePageView GetCapstonePageView() { return capstonePageView; }
        public CapstoneListStaff GetCapstoneListStaff() { return capstoneListStaff; }
        public CapstoneListFaculty GetCapstoneListFaculty() { return capstoneListFaculty; }
        public UserList GetUserList() { return userList; }

        // Login Set Functions
        public bool LoginSetUserName()
        {
            return true;
        }

        public bool LoginSetPassword()
        {
            return true;
        }
        // End Login Set Functions

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

        // UserPageEdit Get Functions - same as UserPage Get Functions (for now)
        public string UPEGetFirstName()
        {
            return UPGetFirstName();
        }

        public string UPEGetLastName()
        {
            return UPGetLastName();
        }

        public string UPEGetUserRole()
        {
            return UPGetUserRole();
        }

        public List<string> UPEGetEmails()
        {
            return UPGetEmails();
        }

        public List<string> UPEGetPhones()
        {
            return UPGetPhones();
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
        public void UPESaveChanges()
        {
            return;
        }
        // End UserPageEdit Set Functions
    }
}
