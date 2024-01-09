using System.ComponentModel.DataAnnotations;

namespace MovieWeb.Models
{
    public class Signup
    {
        [Required(ErrorMessage = "Tên tài khoản không được để trống!")]
        [MaxLength(50, ErrorMessage = "Tên tài khoản không được vượt quá 50 ký tự!")]
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email không được để trống!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(20, ErrorMessage = "{0} phải từ {2} đến {1} ký tự!", MinimumLength = 6)]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{0,}$", ErrorMessage = "Mật khẩu phải chứa 1 ký tự hoa, 1 ký tự thường và 1 chữ số!")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Xác nhận mật khẩu không được để trống.")]
        [Compare("Password", ErrorMessage = "Xác nhận mật khẩu sai, hãy thử lại!")]
        [Display(Name = "Xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        public string confirmPassword { get; set; }
    }
}
