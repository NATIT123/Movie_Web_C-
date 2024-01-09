using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Data;
using MovieWeb.Models;
using X.PagedList;

namespace MovieWeb.Controllers
{
    public class DetailController : Controller
    {
        private readonly ILogger<DetailController> _logger;
        private MovieWebDBContext context;

        public DetailController(ILogger<DetailController> logger, MovieWebDBContext context)
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
        public IActionResult Index(long? Id)
        {
            ViewModel myModel = getViewModel(context);
            var Movie = (from movie in context.Movie
                         where movie.ID == Id
                         select movie).FirstOrDefault();
            var Genre = (from genre in context.Genre
                         join mg in context.MovieGenre
                         on genre.ID equals mg.GenreID
                         where mg.MovieID == Id
                         select genre).ToList();
            var Episode = (from ep in context.Episode
                           where ep.MovieID == Id
                           select ep).ToList();
            myModel.Movie = Movie;
            myModel._Genres = Genre;
            myModel.Episodes = Episode;
            return View(myModel);
        }

        public IActionResult Follow(long Id)
        {
            var Email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            var _User = (from user in context.User where user.Email == Email select user).FirstOrDefault();
            var Follow = (from fl in context.Follow where fl.MovieID == Id && fl.UserID == _User.ID select fl).FirstOrDefault();
            if (Follow == null)
            {
                context.Follow.Add(new MovieUser()
                {
                    MovieID = Id,
                    UserID = _User.ID
                });
            }
            else
            {
                context.Follow.Remove(Follow);
            }
            
            context.SaveChanges();
            return RedirectToAction("Index", new {Id});
        }
    }
}
