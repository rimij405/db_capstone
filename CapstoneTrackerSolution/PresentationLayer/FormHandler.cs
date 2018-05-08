using BusinessLayer;
using ISTE.DAL.Database.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ISTE.BAL.Implementations;
using Services;

namespace PresentationLayer
{
    /*Form handler singleton to manage form navigation, information access and updating, and temporary information management 
     * (ex: current user logged in and their role)
     * Author: Jake Toporoff, Ian Effendi
     */
    public class FormHandler
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        private static FormHandler instance;
        private static MySqlDatabase database;
        private static Logger log;

        //////////////////////
        // Static method(s).

        // Classes can use this to instantiate the form handler and access methods
        public static FormHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FormHandler();
                }
                return instance;
            }
        }

        //////////////////////
        // Conversion operators.

        //////////////////////
        // Field(s).
        //////////////////////

        // Global form variables that need to be tracked across pages
        FormHandlerProperties settings;

        // Business classes.
        BusinessLogin B_Login;
                                
        //////////////////////
        // Properties.
        //////////////////////

        public FormHandlerProperties Settings { get { return (this.settings ?? (this.settings = new FormHandlerProperties())); } }
        
        public MySqlDatabase Database { get { return this.Settings.Database; } }

        public BusinessLogin BusinessLogin { get { return (B_Login ?? (B_Login = new BusinessLogin(this.Database, this.Settings.CurrentUser))); } }
        
        //////////////////////
        // Indexer(s).

        //////////////////////
        // Constructor(s).
        //////////////////////

        private FormHandler() {
            settings = new FormHandlerProperties();
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).
                
        //////////////////////
        // Helper method(s).

        //////////////////////
        // Accessor method(s).
   
        //////////////////////
        // Mutator method(s).
                
    }

    /// <summary>
    /// Collection of values to store.
    /// Author: Ian Effendi
    /// </summary>
    public class FormHandlerProperties
    {
        //////////////////////
        // Field(s).
        //////////////////////

        private MySqlDatabase db;
        private BusinessUser user;
        private Logger log;

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

        //////////////////////
        // Properties.
        //////////////////////

        public MySqlDatabase Database
        {
            get { return db ?? (db = new MySqlDatabase(new MySqlConfiguration())); }
        }

        public BusinessUser CurrentUser
        {
            get { return this.user; }
            set { this.user = value; }
        }

        public Logger Log
        {
            get { return (log ?? (log = new Logger("", "presentation", "log"))); }
        }

        public Login Login {
            get { return (login ?? (login = new Login())); }
        }

        public UserPage UserPage
        {
            get { return (userPage ?? (userPage = new UserPage())); }
        }

        public UserPageEdit UserPageEdit {
            get { return (userPageEdit ?? (userPageEdit = new UserPageEdit())); }
        }

        public CapstonePageView CapstonePageView
        {
            get { return (capstonePageView ?? (capstonePageView = new CapstonePageView())); }
        }

        public CapstonePageEdit CapstonePageEdit
        {
            get { return (capstonePageEdit ?? (capstonePageEdit = new CapstonePageEdit())); }
        }

        public CapstoneListStaff CapstoneListStaff
        {
            get { return (capstoneListStaff ?? (capstoneListStaff = new CapstoneListStaff())); }
        }

        public CapstoneListFaculty CapstoneListFaculty
        {
            get { return (capstoneListFaculty ?? (capstoneListFaculty = new CapstoneListFaculty())); }
        }

        public UserList UserList
        {
            get { return (userList ?? (userList = new UserList())); }
        }

        public StatusHistory StatusHistory
        {
            get { return (statusHistory ?? (statusHistory = new StatusHistory())); }
        }

        //////////////////////
        // Indexer(s).

        //////////////////////
        // Constructor(s).
        //////////////////////

        public FormHandlerProperties()
        {

        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        //////////////////////
        // Helper method(s).

        //////////////////////
        // Accessor method(s).

        //////////////////////
        // Mutator method(s).

        public void CreateUser(string username, string password)
        {
            this.user = new BusinessUser(this.Database, username, password);
        }


    }




}
