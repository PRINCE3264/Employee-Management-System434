using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement22.Entity
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string JobTitle { get; set; }
        public string Gender { get; set; }

        // Foreign key to Department
        public int? DepartmentId { get; set; }

        // ✅ Correct navigation property
        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        public DateTime JoiningDate { get; set; }
        public DateTime LastWorkingDate { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
    }

}
