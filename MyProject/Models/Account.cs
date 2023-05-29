using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    [Table("Tbl_Account")]
    public class Account
    {
        [Key,ForeignKey("Employee")]
        public string NIK { get; set; }
        //[MinLength(8, ErrorMessage="Kata sandi harus memiliki setidaknya 8 karakter")]
        [PasswordPropertyText(true)]
        [StringLength(64, ErrorMessage = "Panjang kata sandi harus antara 8 karakter s.d. 64 karakter", MinimumLength = 8)]
        public string Password { get; set; }

        public virtual Employee? Employee { get; set; }
    }
}
