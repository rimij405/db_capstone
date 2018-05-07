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
    class Term
    {
        string termCode;
        string termStart;
        string termEnd;
        string gradeDeadline;
        string addDropDeadline;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public Term(string tcd, string tStrt, string tEnd, string grdDln, string addDrpDln)
        {
            termCode = tcd;
            termStart = tStrt;
            termEnd = tEnd;
            gradeDeadline = grdDln;
            addDropDeadline = addDrpDln;
        }

        // Query database for info at given ID
        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM Term WHERE termCode = @term;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@term", termCode}
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            termCode = rs[0, "termCode"].Value;
            termStart = rs[0, "termStart"].Value;
            termEnd = rs[0, "termEnd"].Value;
            gradeDeadline = rs[0, "gradeDeadline"].Value;
            addDropDeadline = rs[0, "addDropDeadline"].Value;

            return rs;
        }

        // Update existing record with new information at given ID
        public MySqlResultSet Put(MySqlDatabase sqlDb)
        {
            string sqlStr = "UPDATE Term SET termStart = @termStart, termEnd = @termEnd, " +
                "gradeDeadline = @gradeDeadline, addDropDeadline = @addDropDeadline WHERE termCode = @termCode;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@termStart", termStart},
                {"@termEnd", termEnd },
                {"@gradeDeadline", gradeDeadline },
                {"@addDropDeadline", addDropDeadline }
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }
    }
}
