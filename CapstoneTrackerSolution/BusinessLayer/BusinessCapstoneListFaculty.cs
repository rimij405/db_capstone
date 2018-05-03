using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    // Business layer class that handles taking data from the database and converting it to a usable format for the
    // presentation layer and vice versa for the capstone list - faculty form
    // Author: Jake Toporoff
    public class BusinessCapstoneListFaculty
    {
        List<string> pendingCapstones;
        List<string> currentCapstones;
        List<string> currentGrades;

        // CapstoneListFaculty Get Functions
        public List<string> CapstoneLFGetPendingCapstones()
        {
            List<string> capstones = new List<string>();
            capstones.Add("The history of something important");
            capstones.Add("A study of paint drying");
            return capstones;
        }

        public List<string> CapstoneLFGetCurrentCapstones()
        {
            List<string> capstones = new List<string>();
            capstones.Add("The history of something important");
            capstones.Add("A study of paint drying");
            return capstones;
        }

        public List<string> CapstoneLFGetCurrentGrades()
        {
            List<string> grades = new List<string>();
            grades.Add("F--");
            grades.Add("A");
            return grades;
        }
        // End CapstoneListFaculty Get Functions

        // CapstoneListFaculty Set Functions
        public void CapstoneLFAcceptInvitation(string capstone)
        {
            return;
        }

        public void CapstoneLFRejectInvitation(string capstone)
        {
            return;
        }
        // End CapstoneListFaculty Set Functions
    }
}
