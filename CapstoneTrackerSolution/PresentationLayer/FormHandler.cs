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

        public void CreateLogin()  { login = new Login(); }
        public void CreateUserPage() {
            userPage = new UserPage();
        }
        public void CreateUserPageEdit() { userPageEdit = new UserPageEdit(); }
        public void CreateCapstonePageView() { capstonePageView = new CapstonePageView(); }
        public void CreateCapstonePageEdit() { capstonePageEdit = new CapstonePageEdit(); }
        public void CreateCapstoneListStaff() { capstoneListStaff = new CapstoneListStaff(); }
        public void CreateCapstoneListFaculty() { capstoneListFaculty = new CapstoneListFaculty(); }

        public Login GetLogin() { return login; }
        public UserPage GetUserPage() { return userPage; }
        public UserPageEdit GetUserPageEdit() { return userPageEdit; }
        public CapstonePageEdit GetCapstonePageEdit() { return capstonePageEdit; }
        public CapstonePageView GetCapstonePageView() { return capstonePageView; }
        public CapstoneListStaff GetCapstoneListStaff() { return capstoneListStaff; }
        public CapstoneListFaculty GetCapstoneListFaculty() { return capstoneListFaculty; }
    }
}
