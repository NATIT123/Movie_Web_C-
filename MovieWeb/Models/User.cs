using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace MovieWeb.Models
{
    public class User
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "Tên tài khoản không được để trống!")]
        [MaxLength(50, ErrorMessage = "Tên tài khoản không được vượt quá 50 ký tự!")]
        [Display(Name = "Tên tài khoản")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email không được để trống!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(20, ErrorMessage = "{0} phải từ {2} đến {1} ký tự!", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        public string Role { get; set; }

        [Display(Name = "Xác thực Email")]
        public bool IsEmailConfirmed { get; set; }
        public ICollection<MovieUser>? Follows { get; set; }
    }
}
