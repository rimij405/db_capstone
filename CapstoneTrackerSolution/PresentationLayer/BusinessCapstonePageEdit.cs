using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer
{
    // Business layer class that handles taking data from the database and converting it to a usable format for the
    // presentation layer and vice versa for the capstone page edit form
    // Author: Jake Toporoff
    public class BusinessCapstonePageEdit
    {
        string title;
        string description;
        List<string> faculty;
        List<bool> accepted; // will be appended onto and returned with faculty in readable format
        string defenseDate;
        string status;
        string grade;
        string plagarismScore;

        // CapstonePageEdit Get Functions
        public string CapstonePEGetTitle()
        {
            return "capstone title";
        }

        public string CapstonePEGetAbstract()
        {
            return "capstone description";
        }

        public List<string> CapstonePEGetFaculty()
        {
            List<string> faculty = new List<string>();
            faculty.Add("John Smith");
            faculty.Add("Firstname Lastname");
            return faculty;
        }

        public string CapstonePEGetStatus()
        {
            return "current status";
        }

        public string CapstonePEGetGrade()
        {
            return "Grade";
        }

        public string CapstonePEGetPlagarismScore()
        {
            return "Plagarism Score";
        }

        public DateTime CapstonePEGetDefenseDate()
        {
            return DateTime.Parse("DateTime");
        }
        // End CapstonePageEdit Get Functions

        // CapstonePageEdit Set Functions
        public void CapstonePESaveChanges(string title, string desc, List<string> faculty, string date, string status, string grade, string plagScore)
        {
            SetTitle(title);
            SetAbstract(desc);
            SetFaculty(faculty);
            SetDate(date);
            SetStatus(status);
            SetGrade(grade);
            SetPlagarismScore(plagScore);
        }

        private void SetTitle(string title)
        {
            return;
        }

        private void SetAbstract(string desc)
        {
            return;
        }

        private void SetFaculty(List<string> faculty)
        {
            return;
        }

        private void SetDate(string date)
        {
            return;
        }

        private void SetStatus(string status)
        {
            return;
        }

        private void SetGrade(string grade)
        {
            return;
        }

        private void SetPlagarismScore(string plagScore)
        {
            return;
        }
        // End CapstonePageEdit Set Functions
    }
}
