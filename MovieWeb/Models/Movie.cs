using System.ComponentModel.DataAnnotations;

namespace MovieWeb.Models
{
	public class Movie
	{
		public long ID { get; set; }

        [Required(ErrorMessage = "Tên phim không được để trống!")]
        [Display(Name = "Tên phim")]
        public String Name { get; set; }

        [Display(Name = "Mô tả")]
        public String? Description { get; set; }

        [Required(ErrorMessage = "Tình trạng không được để trống!")]
        [Display(Name = "Tình trạng phim")]
        public String Status { get; set; }

        [Display(Name = "Ngày cập nhật")]
        public String updatedDate { get; set; }

        [Required(ErrorMessage = "Thời lượng không được để trống!")]
        [Display(Name = "Thời lượng")]
        public String Duration { get; set; }

        [Required(ErrorMessage = "Tình trạng phim không được để trống!")]
        [Display(Name = "Năm phát hành")]
        public int realeasedYear { get; set; }

        [Display(Name = "Lượt thích")]
        public int numOfLikes { get; set; }

        [Display(Name = "Lượt xem")]
        public int numOfViews { get; set; }

        [Display(Name = "Đánh giá")]
        public double Rating { get; set; }

        [Display(Name = "Loại phim")]
        public String Type { get; set; }

        [Required(ErrorMessage = "Nguồn ảnh không được để trống!")]
        [Display(Name = "Nguồn ảnh")]
        public String imgSrc { get; set; }

        [Display(Name = "Quốc gia")]
        public long NationID { get; set; }
        public Nation? Nation { get; set; }
        public ICollection<Episode>? Episodes { get; set; }
        public ICollection<MovieGenre>? MGs { get; set; }
        public ICollection<MovieUser>? Follows { get; set; }
    }
}
