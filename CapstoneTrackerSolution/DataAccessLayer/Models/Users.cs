using ISTE.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Models
{
    class Users
    {
        string userId;
        string username;
        string password;
        string firstName;
        string lastName;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public Users(string uid, string un, string pw, string fn, string ln)
        {
            userId = uid;
            username = un;
            password = pw;
            firstName = fn;
            lastName = ln;
        }

        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM Users WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            userId = rs[0, "userID"].Value;
            username = rs[0, "username"].Value;
            password = rs[0, "password"].Value;
            firstName = rs[0, "firstName"].Value;
            lastName = rs[0, "lastName"].Value;

            return rs;
        }

        // Update existing record with new information at given ID
        public MySqlResultSet Put(MySqlDatabase sqlDb)
        {
            string sqlStr = "UPDATE Users SET username = @username, password = @password, firstName = @firstName, " +
                "lastName = @lastName WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@username", username },
                {"@password", password },
                {"@firstName", firstName },
                {"@lastName", lastName },
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Add new value to database
        public MySqlResultSet Post(MySqlDatabase sqlDb)
        {
            string sqlStr = "INSERT INTO Users(userID, username, " +
                "password, firstName, lastName) VALUES(@userId, @username, @password, " +
                "@firstName, @lastName);";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId},
                {"@username", username },
                {"@password", password },
                {"@firstName", firstName },
                {"@lastName", lastName }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Delete record from database at given ID
        public MySqlResultSet Delete(MySqlDatabase sqlDb)
        {
            string sqlStr = "DELETE FROM Users WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId}
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }
    }
}
