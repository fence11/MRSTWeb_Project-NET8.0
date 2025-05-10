using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BigBox_v4.Domain;
using BigBox_v4.Data;

namespace BigBox_v4.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AccountController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(User model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var exists = _context.Users.Any(u => u.Username == model.Username);
            if (exists)
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
