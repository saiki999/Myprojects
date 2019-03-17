using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;

namespace VMS.Models
{
    public class VisitorVisit
    {


        public string VisitorFirstName { get; set; }

        public string VisitorLastName { get; set; }

        public string Email { get; set; }
        public string Company { get; set; }

        public string EmployeeFirstName { get; set; }

        public string EmployeeLastName { get; set; }

        public double Phone { get; set; }

        public string ImageURL { get; set; }


        public string Purpose { get; set; }


        public string OfficeLocation { get; set; }

        public int HostEmployeeId { get; set; }

        public DateTime VisitorCheckInTime { get; set; }

        public DateTime VisitorCheckOutTime { get; set; }

    }
}
