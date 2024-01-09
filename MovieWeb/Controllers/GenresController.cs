using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Data;
using MovieWeb.Models;

namespace MovieWeb.Controllers
{
    public class GenresController : Controller
    {
        private readonly MovieWebDBContext _context;

        public GenresController(MovieWebDBContext context)
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

        // GET: Genres
        public IActionResult Index()
        {
            ViewModel myModel = getViewModel(_context);
            return _context.Nation != null ?
                          View(myModel) :
                          Problem("Danh sách thông tin thể loại chưa có dữ liệu.");
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var genre = await _context.Genre
                .FirstOrDefaultAsync(m => m.ID == id);
            if (genre == null)
            {
                return NotFound();
            }

            ViewModel myModel = getViewModel(_context);
            myModel.Genre = genre;
            return View(myModel);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            ViewModel myModel = getViewModel(_context);
            return View(myModel);
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewModel myModel = getViewModel(_context);
            myModel.Genre = genre;
            return View(myModel);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            ViewModel myModel = getViewModel(_context);
            myModel.Genre = genre;
            return View(myModel);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name")] Genre genre)
        {
            if (id != genre.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.ID))
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
            ViewModel myModel = getViewModel(_context);
            myModel.Genre = genre;
            return View(myModel);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var genre = await _context.Genre
                .FirstOrDefaultAsync(m => m.ID == id);
            if (genre == null)
            {
                return NotFound();
            }

            ViewModel myModel = getViewModel(_context);
            myModel.Genre = genre;
            return View(myModel);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Genre == null)
            {
                return Problem("Entity set 'MovieWebDBContext.Genre'  is null.");
            }
            var genre = await _context.Genre.FindAsync(id);
            if (genre != null)
            {
                _context.Genre.Remove(genre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(long id)
        {
          return (_context.Genre?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
