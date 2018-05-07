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
        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM Capstone WHERE capstoneID = @capstoneId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@capstoneId", capstoneId}
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            capstoneId = rs["capstoneID"][0].Value;
            studentId = rs["studentID"][0].Value;
            chairId = rs["chairID"][0].Value;
            capstoneStartTerm = rs["capstoneStartTerm"][0].Value;
            defenseDateApprovedBy = rs["defenseDateApprovedBy"][0].Value;
            defenseDate = rs["defenseDate"][0].Value;
            title = rs["title"][0].Value;
            abstractStr = rs["abstract"][0].Value;
            plagarismScore = rs["plagarismScore"][0].Value;
            grade = rs["grade"][0].Value;

            return rs;
        }

        // Update existing record with new information at given ID
        public MySqlResultSet Put(MySqlDatabase sqlDb)
        {
            string sqlStr = "UPDATE Capstone SET" +
                "studentID = @studentId, chairID = @chairId, capstoneStartTerm = @capstoneStartTerm, " +
                "defenseDateApprovedBy = @defenseDateApprovedBy, defenseDate = @defenseDate, title = @title, " + 
                "plagarismScore = @plagarismScore, grade = @grade WHERE capstoneID = @capstoneId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@studentId", studentId },
                {"@chairId", chairId },
                {"@capstoneStartTerm", capstoneStartTerm },
                {"@defenseDateApprovedBy", defenseDateApprovedBy },
                {"@defenseDate", defenseDate },
                {"@title", title },
                {"@plagarismScore", plagarismScore },
                {"@grade", grade }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Add new value to database
        public MySqlResultSet Post(MySqlDatabase sqlDb)
        {
            string sqlStr = "INSERT INTO Capstone(capstoneID, studentID, " +
                "chairID, capstoneStartTerm, defenseDateApprovedBy, defenseDate, title, plagarismScore, grade)" +
                "VALUES(@capstoneId, @studentId, @chairId, @capstoneStartTerm, @defenseDateApprovedBy, @defenseDate" +
                ", @title, @plagarismScore, @grade);";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@capstoneId", capstoneId },
                {"@studentId", studentId },
                {"@chairId", chairId },
                {"@capstoneStartTerm", capstoneStartTerm },
                {"@defenseDateApprovedBy", defenseDateApprovedBy },
                {"@defenseDate", defenseDate },
                {"@title", title },
                {"@plagarismScore", plagarismScore },
                {"@grade", grade }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Delete record from database at given ID
        public MySqlResultSet Delete(MySqlDatabase sqlDb)
        {
            string sqlStr = "DELETE FROM Capstone WHERE capstoneID = @capstoneId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@capstoneId", capstoneId }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }
    }
}
