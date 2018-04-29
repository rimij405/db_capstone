using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer
{
    // Business layer class that handles taking data from the database and converting it to a usable format for the
    // presentation layer and vice versa for the capstone page view form
    // Author: Jake Toporoff
    public class BusinessCapstonePageView
    {
        string title;
        string description;
        List<string> faculty;
        List<bool> accepted; // will be appended onto and returned with faculty in readable format
        string defenseDate;
        string status;
        string grade;
        string plagarismScore;

        // CapstonePageView Get Functions
        public string CapstonePVGetTitle()
        {
            return "capstone title";
        }

        public string CapstonePVGetAbstract()
        {
            return "capstone description";
        }

        public List<string> CapstonePVGetFaculty()
        {
            List<string> faculty = new List<string>();
            faculty.Add("John Smith");
            faculty.Add("Firstname Lastname");
            return faculty;
        }

        public string CapstonePVGetDefenseDate()
        {
            return "DateTime";
        }

        public string CapstonePVGetStatus()
        {
            return "current status";
        }

        public string CapstonePVGetGrade()
        {
            return "Grade";
        }

        public string CapstonePVGetPlagarismScore()
        {
            return "Plagarism Score";
        }
        // End CapstonePageView Get Functions
    }
}
