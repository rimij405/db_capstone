using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer
{
    // Business layer class that handles taking data from the database and converting it to a usable format for the
    // presentation layer and vice versa for the capstone list - staff form
    // Author: Jake Toporoff
    public class BusinessCapstoneListStaff
    {
        List<string> capstones;
        List<string> statuses;

        // CapstoneListStaff GetFunctions
        public List<string> CapstoneLSGetCapstones(int format, int selection) // format depends on the combo box selections within the form
        {
            List<string> capstones = new List<string>();
            capstones.Add("The history of something important");
            capstones.Add("A study of paint drying");
            return capstones;
        }

        public List<string> CapstoneLSGetStatuses(int format, int selection)
        {
            List<string> statuses = new List<string>();
            statuses.Add("Rejected");
            statuses.Add("Graded");
            return statuses;
        }
        // End CapstoneListStaff Get Functions
    }
}
