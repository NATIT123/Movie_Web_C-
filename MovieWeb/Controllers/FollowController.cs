using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Data;
using MovieWeb.Models;
using X.PagedList;

namespace MovieWeb.Controllers
{
    public class FollowController : Controller
    {
        private readonly ILogger<FollowController> _logger;
        private MovieWebDBContext context;

        public FollowController(ILogger<FollowController> logger, MovieWebDBContext context)
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
        public IActionResult Index(int? Page)
        {
            ViewModel myModel = getViewModel(context);
            var Email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            var _User = (from user in context.User where user.Email == Email select user).FirstOrDefault();
            var movieWebDBContext = context.Follow.Include(m => m.User).Include(m => m.Movie).Where(x => x.User.ID == _User.ID).ToPagedList(Page ?? 1, 20);
            myModel.Follows = movieWebDBContext;
            return View(myModel);
        }
    }
}
