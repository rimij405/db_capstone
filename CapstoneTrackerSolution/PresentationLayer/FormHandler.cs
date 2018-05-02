using BusinessLayer;
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

        // Global form variables that need to be tracked across pages
        string username;
        string selectedUser;
        string selectedCapstone;
        string userRole;

        public string GetRole()
        {
            return userRole;
        }

        public string GetUsername()
        {
            return username;
        }

        public void SetUsername(string user)
        {
            username = user;
            //TODO - Set userRole
        }

        public void SetSelectedUser(string user)
        {
            selectedUser = user;
        }

        // input type determines whether capstone is being set with a capstone name (0) or a username (1)
        public void SetSelectedCapstone(string capstone, int inputType)
        {
            if (inputType == 0)
            {
                selectedCapstone = capstone;
            }
            else
            {

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


        // Business layer classes
        BusinessLogin b_L = new BusinessLogin();
        BusinessUserPage b_UP = new BusinessUserPage();
        BusinessUserPageEdit b_UPE = new BusinessUserPageEdit();
        BusinessCapstonePageView b_CPV = new BusinessCapstonePageView();
        BusinessCapstonePageEdit b_CPE = new BusinessCapstonePageEdit();
        BusinessCapstoneListStaff b_CLS = new BusinessCapstoneListStaff();
        BusinessCapstoneListFaculty b_CLF = new BusinessCapstoneListFaculty();
        BusinessUserList b_UL = new BusinessUserList();
        BusinessStatusHistory b_SH = new BusinessStatusHistory();

        // Business layer accessors
        public BusinessLogin GetBusinessLogin() { return b_L; }
        public BusinessUserPage GetBusinessUserPage() { return b_UP; }
        public BusinessUserPageEdit GetBusinessUserPageEdit() { return b_UPE; }
        public BusinessCapstonePageView GetBusinessCapstonePageView() { return b_CPV; }
        public BusinessCapstonePageEdit GetBusinessCapstonePageEdit() { return b_CPE; }
        public BusinessCapstoneListStaff GetBusinessCapstoneListStaff() { return b_CLS; }
        public BusinessCapstoneListFaculty GetBusinessCapstoneListFaculty() { return b_CLF; }
        public BusinessUserList GetBusinessUserList() { return b_UL; }
        public BusinessStatusHistory GetBusinessStatusHistory() { return b_SH; }
    }
}
