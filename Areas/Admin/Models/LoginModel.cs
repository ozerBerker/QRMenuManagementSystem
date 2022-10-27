using System.ComponentModel.DataAnnotations;

namespace QRMenuManagementSystem.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public LoginModel(string _Email, string _Password)
        {
            Email= _Email;
            Password= _Password;
        }
    }
}
