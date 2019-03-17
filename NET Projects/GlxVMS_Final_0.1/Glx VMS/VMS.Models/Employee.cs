using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMS.Models
{
    public class Employee
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        public string EmployeeFirstName { get; set; }

        public string EmployeeLastName { get; set; }

        public string Location { get; set; }

        public string Department { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public double Phone { get; set; }

        [Required]
        public string EmployeePhotoUrl { get; set; }

        public string EmployeeSearchText { get; set; }
    }
}
