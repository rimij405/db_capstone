using ISTE.DAL.Database.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.BAL.Implementations
{
    // Business layer class that handles taking data from the database and converting it to a usable format for the
    // presentation layer and vice versa for the login form
    // Author: Jake Toporoff, Ian Effendi
    public class BusinessLogin
    {

        private MySqlDatabase database;
        private BusinessUser currentUser;

        /// <summary>
        /// Create the Business Login class.
        /// </summary>
        /// <param name="currentUser">User to login with.</param>
        public BusinessLogin(MySqlDatabase db, BusinessUser user)
        {
            this.database = db;
            this.currentUser = user;
        }

        /// <summary>
        /// Log in.
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            return currentUser.Login(database);
        }
    }
}
