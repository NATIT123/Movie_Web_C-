namespace MovieWeb.Models
{
    public class ViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Movie> Trending { get; set; }
        public IEnumerable<Nation> Nations { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Genre> _Genres { get; set; }
        public IEnumerable<Episode> Episodes { get; set; }
        public IEnumerable<MovieUser> Follows { get; set; }
        public IEnumerable<MovieGenre> MGs { get; set; }
        public IEnumerable<Movie> Theme { get; set; }
        public Login Login { get; set; }
        public Signup Signup { get; set; }
        public ForgotPass ForgotPass { get; set; }
        public MGViewModel MGVM { get; set; }
        public MovieGenre MG { get; set; } 
        public User User { get; set; }
        public Movie Movie { get; set; }
        public Genre Genre { get; set; }
        public Nation Nation { get; set; }
        public Episode Episode { get; set; }
    }
}
