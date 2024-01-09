using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Data;
using MovieWeb.Models;

namespace MovieWeb.Controllers
{
    public class MovieGenresController : Controller
    {
        private readonly MovieWebDBContext _context;

        public MovieGenresController(MovieWebDBContext context)
        {
            _context = context;
        }

        public static ViewModel getViewModel(MovieWebDBContext context)
        {
            var Genre = (from genre in context.Genre select genre).ToList();
            var Nation = (from nation in context.Nation select nation).ToList();
            var Movie = (from movie in context.Movie select movie).ToList();
            ViewModel myModel = new ViewModel();
            myModel.Movies = Movie;
            myModel.Genres = Genre;
            myModel.Nations = Nation;
            return myModel;
        }

        // GET: MovieGenres
        public IActionResult Index()
        {
            var movieWebDBContext = _context.MovieGenre.Include(m => m.Genre).Include(m => m.Movie).ToList();
            ViewModel myModel = getViewModel(_context);
            myModel.MGs = movieWebDBContext;
            return View(myModel);
        }

        // GET: MovieGenres/Create
        public IActionResult Create()
        {
            ViewData["GenreID"] = new SelectList(_context.Genre, "ID", "Name");
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Name");
            ViewModel myModel = getViewModel(_context);
            return View(myModel);
        }

        // POST: MovieGenres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MGViewModel mgvm)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in mgvm.GenreID)
                {
                    var _mg = (from mg in _context.MovieGenre
                               where mg.MovieID == mgvm.MovieID
                               && mg.GenreID == item
                               select mg).FirstOrDefault();
                    if(_mg == null)
                    {
                        _context.MovieGenre.Add(new MovieGenre()
                        {
                            MovieID = mgvm.MovieID,
                            GenreID = item
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewModel myModel = getViewModel(_context);
            myModel.MG = null;
            return View(myModel);
        }

        // GET: MovieGenres/Delete/5
        public IActionResult Delete(long? movieid, long? genreid)
        {
            if (_context.MovieGenre == null)
            {
                return NotFound();
            }

            var movieGenre = (from mg in _context.MovieGenre
                     where mg.MovieID == movieid && mg.GenreID == genreid
                     select mg).FirstOrDefault();
            if (movieGenre == null)
            {
                return NotFound();
            }
            ViewModel myModel = getViewModel(_context);
            myModel.MG = movieGenre;
            return View(myModel);
        }

        // POST: MovieGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long movieid, long genreid)
        {
            if (_context.MovieGenre == null)
            {
                return Problem("Entity set 'MovieWebDBContext.MovieGenre'  is null.");
            }
            var movieGenre = (from mg in _context.MovieGenre
                              where mg.MovieID == movieid && mg.GenreID == genreid
                              select mg).FirstOrDefault();
            if (movieGenre != null)
            {
                _context.MovieGenre.Remove(movieGenre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieGenreExists(long id)
        {
          return (_context.MovieGenre?.Any(e => e.MovieID == id)).GetValueOrDefault();
        }
    }
}
