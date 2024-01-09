using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Data;
using MovieWeb.IServices;
using MovieWeb.Migrations;
using MovieWeb.Models;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace MovieWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MovieWebDBContext context;

        public HomeController(ILogger<HomeController> logger, MovieWebDBContext context)
        {
            _logger = logger;
            this.context = context;
        }
        public static ViewModel getViewModel(MovieWebDBContext context)
        {
            var Trending = (from movie in context.Movie orderby movie.realeasedYear descending select movie).Take(10).ToList();
            var Genre = (from genre in context.Genre select genre).ToList();
            var Nation = (from nation in context.Nation select nation).ToList();
            var Movie = (from movie in context.Movie orderby movie.realeasedYear descending, movie.updatedDate descending select movie).ToList();
            var Anime = (from movie in context.Movie
                         join genre in context.MovieGenre
                         on movie.ID equals genre.MovieID
                         orderby movie.realeasedYear descending
                         where genre.GenreID == 6
                         select movie).ToList();
            ViewModel myModel = new ViewModel();
            myModel.Genres = Genre;
            myModel.Nations = Nation;
            myModel.Movies = Movie;
            myModel.Trending = Trending;
            myModel.Theme = Anime;
            return myModel;
        }

        public IActionResult Index()
        {
            ViewModel myModel = getViewModel(context);
            return View(myModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}