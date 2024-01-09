using Microsoft.EntityFrameworkCore;

namespace MovieWeb.Models
{
    public class MovieUser
    {
        public long MovieID { get; set; }
        public long UserID { get; set; }
        public Movie Movie { get; set; }
        public User User { get; set; }
    }
}
