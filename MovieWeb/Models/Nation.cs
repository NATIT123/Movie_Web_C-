using System.ComponentModel.DataAnnotations;

namespace MovieWeb.Models
{
    public class Nation
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "Tên quốc gia không được để trống!")]
        [Display(Name = "Tên quốc gia")]
        public String Name { get; set; }
    }
}
