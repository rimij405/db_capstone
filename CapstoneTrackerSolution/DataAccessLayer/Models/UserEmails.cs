using ISTE.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Query database for info at given ID
        public List<List<string>> Fetch()
        {
            List<List<string>> ary = null;
            string sqlStr = "SELECT * FROM UserEmails WHERE userID = '" + userId + "';";
            ary = sql.GetData(sqlStr);
            if (ary != null && ary.Count != 0)
            {
                userId = ary[0][1];
                email = ary[0][2];
                emailType = ary[0][3];
            }
            return ary;
        }

        // Update existing record with new information at given ID
        public int Put()
        {
            string sqlStr = "UPDATE UserEmails SET userID = '" + userId + "'," +
                "email = '" + email + ", emailType = '" + emailType + "';";
            return sql.SetData(sqlStr);
        }

        // Add new value to database
        public int Post()
        {
            string sqlStr = "INSERT INTO UserEmails(userID, email, emailType)" +
                "VALUES('" + userId + "', '" + email + "', '" + emailType + "');";
            return sql.SetData(sqlStr);
        }

        // Delete record from database at given ID
        public int Delete()
        {
            string sqlStr = "DELETE FROM UserEmails WHERE userID = '" + userId + "';";
            return sql.SetData(sqlStr);
        }
    }
}
