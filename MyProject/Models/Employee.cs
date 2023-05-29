using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    [Table("Tbl_Employee")]
    public class Employee
    {
        [Key]
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public int Salary { get; set; }
        [EmailAddress(ErrorMessage = "Alamat Email tidak valid!")]
        public string Email { get; set; }
        public Gender Gender { get; set; }
        [ForeignKey("Department")]
        public int Department_ID { get; set; }
        public virtual Department? Department { get; set; }
        public virtual Account? Account { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
