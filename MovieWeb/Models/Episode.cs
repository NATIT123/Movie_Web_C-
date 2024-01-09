using System.ComponentModel.DataAnnotations;

namespace MovieWeb.Models
{
	public class Episode
	{
		public long ID { get; set; }

        [Required(ErrorMessage = "Tập phim không được để trống!")]
        [Display(Name = "Tập phim")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Nguồn phim không được để trống!")]
        [Display(Name = "Nguồn phim")]
        public String videoSrc { get; set;}

        [Display(Name = "Phim")]
        public long MovieID { get; set; }
		public Movie? Movie { get; set; }
    }
}
