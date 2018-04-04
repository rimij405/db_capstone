using ISTE.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<List<string>> Fetch()
        {
            List<List<string>> ary = null;
            string sqlStr = "SELECT * FROM Term WHERE termCode = '" + termCode + "';";
            ary = sql.GetData(sqlStr);
            if (ary != null && ary.Count != 0)
            {
                termCode = ary[0][1];
                termStart = ary[0][2];
                termEnd = ary[0][3];
                gradeDeadline = ary[0][4];
                addDropDeadline = ary[0][5];
            }
            return ary;
        }

        // Update existing record with new information at given ID
        public int Put()
        {
            string sqlStr = "UPDATE Term SET termCode = '" + termCode + "'," +
                "termStart = '" + termStart + ", termEnd = '" + termEnd +
                "', gradeDeadline = '" + gradeDeadline + 
                "', addDropDeadline = '" + addDropDeadline + "';";
            return sql.SetData(sqlStr);
        }
    }
}
