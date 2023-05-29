using MyProject.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyProject.View_Models
{
    public class RegisterVM
    {
        //public string NIK { get; set; } //no need to prefix Key attribute
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public int Salary { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int Gender { get; set; } //set type to int instead of Gender
        public int Department_ID { get; set; } //no need to prefix ForeginKey attribute
        [PasswordPropertyText(true)]
        [StringLength(64, ErrorMessage = "Panjang kata sandi harus antara 8 karakter s.d. 64 karakter", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
