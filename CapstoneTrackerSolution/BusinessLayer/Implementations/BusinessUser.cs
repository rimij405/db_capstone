/*
    BusinessUser.cs
    ---
    Ian Effendi
 */

using ISTE.DAL.Models.Implementations;
using ISTE.DAL.Database.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISTE.DAL.Database.Interfaces;
using ISTE.DAL.Models;
using Services;

namespace ISTE.BAL.Implementations
{
    /// <summary>
    /// Represents a business user in the business layer.
    /// </summary>
    public class BusinessUser
    {

        private MySqlDatabase database;

        string userID = "";
        string username = "";
        string password = "";
        string firstName = "";
        string lastName = "";
        BusinessRole role = null;
        BusinessEmails emails = null;
        BusinessPhones phones = null;

        bool loggedIn = false;

        /// <summary>
        /// Reference to username.
        /// </summary>
        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }

        public string FirstName
        {
            get { return this.firstName; }
        }

        public string LastName
        {
            get { return this.lastName; }
        }

        /// <summary>
        /// Reference to the userrole.
        /// </summary>
        public BusinessRole UserRole
        {
            get
            {
                if (role == null) {
                    role = new BusinessRole(database, this.GetRoleCode(database));
                }
                return role;
            }
        }

        /// <summary>
        /// Construct a business user by ID.
        /// </summary>
        /// <param name="userID"></param>
        public BusinessUser(MySqlDatabase database, string username, string password)
        {
            this.loggedIn = false;
            this.database = database;
            this.username = username;
            this.password = password;         
        }

        /// <summary>
        /// Authenticate the user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool Authenticate(MySqlDatabase database)
        {
            // Set username
            MySqlUser db_User = new MySqlUser();
            db_User.Username = username;

            // Check if the user is authenticated.
            return db_User.Authenticate(database, out IResultSet results);
        }

        /// <summary>
        /// Check if the user is authorized.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public bool Authorize(MySqlDatabase database)
        {
            // Set username and password.
            MySqlUser db_User = new MySqlUser();
            db_User.Username = username;
            db_User.Password = password;

            // Check if the user is authenticated.
            if (db_User.Authenticate(database, out IResultSet resultsA) && db_User.Authorize(database, out IResultSet resultsB))
            {
                if(resultsA != null && !resultsA.IsEmpty)
                {
                    if (resultsB != null && !resultsB.IsEmpty)
                    {
                        this.userID = resultsB[0, 0].Value;
                        this.username = resultsB[0, 1].Value;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// If the user is successfully authorized, fetch the data.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public bool Fetch(MySqlDatabase database)
        {
            if (!Authorize(database)) { return false; }

            MySqlUser db_User = new MySqlUser();
            db_User.UserID = new MySqlID(userID);
            db_User.Username = username;
            db_User.Password = password;

            if (!db_User.TryFetch(database, out IResultSet results, out DatabaseError errorCode))
            {
                Logger log = new Logger("", "business-error", "txt");
                log.Write($"Error occured while fetching user information. (DatabaseError: {errorCode})");
                log.Write($"{results.ToString()}");
                log.Write($"User {db_User.UserID}: {db_User.Username}");
                return false;
            }
            else
            {
                Logger log = new Logger("", "business-debug", "txt");
                log.Write($"Fetching user info.");
                this.userID = db_User["userID"].Value;
                this.username = db_User["username"].Value;
                this.password = db_User["password"].Value;
                this.firstName = db_User["firstName"].Value;
                this.lastName = db_User["lastName"].Value;
                log.Write($"Current User Role: {this.UserRole}");
                log.Write($"Current user: {db_User.Model.ToString()}");
                return true;
            }
        }

        /// <summary>
        /// Login function.
        /// </summary>
        /// <returns></returns>
        public bool Login(MySqlDatabase database)
        {
            if (!loggedIn)
            {
                loggedIn = Fetch(database);
            }
            return loggedIn;
        }

        public void Logout() { this.loggedIn = false; }

        public string GetRoleCode(MySqlDatabase database)
        {
            if (!loggedIn) { return ""; }

            MySqlUser db_User = new MySqlUser();
            db_User.UserID = new MySqlID(userID);

            MySqlResultSet set = db_User.FetchRoleCode(database, out DatabaseError err) as MySqlResultSet;
            if(set == null || set.IsEmpty) { return ""; }
            return set[0, 0].Value;
        }


    }
}
