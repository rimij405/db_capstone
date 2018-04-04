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
        public List<List<string>> Fetch()
        {
            List<List<string>> ary = null;
            string sqlStr = "SELECT * FROM Students WHERE userID = '" + userId + "';";
            ary = sql.GetData(sqlStr);
            if (ary != null && ary.Count != 0)
            {
                userId = ary[0][1];
                mastersStart= ary[0][2];
            }
            return ary;
        }

        // Update existing record with new information at given ID
        public int Put()
        {
            string sqlStr = "UPDATE Students SET userID = '" + userId + "'," +
                "mastersStart = '" + mastersStart + "';";
            return sql.SetData(sqlStr);
        }

        // Add new value to database
        public int Post()
        {
            string sqlStr = "INSERT INTO Students(userID, mastersStart) VALUES('" + userId + "', '" + mastersStart + "');";
            return sql.SetData(sqlStr);
        }

        // Delete record from database at given ID
        public int Delete()
        {
            string sqlStr = "DELETE FROM Students WHERE userID = '" + userId + "';";
            return sql.SetData(sqlStr);
        }
    }
}
