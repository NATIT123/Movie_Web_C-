using System.ComponentModel.DataAnnotations;

namespace MovieWeb.Models
{
    public class MGViewModel
    {
        [Display(Name = "Phim")]
        public long MovieID { get; set; }

        [Display(Name = "Thể loại")]
        public List<long> GenreID { get; set; }
    }
}
