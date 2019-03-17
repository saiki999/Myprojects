using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VMS.Models
{
    public class Visitor
    {
        [Key]
        public int VisitorId { get; set; }
        [Required]
        public string VisitorFirstName { get; set; }
        [Required]
        public string VisitorLastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Company { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public double Phone { get; set; }

        public string ImageURL { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}
