using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer
{
    // Business layer class that handles taking data from the database and converting it to a usable format for the
    // presentation layer and vice versa for the status history form
    // Author: Jake Toporoff
    public class BusinessStatusHistory
    {
        List<string> statuses;
        List<string> dates;
        string description;

        // StatusHistory Get Functions
        public List<string> StatusHistoryGetStatuses()
        {
            List<string> statuses = new List<string>();
            statuses.Add("Pending");
            statuses.Add("Completed");
            return statuses;
        }

        public List<string> StatusHistoryGetStatusDates()
        {
            List<string> dates = new List<string>();
            dates.Add("01/01/2001");
            dates.Add("01/01/2002");
            return dates;
        }

        public string StatusHistoryGetDescription(string selected)
        {
            return selected;
        }
        // End StatusHistory Get Functions
    }
}
