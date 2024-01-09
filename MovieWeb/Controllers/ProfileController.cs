using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Data;
using MovieWeb.IServices;
using MovieWeb.Models;

namespace MovieWeb.Controllers
{
    public class ProfileController : Controller
    {
        private readonly MovieWebDBContext _context;
        private readonly IEmailSender emailSender;

        public ProfileController(MovieWebDBContext context, IEmailSender emailSender)
        {
            _context = context;
            this.emailSender = emailSender;
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
        public IActionResult Index()
        {
            ViewModel myModel = getViewModel(_context);
            var Email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            var _User = (from user in _context.User where user.Email == Email select user).FirstOrDefault();
            myModel.User = _User;
            return View(myModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("ID,Name,Email,Password,Role,IsEmailConfirmed")] User user)
        {
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

        public async Task<IActionResult> ConfirmEmail()
        {
            ViewModel myModel = getViewModel(_context);
            var email = User.Claims.FirstOrDefault(x => x.Type == "Email").Value;
            var user = _context.User.FirstOrDefault(m => m.Email == email);
            myModel.User = user;
            var subject = "MovieWeb TTH: Xác thực Email";
            var message = GetMailBody(email);
            await emailSender.SendEmailAsync(email, subject, message);
            return View(myModel);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string email)
        {
            ViewModel myModel = getViewModel(_context);
            var user = await _context.User.FirstOrDefaultAsync(m => m.Email == email);
            if (user != null)
            {
                user.IsEmailConfirmed = true;
                _context.Update(user);
                await _context.SaveChangesAsync();
                RedirectToAction("Index", "Home");
            }
            return View(myModel);
        }

        public string GetMailBody(string email)
        {
            string url = "https://localhost:7257/Profile/ConfirmEmail/" + email;
            return string.Format(@"<div style='text-align:center;'>
                                    <h1>Chào mừng tới website của chúng tôi</h1>
                                    <h3>Nhấn nút bên dưới để xác thực Email của bạn.</h3>
                                    <form method='post' action='{0}' style='display: inline;'>
                                      <input type='hidden' id='email' name='email' value='" + email + "'/>" +
                                      "<button type = 'submit' style=' display: block; text-align: center;font-weight: bold;" +
                                      "background-color: #008CBA;font-size: 16px;border-radius: 10px;color:#ffffff; cursor:pointer;" +
                                      "width:100%;padding:10px;'>Xác thực</button></form></div>", url);
        }

        private bool UserExists(long id)
        {
            return (_context.User?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
