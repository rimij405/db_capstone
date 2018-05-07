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
    class UserPhones
    {
        string userId;
        string number;
        string phoneType;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public UserPhones(string uid, string nm, string pt)
        {
            userId = uid;
            number = nm;
            phoneType = pt;
        }

        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM UserPhones WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            userId = rs[0, "userID"].Value;
            number = rs[0, "number"].Value;
            phoneType = rs[0, "phoneType"].Value;

            return rs;
        }

        // Update existing record with new information at given ID
        public MySqlResultSet Put(MySqlDatabase sqlDb)
        {
            string sqlStr = "UPDATE UserEmails SET number = @number, phoneType = @phoneType WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@number", number },
                {"@phoneType", phoneType },
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Add new value to database
        public MySqlResultSet Post(MySqlDatabase sqlDb)
        {
            string sqlStr = "INSERT INTO UserEmails(userID, number, phonetype)" +
                "VALUES(@userId, @number, @phoneType);";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId},
                {"@number", number },
                {"@phoneType", phoneType }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Delete record from database at given ID
        public MySqlResultSet Delete(MySqlDatabase sqlDb)
        {
            string sqlStr = "DELETE FROM UserPhones WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId}
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }
    }
}
