using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWeb.Models
{
    public class MovieGenre
    {
        public long MovieID { get; set; }
        public long GenreID { get; set; }
        public Movie Movie { get; set; }
        public Genre Genre { get; set; }
    }
}
