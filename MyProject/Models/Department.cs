using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    [Table("Tbl_Department")]
    public class Department
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        //public virtual ICollection<Employee> Employess { get; set; }
    }
}
