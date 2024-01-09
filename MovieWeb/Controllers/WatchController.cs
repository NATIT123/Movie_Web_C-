using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Data;
using MovieWeb.Models;

namespace MovieWeb.Controllers
{
    public class WatchController : Controller
    {
        private readonly ILogger<WatchController> _logger;
        private MovieWebDBContext context;

        public WatchController(ILogger<WatchController> logger, MovieWebDBContext context)
        {
            _logger = logger;
            this.context = context;
        }
        public static ViewModel getViewModel(MovieWebDBContext context)
        {
            var Trending = (from movie in context.Movie orderby movie.updatedDate select movie).Take(10).ToList();
            var Genre = (from genre in context.Genre select genre).ToList();
            var Nation = (from nation in context.Nation select nation).ToList();
            ViewModel myModel = new ViewModel();
            myModel.Genres = Genre;
            myModel.Nations = Nation;
            myModel.Trending = Trending;
            return myModel;
        }
        public IActionResult Index(long? MovieID, string? Episode)
        {
            var Movie = (from movie in context.Movie
                         where movie.ID == MovieID
                         select movie).FirstOrDefault();
            if (Movie.Type == "Phim lẻ") {
                Episode = "Full";
            }
            Movie.numOfViews += 1;
            context.Update(Movie);
            context.SaveChanges();
            ViewModel myModel = getViewModel(context);
            var _Episode = (from ep in context.Episode
                            where ep.MovieID == MovieID && ep.Name == Episode
                            select ep).FirstOrDefault();
            var Episodes = (from ep in context.Episode
                           where ep.MovieID == MovieID
                           orderby ep.Name
                           select ep).ToList();
            myModel.Episodes = Episodes;
            myModel.Episode = _Episode;
            myModel.Movie = Movie;
            return View(myModel);
        }
    }
}
