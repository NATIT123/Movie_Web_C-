using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Data;
using MovieWeb.Models;
using Newtonsoft.Json.Linq;

namespace MovieWeb.Controllers
{
    public class UsersController : Controller
    {
        private readonly MovieWebDBContext _context;

        public UsersController(MovieWebDBContext context)
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

        // GET: Users
        public IActionResult Index()
        {
            ViewModel myModel = getViewModel(_context);
            if(User.Claims.FirstOrDefault(x => x.Type == "Role")?.Value == "Chủ sở hữu"){
                var User = (from user in _context.User select user).ToList();
                myModel.Users = User;
            }
            else
            {
                var User = (from user in _context.User where user.Role == "Người dùng" select user).ToList();
                myModel.Users = User;
            }

            return _context.User != null ? 
                          View(myModel) :
                          Problem("Danh sách thông tin tài khoản chưa có dữ liệu.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewModel myModel = getViewModel(_context);
            myModel.User = user;
            return View(myModel);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewModel myModel = getViewModel(_context);
            return View(myModel);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Email,Password,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                var _userExist = _context.User.Where(x => x.Email == user.Email).FirstOrDefault();
                if (_userExist != null)
                {
                    TempData["Message"] = "Email đã tồn tại!";
                }
                else
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewModel myModel = getViewModel(_context);
            myModel.User = user;
            return View(myModel);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewModel myModel = getViewModel(_context);
            myModel.User = user;
            return View(myModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ID,Name,Email,Password,Role,IsEmailConfirmed")] User user)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.ID))
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
            myModel.User = user;
            return View(myModel);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            ViewModel myModel = getViewModel(_context);
            myModel.User = user;
            return View(myModel);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.User == null)
            {
                return Problem("Danh sách thông tin tài khoản chưa có dữ liệu.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(long id)
        {
          return (_context.User?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
