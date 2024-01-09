using System.ComponentModel.DataAnnotations;

namespace MovieWeb.Models
{
    public class ForgotPass
    {
        [Required(ErrorMessage = "Email không được để trống!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
