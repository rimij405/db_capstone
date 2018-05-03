using ISTE.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Models
{
    class Students
    {
        string userId;
        string mastersStart;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public Students(string uid, string ms)
        {
            userId = uid;
            mastersStart = ms;
        }

        // Query database for info at given ID
        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM Students WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            userId = rs[0, "userID"].Value;
            mastersStart = rs[0, "mastersStart"].Value;

            return rs;
        }

        // Update existing record with new information at given ID
        public MySqlResultSet Put(MySqlDatabase sqlDb)
        {
            string sqlStr = "UPDATE Students SET mastersStart = @mastersStart WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@mastersStart", mastersStart},
                {"@userId", userId}
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Add new value to database
        public MySqlResultSet Post(MySqlDatabase sqlDb)
        {
            string sqlStr = "INSERT INTO Students(userID, mastersStart) VALUES(@userId, @mastersStart);";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId},
                {"@mastersStart", mastersStart}
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Delete record from database at given ID
        public MySqlResultSet Delete(MySqlDatabase sqlDb)
        {
            string sqlStr = "DELETE FROM Students WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId}
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }
    }
}
