using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyProject.View_Models
{
    public class LoginVM
    {
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText(true)]
        public string Password { get; set; }
    }
}
