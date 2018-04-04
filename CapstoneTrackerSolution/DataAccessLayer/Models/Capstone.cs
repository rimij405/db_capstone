using ISTE.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Models
{
    class Capstone
    {
        string capstoneId;
        string studentId;
        string chairId;
        string capstoneStartTerm;
        string defenseDateApprovedBy;
        string defenseDate;
        string title;
        string abstractStr;
        string plagarismScore;
        string grade;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public Capstone(string cid, string sid, string chrId, string cpStrt, string dfsDteAppBy, string dfsDte, string tl, string absStr, string plgrsm, string grd)
        {
            capstoneId = cid;
            studentId = sid;
            chairId = chrId;
            capstoneStartTerm = cpStrt;
            defenseDateApprovedBy = dfsDteAppBy;
            defenseDate = dfsDte;
            title = tl;
            abstractStr = absStr;
            plagarismScore = plgrsm;
            grade = grd;
        }

        // Query database for info at given ID
        public List<List<string>> Fetch()
        {
            List<List<string>> ary = null;
            string sqlStr = "SELECT * FROM Capstone WHERE capstoneID = '" + capstoneId + "';";
            ary = sql.GetData(sqlStr);
            if (ary != null && ary.Count != 0)
            {
                capstoneId = ary[0][1];
                studentId = ary[0][2];
                chairId = ary[0][3];
                capstoneStartTerm = ary[0][4];
                defenseDateApprovedBy = ary[0][5];
                defenseDate = ary[0][6];
                title = ary[0][7];
                abstractStr = ary[0][8];
                plagarismScore = ary[0][9];
                grade = ary[0][10];
            }
            return ary;
        }

        // Update existing record with new information at given ID
        public int Put()
        {
            string sqlStr = "UPDATE Capstone SET capstoneID = '" + capstoneId + "'," +
                "studentID = '" + studentId + "', chairID = '" + chairId + "', capstoneStartTerm = '" + capstoneStartTerm + 
                "', defenseDateApprovedBy = '" + defenseDateApprovedBy + "', defenseDate = '" + defenseDate + "', title = '" + title
                + "', plagarismScore = '" + plagarismScore + "', grade = '" + grade + "' WHERE capstoneID = '" + capstoneId + "';";
            return sql.SetData(sqlStr);
        }

        // Add new value to database
        public int Post()
        {
            string sqlStr = "INSERT INTO Capstone(capstoneID, studentID, " +
                "chairID, capstoneStartTerm, defenseDateApprovedBy, defenseDate, title, plagarismScore, grade)" +
                "VALUES('" + capstoneId + "', '" + studentId + "', '" + chairId + "', '" + capstoneStartTerm + 
                "', " + defenseDateApprovedBy + "', '" + defenseDate + "', '" + "', '" + title + "', '" +
                "', '" + plagarismScore + "', '" + grade + "');";
            return sql.SetData(sqlStr);
        }

        // Delete record from database at given ID
        public int Delete()
        {
            string sqlStr = "DELETE FROM Capstone WHERE capstoneID = " + capstoneId + ";";
            return sql.SetData(sqlStr);
        }
    }
}
