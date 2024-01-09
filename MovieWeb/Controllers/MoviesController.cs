using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Data;
using MovieWeb.Models;

namespace MovieWeb.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieWebDBContext _context;

        public MoviesController(MovieWebDBContext context)
        {
            _context = context;
        }

        public static ViewModel getViewModel(MovieWebDBContext context)
        {
            var Genre = (from genre in context.Genre select genre).ToList();
            var Nation = (from nation in context.Nation select nation).ToList();
            ViewModel myModel = new ViewModel();
            myModel.Genres = Genre;
            myModel.Nations = Nation;
            return myModel;
        }

        // GET: Movies
        public IActionResult Index()
        {
            var movieWebDBContext = _context.Movie.Include(m => m.Nation).ToList();
            ViewModel myModel = getViewModel(_context);
            myModel.Movies = movieWebDBContext;
            return View(myModel);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Nation)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewModel myModel = getViewModel(_context);
            myModel.Movie = movie;
            return View(myModel);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["NationID"] = new SelectList(_context.Nation, "ID", "ID");
            ViewModel myModel = getViewModel(_context);
            return View(myModel);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description,Status,updatedDate,Duration,realeasedYear,numOfLikes,numOfViews,Rating,Type,imgSrc,NationID")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.imgSrc = "https://localhost:7257/img/Movies/" + movie.imgSrc;
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NationID"] = new SelectList(_context.Nation, "ID", "ID", movie.NationID);
            ViewModel myModel = getViewModel(_context);
            myModel.Movie = movie;
            return View(myModel);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["NationID"] = new SelectList(_context.Nation, "ID", "ID", movie.NationID);
            ViewModel myModel = getViewModel(_context);
            myModel.Movie = movie;
            return View(myModel);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,Description,Status,updatedDate,Duration,realeasedYear,numOfLikes,numOfViews,Rating,Type,imgSrc,NationID")] Movie movie)
        {
            if (id != movie.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    movie.imgSrc = "https://localhost:7257/img/Movies/" + movie.imgSrc;
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NationID"] = new SelectList(_context.Nation, "ID", "ID", movie.NationID);
            ViewModel myModel = getViewModel(_context);
            myModel.Movie = movie;
            return View(myModel);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .Include(m => m.Nation)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            ViewModel myModel = getViewModel(_context);
            myModel.Movie = movie;
            return View(myModel);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'MovieWebDBContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(long id)
        {
          return (_context.Movie?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
