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
    public class EpisodesController : Controller
    {
        private readonly MovieWebDBContext _context;

        public EpisodesController(MovieWebDBContext context)
        {
            _context = context;
        }

        public static ViewModel getViewModel(MovieWebDBContext context)
        {
            var Genre = (from genre in context.Genre select genre).ToList();
            var Nation = (from nation in context.Nation select nation).ToList();
            var Movie = (from movie in context.Movie select movie).ToList();
            ViewModel myModel = new ViewModel();
            myModel.Genres = Genre;
            myModel.Nations = Nation;
            myModel.Movies = Movie;
            return myModel;
        }

        // GET: Episodes
        public async Task<IActionResult> Index()
        {

            var movieWebDBContext = _context.Episode.Include(e => e.Movie).ToList();
            ViewModel myModel = getViewModel(_context);
            myModel.Episodes = movieWebDBContext;
            return View(myModel);
        }

        // GET: Episodes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Episode == null)
            {
                return NotFound();
            }

            var episode = await _context.Episode
                .Include(e => e.Movie)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (episode == null)
            {
                return NotFound();
            }


            ViewModel myModel = getViewModel(_context);
            myModel.Episode = episode;
            return View(myModel);
        }

        // GET: Episodes/Create
        public IActionResult Create()
        {
            ViewData["MovieID"] = new SelectList(_context.Movie, "ID", "Duration");
            ViewModel myModel = getViewModel(_context);
            return View(myModel);
        }

        // POST: Episodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,videoSrc,MovieID")] Episode episode)
        {
            if (ModelState.IsValid)
            {
                episode.videoSrc = "https://localhost:7257/video/" + episode.videoSrc;
                _context.Add(episode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewModel myModel = getViewModel(_context);
            myModel.Episode = episode;
            return View(myModel);
        }

        // GET: Episodes/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Episode == null)
            {
                return NotFound();
            }

            var episode = await _context.Episode.FindAsync(id);
            if (episode == null)
            {
                return NotFound();
            }
            ViewModel myModel = getViewModel(_context);
            myModel.Episode = episode;
            return View(myModel);
        }

        // POST: Episodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,videoSrc,MovieID")] Episode episode)
        {
            if (id != episode.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    episode.videoSrc = "https://localhost:7257/video/" + episode.videoSrc;
                    _context.Update(episode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EpisodeExists(episode.ID))
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
            myModel.Episode = episode;
            return View(myModel);
        }

        // GET: Episodes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Episode == null)
            {
                return NotFound();
            }

            var episode = await _context.Episode
                .Include(e => e.Movie)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (episode == null)
            {
                return NotFound();
            }

            ViewModel myModel = getViewModel(_context);
            myModel.Episode = episode;
            return View(myModel);
        }

        // POST: Episodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Episode == null)
            {
                return Problem("Entity set 'MovieWebDBContext.Episode'  is null.");
            }
            var episode = await _context.Episode.FindAsync(id);
            if (episode != null)
            {
                _context.Episode.Remove(episode);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EpisodeExists(long id)
        {
          return (_context.Episode?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
