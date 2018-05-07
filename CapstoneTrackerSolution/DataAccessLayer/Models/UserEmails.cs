using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ISTE.DAL.Database;
using ISTE.DAL.Database.Implementations;
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Models
{
    class UserEmails
    {
        string userId;
        string email;
        string emailType;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public UserEmails(string uid, string em, string et)
        {
            userId = uid;
            email = em;
            emailType = et;
        }

        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM UserEmails WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            userId = rs[0, "userID"].Value;
            email = rs[0, "email"].Value;
            emailType = rs[0, "emailType"].Value;

            return rs;
        }

        // Update existing record with new information at given ID
        public MySqlResultSet Put(MySqlDatabase sqlDb)
        {
            string sqlStr = "UPDATE UserEmails SET email = @email, emailType = @emailType WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@email", email },
                {"@emailType", emailType },
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Add new value to database
        public MySqlResultSet Post(MySqlDatabase sqlDb)
        {
            string sqlStr = "INSERT INTO UserEmails(userID, email, emailType)" +
                "VALUES(@userId, @email, @emailType);";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId},
                {"@email", email },
                {"@emailType", emailType }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Delete record from database at given ID
        public MySqlResultSet Delete(MySqlDatabase sqlDb)
        {
            string sqlStr = "DELETE FROM UserEmails WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId}
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }
    }
}
