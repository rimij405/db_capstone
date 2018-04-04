using ISTE.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Models
{
    class UserRoles
    {
        string userId;
        string roleCode;
        string currentStatus;
        string statusTimestamp;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public UserRoles(string uid, string rlCd, string crntSts, string stsTmstmp)
        {
            userId = uid;
            roleCode = rlCd;
            currentStatus = crntSts;
            statusTimestamp = stsTmstmp;
        }

        // Query database for info at given ID
        public List<List<string>> Fetch()
        {
            List<List<string>> ary = null;
            string sqlStr = "SELECT * FROM UserRoles WHERE userID = '" + userId + "';";
            ary = sql.GetData(sqlStr);
            if (ary != null && ary.Count != 0)
            {
                userId = ary[0][1];
                roleCode = ary[0][2];
                currentStatus = ary[0][3];
                statusTimestamp = ary[0][4];
            }
            return ary;
        }

        // Update existing record with new information at given ID
        public int Put()
        {
            string sqlStr = "UPDATE UserRoles SET userID = '" + userId + "'," +
                "roleCode = '" + roleCode + ", currentStatus = '" + currentStatus + 
                "', statusTimestamp = '" + statusTimestamp + "';";
            return sql.SetData(sqlStr);
        }

        // Add new value to database
        public int Post()
        {
            string sqlStr = "INSERT INTO UserRoles(userID, roleCode, currentStatus, statusTimestamp)" +
                "VALUES('" + userId + "', '" + roleCode + "', '" + currentStatus + "', '" + statusTimestamp + "');";
            return sql.SetData(sqlStr);
        }

        // Delete record from database at given ID
        public int Delete()
        {
            string sqlStr = "DELETE FROM UserRoles WHERE userID = '" + userId + "';";
            return sql.SetData(sqlStr);
        }
    }
}
