using System.ComponentModel.DataAnnotations;

namespace MovieWeb.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Email không được để trống!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
