using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MovieWeb.Models;
using System.Security.Claims;
using MovieWeb.Data;
using MovieWeb.Services;
using MovieWeb.IServices;

namespace MovieWeb.Controllers
{
    public class AuthenticationController : Controller
    {
        private MovieWebDBContext context;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IEmailSender emailSender;

        public AuthenticationController(ILogger<AuthenticationController> logger, MovieWebDBContext context, IEmailSender emailSender)
        {
            _logger = logger;
            this.context = context;
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

        public IActionResult Login()
        {
            ViewModel myModel = getViewModel(context);
            myModel.Login = new Login();
            return View(myModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login loginObj)
        {
            ViewModel myModel = getViewModel(context);
            if (ModelState.IsValid)
            {
                var _userExist = context.User.Where(x => x.Email == loginObj.Email).FirstOrDefault();
                if (_userExist == null)
                {
                    TempData["Message"] = "Tài khoản không tồn tại!";
                }
                else
                {
                    var _user = context.User.Where(x => x.Email == loginObj.Email && x.Password == loginObj.Password).FirstOrDefault();
                    if (_user != null)
                    {
                        //A claim is a statement about a subject by an issuer and    
                        //represent attributes of the subject that are useful in the context of authentication and authorization operations.
                        var claims = new List<Claim>() {
                            new Claim("Email", _user.Email),
                            new Claim("Name", _user.Name),
                            new Claim("Role", _user.Role)
                        };
                        //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                        var principal = new ClaimsPrincipal(identity);
                        //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                        {
                            AllowRefresh = true,
                            IsPersistent = true
                        });
                        return RedirectToAction("Index", "Home"); 
                    }
                    else
                    {
                        TempData["Message"] = "Sai thông tin đăng nhập!";
                    }
                }
            }
            myModel.Login = loginObj;
            return View(myModel);
        }

        public IActionResult Signup()
        {
            ViewModel myModel = getViewModel(context);
            myModel.Signup = new Signup();
            return View(myModel);
        }

        [HttpPost]
        public IActionResult Signup(Signup suObj)
        {

            ViewModel myModel = getViewModel(context);
            if (ModelState.IsValid)
            {
                var _userExist = context.User.Where(x => x.Email == suObj.Email).FirstOrDefault();
                if (_userExist != null)
                {
                    TempData["Message"] = "Email đã tồn tại!";
                }
                else
                {
                    context.User.Add(new User() { 
                        Name = suObj.Username, 
                        Email = suObj.Email,
                        Password = suObj.Password,
                        Role = "Người dùng",
                        IsEmailConfirmed = false
                    });
                    context.SaveChanges();
                    return RedirectToAction("Login","Authentication");
                }
            }
            return View(myModel);
        }

        public IActionResult ForgotPassword()
        {
            ViewModel myModel = getViewModel(context);
            myModel.ForgotPass = new ForgotPass();
            return View(myModel);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPass obj)
        {
            ViewModel myModel = getViewModel(context);
            if (ModelState.IsValid)
            {
                var _user = context.User.Where(x => x.Email == obj.Email).FirstOrDefault();
                if (_user == null)
                {
                    TempData["Message"] = "Tài khoản không tồn tại!";
                }
                else
                {
                        var subject = "MovieWeb TTH: Email lấy lại mật khẩu";
                        var message = GetMailBody(_user.Password);
                        await emailSender.SendEmailAsync(obj.Email, subject, message);
                }
            }
            myModel.ForgotPass = obj;
            return View(myModel);
        }

        public string GetMailBody(string pass)
        {
            return string.Format(@"<div style='text-align:center;'>
                                    <h1>Lấy lại mật khẩu</h1></div>
                                    <p>Mật khẩu: </p>"+pass);
        }

        public async Task<IActionResult> Logout()
        {
            //SignOutAsync is Extension method for SignOut    
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //Redirect to home page    
            return RedirectToAction("Index", "Home");
        }
    }
}
