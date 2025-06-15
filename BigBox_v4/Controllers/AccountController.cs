using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System;
using BigBox_v4.Domain;
using BigBox_v4.Data;
using BigBox_v4;

namespace BigBox_v4.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IWebSessionRepository _sessionRepo;

        public AccountController(ApplicationDBContext context, IWebSessionRepository sessionRepo)
        {
            _context = context;
            _sessionRepo = sessionRepo;
            _passwordHasher = new PasswordHasher<User>();
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View();
            }

            await _sessionRepo.RemoveUserSessionsAsync(user.Id);

            var sessionId = Guid.NewGuid().ToString();
            var sessionHash = SecurityHelpers.Hash(sessionId);
            await _sessionRepo.AddAsync(new WebSession
            {
                UserId = user.Id,
                SessionHash = sessionHash,
                ExpiresAt = DateTime.UtcNow.AddHours(1)
            });

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("IsAdmin", user.IsAdmin.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            Response.Cookies.Append("WebSession", sessionId, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

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

            model.PasswordHash = _passwordHasher.HashPassword(model, model.PasswordHash);

            _context.Users.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (Request.Cookies.TryGetValue("WebSession", out var sessionId))
            {
                var hash = SecurityHelpers.Hash(sessionId);
                await _sessionRepo.RemoveByHashAsync(hash);
                Response.Cookies.Delete("WebSession");
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Profile()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }
    }
}
