using System.ComponentModel.DataAnnotations;

namespace VMS.Models
{
    public class Employee
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        public string EmployeeFirstName { get; set; }

        public string EmployeeLastName { get; set; }

        public string Location { get; set; }

        public string Department { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string EmployeePhotoUrl { get; set; }

        public string EmployeeSearchText { get; set; }
    }
}
