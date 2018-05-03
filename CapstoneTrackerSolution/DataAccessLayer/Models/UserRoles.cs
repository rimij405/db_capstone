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

        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM UserRoles WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            userId = rs[0, "userID"].Value;
            roleCode = rs[0, "roleCode"].Value;
            currentStatus = rs[0, "currentStatus"].Value;
            statusTimestamp = rs[0, "statusTimestamp"].Value;

            return rs;
        }

        // Update existing record with new information at given ID
        public MySqlResultSet Put(MySqlDatabase sqlDb)
        {
            string sqlStr = "UPDATE UserRoles SET roleCode = @roleCode, currentStatus = @currentStatus, " +
                "statusTimestamp = @statusTimestamp WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@roleCode", roleCode },
                {"@currentStatus", currentStatus },
                {"@statusTimestamp", statusTimestamp },
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Add new value to database
        public MySqlResultSet Post(MySqlDatabase sqlDb)
        {
            string sqlStr = "INSERT INTO UserRoles(userID, roleCode, currentStatus, statusTimestamp)" +
                "VALUES(@userId, @roleCode, @currentStatus, @statusTimestamp);";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId},
                {"@roleCode", roleCode },
                {"@currentStatus", currentStatus },
                {"@statusTimestamp", statusTimestamp }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Delete record from database at given ID
        public MySqlResultSet Delete(MySqlDatabase sqlDb)
        {
            string sqlStr = "DELETE FROM UserRoles WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId}
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }
    }
}
