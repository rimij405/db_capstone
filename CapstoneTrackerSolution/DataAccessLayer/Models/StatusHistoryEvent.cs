using ISTE.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Models
{
    class StatusHistoryEvent
    {
        string capstoneId;
        string statusCode;
        string timeStamp;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public StatusHistoryEvent(string cid, string stsCd, string tmStmp)
        {
            capstoneId = cid;
            statusCode = stsCd;
            timeStamp = tmStmp;
        }

        // Query database for info at given ID
        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM StatusHistoryEvent WHERE capstoneID = @capstoneId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@capstoneId", capstoneId }
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            capstoneId = rs[0, "capstoneID"].Value;
            statusCode = rs[0, "statusCode"].Value;
            timeStamp = rs[0, "timeStamp"].Value;

            return rs;
        }

        // Update existing record with new information at given ID
        public MySqlResultSet Put(MySqlDatabase sqlDb)
        {
            string sqlStr = "UPDATE StatusHistoryEvent SET statusCode = @statusCode, timeStamp = @timeStamp WHERE capstoneID = @capstoneId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@statusCode", statusCode },
                {"@timeStamp", timeStamp },
                {"@capstoneId", capstoneId }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Add new value to database
        public MySqlResultSet Post(MySqlDatabase sqlDb)
        {
            string sqlStr = "INSERT INTO StatusHistoryEvent(capstoneID, statusCode, timeStamp)" +
                "VALUES(@capstoneId, @statusCode, @timeStamp);";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@capstoneId", capstoneId },
                {"@statusCode", statusCode },
                {"@timeStamp", timeStamp}
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Delete record from database at given ID
        public MySqlResultSet Delete(MySqlDatabase sqlDb)
        {
            string sqlStr = "DELETE FROM StatusHistoryEvent WHERE capstoneID = @capstoneId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@capstoneId", capstoneId }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }
    }
}
