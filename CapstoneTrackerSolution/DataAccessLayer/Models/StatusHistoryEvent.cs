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
        public List<List<string>> Fetch()
        {
            List<List<string>> ary = null;
            string sqlStr = "SELECT * FROM StatusHistoryEvent WHERE capstoneID = '" + capstoneId + "';";
            ary = sql.GetData(sqlStr);
            if (ary != null && ary.Count != 0)
            {
                capstoneId = ary[0][1];
                statusCode = ary[0][2];
                timeStamp = ary[0][3];
            }
            return ary;
        }

        // Update existing record with new information at given ID
        public int Put()
        {
            string sqlStr = "UPDATE StatusHistoryEvent SET capstoneID = '" + capstoneId + "'," +
                "statusCode = '" + statusCode + ", timeStamp = '" + timeStamp + "';";
            return sql.SetData(sqlStr);
        }

        // Add new value to database
        public int Post()
        {
            string sqlStr = "INSERT INTO StatusHistoryEvent(capstoneID, statusCode, timeStamp)" +
                "VALUES('" + capstoneId + "', '" + statusCode + "', '" + timeStamp + "');";
            return sql.SetData(sqlStr);
        }

        // Delete record from database at given ID
        public int Delete()
        {
            string sqlStr = "DELETE FROM StatusHistoryEvent WHERE capstoneID = '" + capstoneId + "';";
            return sql.SetData(sqlStr);
        }
    }
}
