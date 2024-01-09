using System.ComponentModel.DataAnnotations;

namespace MovieWeb.Models
{
	public class Genre
	{
		public long ID { get; set; }

        [Required(ErrorMessage = "Tên thể loại không được để trống!")]
        [Display(Name = "Tên thể loại")]
        public String Name { get; set; }
        public ICollection<MovieGenre>? MGs { get; set; }
    }
}
