using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    /*Form handler singleton to manage form navigation, information access and updating, and temporary information management 
     * (ex: current user logged in and their role)
     * Author: Jake Toporoff
     */
    public class FormHandler
    {
        private static FormHandler instance;

        private FormHandler() { }

        // Classes can use this to instantiate the form handler and access methods
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

        // All forms start out as null since they may not be used every session
        Login login = null;
        UserPage userPage = null;
        UserPageEdit userPageEdit = null;
        CapstonePageView capstonePageView = null;
        CapstonePageEdit capstonePageEdit = null;
        CapstoneListStaff capstoneListStaff = null;
        CapstoneListFaculty capstoneListFaculty = null;
        UserList userList = null;
        StatusHistory statusHistory = null;

        // Constructors to initialize forms on use
        public void CreateLogin()  { login = new Login(); }
        public void CreateUserPage() { userPage = new UserPage(); }
        public void CreateUserPageEdit() { userPageEdit = new UserPageEdit(); }
        public void CreateCapstonePageView() { capstonePageView = new CapstonePageView(); }
        public void CreateCapstonePageEdit() { capstonePageEdit = new CapstonePageEdit(); }
        public void CreateCapstoneListStaff() { capstoneListStaff = new CapstoneListStaff(); }
        public void CreateCapstoneListFaculty() { capstoneListFaculty = new CapstoneListFaculty(); }
        public void CreateUserList() { userList = new UserList(); }
        public void CreateStatusHistory() { statusHistory = new StatusHistory(); }

        // Form Accessors
        public Login GetLogin() { return login; }
        public UserPage GetUserPage() { return userPage; }
        public UserPageEdit GetUserPageEdit() { return userPageEdit; }
        public CapstonePageEdit GetCapstonePageEdit() { return capstonePageEdit; }
        public CapstonePageView GetCapstonePageView() { return capstonePageView; }
        public CapstoneListStaff GetCapstoneListStaff() { return capstoneListStaff; }
        public CapstoneListFaculty GetCapstoneListFaculty() { return capstoneListFaculty; }
        public UserList GetUserList() { return userList; }
        public StatusHistory GetStatusHistory() { return statusHistory; }

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

        // CapstonePageView Get Functions
        public string CapstonePVGetTitle()
        {
            return "capstone title";
        }

        public string CapstonePVGetAbstract()
        {
            return "capstone description";
        }

        public List<string> CapstonePVGetFaculty()
        {
            List<string> faculty = new List<string>();
            faculty.Add("John Smith");
            faculty.Add("Firstname Lastname");
            return faculty;
        }

        public string CapstonePVGetDefenseDate()
        {
            return "DateTime";
        }

        public string CapstonePVGetStatus()
        {
            return "current status";
        }

        public string CapstonePVGetGrade()
        {
            return "Grade";
        }

        public string CapstonePVGetPlagarismScore()
        {
            return "Plagarism Score";
        }
        // End CapstonePageView Get Functions

        // CapstonePageEdit Get Functions
        public string CapstonePEGetTitle()
        {
            return CapstonePVGetTitle();
        }

        public string CapstonePEGetAbstract()
        {
            return CapstonePVGetAbstract();
        }

        public List<string> CapstonePEGetFaculty()
        {
            return CapstonePVGetFaculty();
        }

        public string CapstonePEGetStatus()
        {
            return CapstonePVGetStatus();
        }

        public string CapstonePEGetGrade()
        {
            return CapstonePVGetGrade();
        }

        public string CapstonePEGetPlagarismScore()
        {
            return CapstonePVGetPlagarismScore();
        }

        public DateTime CapstonePEGetDefenseDate()
        {
            return DateTime.Parse(CapstonePVGetDefenseDate());
        }
        // End CapstonePageEdit Get Functions

        // CapstonePageEdit Set Functions
        public void CapstonePESaveChanges()
        {
            return;
        }
        // End CapstonePageEdit Set Functions

        // CapstoneListStaff GetFunctions
        public List<string> CapstoneLSGetCapstones(int format, int selection) // format depends on the combo box selections within the form
        {
            List<string> capstones = new List<string>();
            capstones.Add("The history of something important");
            capstones.Add("A study of paint drying");
            return capstones;
        }

        public List<string> CapstoneLSGetStatuses(int format, int selection)
        {
            List<string> statuses = new List<string>();
            statuses.Add("Rejected");
            statuses.Add("Graded");
            return statuses;
        }
        // End CapstoneListStaff Get Functions

        // CapstoneListFaculty Get Functions
        public List<string> CapstoneLFGetPendingCapstones()
        {
            List<string> capstones = new List<string>();
            capstones.Add("The history of something important");
            capstones.Add("A study of paint drying");
            return capstones;
        }

        public List<string> CapstoneLFGetCurrentCapstones()
        {
            List<string> capstones = new List<string>();
            capstones.Add("The history of something important");
            capstones.Add("A study of paint drying");
            return capstones;
        }

        public List<string> CapstoneLFGetCurrentGrades()
        {
            List<string> grades = new List<string>();
            grades.Add("F--");
            grades.Add("A");
            return grades;
        }
        // End CapstoneListFaculty Get Functions

        // CapstoneListFaculty Set Functions
        public void CapstoneLFAcceptInvitation()
        {
            return;
        }

        public void CapstoneLFRejectInvitation()
        {
            return;
        }
        // End CapstoneListFaculty Set Functions

        // UserList Get Functions
        public List<string> UserListGetUsers(int format, int selection)
        {
            List<string> users = new List<string>();
            users.Add("John Smith");
            users.Add("Firstname Lastname");
            return users;
        }
        // End UserList Get Functions

        // UserList Set Functions
        public void UserListAddUser(string fName, string lName, string role)
        {
            return;
        }
        // End UserList Set Functions

        // StatusHistory Get Functions
        public List<string> StatusHistoryGetStatuses()
        {
            List<string> statuses = new List<string>();
            statuses.Add("Pending");
            statuses.Add("Completed");
            return statuses;
        }

        public List<string> StatusHistoryGetStatusDates()
        {
            List<string> dates = new List<string>();
            dates.Add("01/01/2001");
            dates.Add("01/01/2002");
            return dates;
        }

        public string StatusHistoryGetDescription(string selected)
        {
            return selected;
        }
        // End StatusHistory Get Functions
    }
}
