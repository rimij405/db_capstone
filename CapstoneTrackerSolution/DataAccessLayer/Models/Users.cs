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

        // Query database for info at given ID
        public List<List<string>> Fetch()
        {
            List<List<string>> ary = null;
            string sqlStr = "SELECT * FROM Users WHERE userID = '" + userId + "';";
            ary = sql.GetData(sqlStr);
            if (ary != null && ary.Count != 0)
            {
                userId = ary[0][1];
                username = ary[0][2];
                password = ary[0][3];
                firstName = ary[0][4];
                lastName = ary[0][5];
            }
            return ary;
        }

        // Update existing record with new information at given ID
        public int Put()
        {
            string sqlStr = "UPDATE Users SET userID = '" + userId + "'," +
                "username = '" + username + "', password = '" + password + "', firstName = '" + firstName + "', lastName = '" + lastName + "' WHERE" +
                " userID = '" + userId + "';";
            return sql.SetData(sqlStr);
        }

        // Add new value to database
        public int Post()
        {
            string sqlStr = "INSERT INTO Users(userID, username, " +
                "password, firstName, lastName) VALUES('" + userId + "', '" + username + "', '" +
                password + "', '" + firstName + "', '" + lastName + "');";
            return sql.SetData(sqlStr);
        }

        // Delete record from database at given ID
        public int Delete()
        {
            string sqlStr = "DELETE FROM Users WHERE userID = '" + userId + "';";
            return sql.SetData(sqlStr);
        }
    }
}
