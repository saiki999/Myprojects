using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Models
{
    public class Visit
    {
        [Key]
        public int VisitId { get; set; }

        [ForeignKey("Visitor")]
        public int VisitorId { get; set; }

        [Required]
        public string Purpose { get; set; }

        [Required]
        public string OfficeLocation { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int HostEmployeeId { get; set; }

        public DateTime VisitorCheckInTime { get; set; }

        public DateTime VisitorCheckOutTime { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public virtual Visitor Visitor { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
