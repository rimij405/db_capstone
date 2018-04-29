using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer
{
    // Business layer class that handles taking data from the database and converting it to a usable format for the
    // presentation layer and vice versa for the user list form
    // Author: Jake Toporoff
    public class BusinessUserList
    {
        List<string> users;

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
    }
}
