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
    public class NationsController : Controller
    {
        private readonly MovieWebDBContext _context;

        public NationsController(MovieWebDBContext context)
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

        // GET: Nations
        public IActionResult Index()
        {
            ViewModel myModel = getViewModel(_context);
            return _context.Nation != null ?
                          View(myModel) :
                          Problem("Danh sách thông tin quốc gia phim chưa có dữ liệu.");
        }

        // GET: Nations/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Nation == null)
            {
                return NotFound();
            }

            var nation = await _context.Nation
                .FirstOrDefaultAsync(m => m.ID == id);
            if (nation == null)
            {
                return NotFound();
            }

            ViewModel myModel = getViewModel(_context);
            myModel.Nation = nation;
            return View(myModel);
        }

        // GET: Nations/Create
        public IActionResult Create()
        {
            ViewModel myModel = getViewModel(_context);
            return View(myModel);
        }

        // POST: Nations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Nation nation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewModel myModel = getViewModel(_context);
            myModel.Nation = nation;
            return View(myModel);
        }

        // GET: Nations/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Nation == null)
            {
                return NotFound();
            }

            var nation = await _context.Nation.FindAsync(id);
            if (nation == null)
            {
                return NotFound();
            }
            ViewModel myModel = getViewModel(_context);
            myModel.Nation = nation;
            return View(myModel);
        }

        // POST: Nations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name")] Nation nation)
        {
            if (id != nation.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NationExists(nation.ID))
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
            myModel.Nation = nation;
            return View(myModel);
        }

        // GET: Nations/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Nation == null)
            {
                return NotFound();
            }

            var nation = await _context.Nation
                .FirstOrDefaultAsync(m => m.ID == id);
            if (nation == null)
            {
                return NotFound();
            }

            ViewModel myModel = getViewModel(_context);
            myModel.Nation = nation;
            return View(myModel);
        }

        // POST: Nations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Nation == null)
            {
                return Problem("Danh sách thông tin quốc gia phim chưa có dữ liệu.");
            }
            var nation = await _context.Nation.FindAsync(id);
            if (nation != null)
            {
                _context.Nation.Remove(nation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NationExists(long id)
        {
          return (_context.Nation?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
