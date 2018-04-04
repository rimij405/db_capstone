using ISTE.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Query database for info at given ID
        public List<List<string>> Fetch()
        {
            List<List<string>> ary = null;
            string sqlStr = "SELECT * FROM UserPhones WHERE userID = '" + userId + "';";
            ary = sql.GetData(sqlStr);
            if (ary != null && ary.Count != 0)
            {
                userId = ary[0][1];
                number = ary[0][2];
                phoneType = ary[0][3];
            }
            return ary;
        }

        // Update existing record with new information at given ID
        public int Put()
        {
            string sqlStr = "UPDATE UserPhones SET userID = '" + userId + "'," +
                "number = '" + number + ", phoneType = '" + phoneType + "';";
            return sql.SetData(sqlStr);
        }

        // Add new value to database
        public int Post()
        {
            string sqlStr = "INSERT INTO UserPhones(userID, number, phoneType)" + 
                "VALUES('" + userId + "', '" + number + "', '" + phoneType + "');";
            return sql.SetData(sqlStr);
        }

        // Delete record from database at given ID
        public int Delete()
        {
            string sqlStr = "DELETE FROM UserPhones WHERE userID = '" + userId + "';";
            return sql.SetData(sqlStr);
        }
    }
}
