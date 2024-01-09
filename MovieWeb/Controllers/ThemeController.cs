using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Data;
using MovieWeb.IServices;
using MovieWeb.Models;
using X.PagedList;

namespace MovieWeb.Controllers
{
    public class ThemeController : Controller
    {
        private readonly ILogger<ThemeController> _logger;
        private MovieWebDBContext context;

        public ThemeController(ILogger<ThemeController> logger, MovieWebDBContext context)
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
        public IActionResult Genre(long? Id, int? Page)
        {
            ViewModel myModel = getViewModel(context);
            var Genre = (from movie in context.Movie 
                         join genre in context.MovieGenre
                         on movie.ID equals genre.MovieID
                         orderby movie.realeasedYear descending
                         where genre.GenreID == Id select movie).ToPagedList(Page ?? 1, 20);
            var _Genre = (from genre in context.Genre where genre.ID == Id select genre).FirstOrDefault();
            myModel.Genre = _Genre;
            myModel.Theme = Genre;
            return View(myModel);
        }

        public IActionResult Nation(long? Id, int? Page)
        {
            ViewModel myModel = getViewModel(context);
            var Nation = (from movie in context.Movie orderby movie.realeasedYear descending where movie.NationID == Id select movie).ToPagedList(Page ?? 1, 20);
            var _Nation = (from nation in context.Nation where nation.ID == Id select nation).FirstOrDefault();
            myModel.Nation = _Nation;
            myModel.Theme = Nation;
            return View(myModel);
        }

        public IActionResult Movie(int? Page)
        {
            ViewModel myModel = getViewModel(context);
            var Movie = (from movie in context.Movie orderby movie.realeasedYear descending where movie.Type == "Phim lẻ" select movie).ToPagedList(Page ?? 1, 20);
            myModel.Theme = Movie;
            return View(myModel);
        }

        public IActionResult Series(int? Page)
        {
            ViewModel myModel = getViewModel(context);
            var Series = (from movie in context.Movie orderby movie.realeasedYear descending where movie.Type == "Phim bộ" select movie).ToPagedList(Page ?? 1, 20);
            myModel.Theme = Series;
            return View(myModel);
        }

        public IActionResult Follow(int? Page, long? Id)
        {
            var movieWebDBContext = context.Follow.Include(m => m.User).Include(m => m.Movie).Where(x => x.User.ID == Id).ToPagedList(Page ?? 1, 20);
            ViewModel myModel = getViewModel(context);
            myModel.Follows = movieWebDBContext;
            return View(myModel);
        }

        public IActionResult RecentlyUpdated(int? Page)
        {
            ViewModel myModel = getViewModel(context);
            var Movie = (from movie in context.Movie orderby movie.realeasedYear descending select movie).ToPagedList(Page ?? 1, 20);
            myModel.Theme = Movie;
            return View(myModel);
        }

        public IActionResult Search(string? searchStr,int? Page)
        {
            ViewModel myModel = getViewModel(context);
            var Movie = (from movie in context.Movie orderby movie.realeasedYear descending select movie);
            if (!String.IsNullOrEmpty(searchStr))
            {
                var _Movie = Movie.Where(s => s.Name!.Contains(searchStr)).ToPagedList(Page ?? 1, 20);
                myModel.Theme = _Movie;
            }
            return View(myModel);
        }
    }
}
